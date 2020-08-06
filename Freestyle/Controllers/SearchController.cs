using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Freestyle.Contexts;
using Freestyle.Models;
using Microsoft.Ajax.Utilities;

namespace Freestyle.Controllers
{
    public class SearchController : Controller
    {
        private MusicContext db = new MusicContext();

        // GET: Search
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "type,primaryName,secondaryName,ScoreLowerBound,GenreCountry,ScoreUpperBound")] Search search)
        {

            if (ModelState.IsValid)
            {


                switch (search.type)
                {
                    case "Album":

                        if (search.primaryName != null)
                        {
                            var album = db.Albums.Where(a => a.Title == search.primaryName).FirstOrDefault();
                            if (album != null)
                            {
                                return RedirectToAction("Details", "Album", new { id = album.Id });
                            }
                            //ModelState.AddModelError("primaryName", "You cannot search for an album that does not exist");
                            //return View(search);
                        }

                        if (search.secondaryName != null)
                        {
                            var artist = db.Artists.Where(a => a.Name == search.secondaryName).FirstOrDefault();
                            if (artist == null)
                            {
                                ModelState.AddModelError("secondaryName", "You cannot search for an artist that does not exist");
                                return View(search);
                            }

                            //var albumsByArtist = db.Albums.Where(a => a.Artist == artist.Name);
                            //if (albumsByArtist.ToList().Count ==  0)
                            //{
                            //    return RedirectToAction("SearchResult", "Album", new {list = albumsByArtist.ToList()});
                            //} 
                        }

                        if (search.ScoreLowerBound > search.ScoreUpperBound)
                        {
                            ModelState.AddModelError("ScoreLowerBound", "Invalid Range");
                            return View(search);
                        }

                        var genre = search.GenreCountry != null ? search.GenreCountry : "";
                        var artistName = search.secondaryName == null ? "" : search.secondaryName;

                        var query = db.Albums.Join(db.Artists,
                                                    album => album.ArtistId,
                                                    artist => artist.Id,
                                                    (album, artist) => new
                                                    {
                                                        Id = album.Id,
                                                        Artist = album.Artist,
                                                        Title = album.Title,
                                                        ArtistId = artist.Id,
                                                        ReleaseDate = album.ReleaseDate,
                                                        AvgScore = album.AvgScore,
                                                        Genre = album.Genre,
                                                        PageViewsAlbum = album.PageViews,
                                                        PageViewsArtist = artist.PageViews,
                                                        OriginCountry = artist.OriginCountry,
                                                        AvgScoreArtist = artist.AvgScore
                                                    });


                        var results = (from element in query
                                       where element.Artist.Contains(artistName)
                                         && search.ScoreLowerBound <= element.AvgScore
                                         && element.AvgScore <= search.ScoreUpperBound
                                         && search.GenreCountry.Equals(element.Genre)
                                       select new
                                       {
                                           Id = element.Id,
                                           Artist = element.Artist,
                                           Title = element.Title,
                                           ReleaseDate = element.ReleaseDate,
                                           Genre = element.Genre,
                                           PageViews = element.PageViewsAlbum,
                                           AvgScore = element.AvgScore,
                                           ArtistId = element.ArtistId
                                       }).ToList();

                        //var results = query.Where(albumArtist => (search.ScoreLowerBound <= albumArtist.AvgScore && albumArtist.AvgScore <= search.ScoreUpperBound)
                        //                            && (search.GenreCountry.Equals(albumArtist.Genre)) && )
                        //    .Select(albumArtist => new
                        //    {
                        //        Id = albumArtist.Id,
                        //        Artist = albumArtist.Artist,
                        //        Title = albumArtist.Title,
                        //        ReleaseDate = albumArtist.ReleaseDate,
                        //        Genre = albumArtist.Genre,
                        //        PageViews = albumArtist.PageViewsAlbum,
                        //        AvgScore = albumArtist.AvgScore,
                        //        ArtistId = albumArtist.ArtistId
                        //    }).ToList();

                        var resultsAsAlbums = new List<Album>();
                        results.ForEach(anon => {
                            var a = new Album
                            {
                                Id = anon.Id,
                                Artist = anon.Artist,
                                Title = anon.Title,
                                ReleaseDate = anon.ReleaseDate,
                                Genre = anon.Genre,
                                PageViews = anon.PageViews,
                                AvgScore = anon.AvgScore,
                                ArtistId = anon.ArtistId
                            };
                            resultsAsAlbums.Add(a);
                        });

                        TempData["results"] = resultsAsAlbums;
                        break;
                    //var albList = query.Where(alb => )



                    //return RedirectToAction(SearchResults,Album);
                    //return PartialView("Tables/AlbumTablePartialView", join.AsEnumerable());



                    case "Artist":
                        return PartialView("Tables/ArtistTablePartialView", db.Artists.AsEnumerable());
                    case "Review":
                        return PartialView("Tables/ReviewTablePartialView", db.Reviews.AsEnumerable());
                    default:
                        break;
                }

                //db.Searches.Add(search);
                //db.SaveChanges();
                db.SaveChanges();
                return RedirectToAction("SearchResult", search.type);
            }

            return View(search);
        }
        // GET: Search/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Search search = db.Searches.Find(id);
            if (search == null)
            {
                return HttpNotFound();
            }
            return View(search);
        }

        // GET: Search/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Search/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "type,primaryName,secondaryName,ScoreLowerBound,GenreCountry")] Search search)
        {
            if (ModelState.IsValid)
            {
                db.Searches.Add(search);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(search);
        }

        // GET: Search/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Search search = db.Searches.Find(id);
            if (search == null)
            {
                return HttpNotFound();
            }
            return View(search);
        }

        // POST: Search/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "type,primaryName,secondaryName,ScoreLowerBound,GenreCountry")] Search search)
        {
            if (ModelState.IsValid)
            {
                db.Entry(search).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(search);
        }

        // GET: Search/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Search search = db.Searches.Find(id);
            if (search == null)
            {
                return HttpNotFound();
            }
            return View(search);
        }

        // POST: Search/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Search search = db.Searches.Find(id);
            db.Searches.Remove(search);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
