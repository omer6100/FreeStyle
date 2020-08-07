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
                            
                        }
                        if (search.secondaryName != null)
                        {
                            var artist = db.Artists.Where(a => a.Name == search.secondaryName).FirstOrDefault();
                            if (artist == null)
                            {
                                ModelState.AddModelError("secondaryName", "You cannot search for an artist that does not exist");
                                return View(search);
                            }

                        }

                        search.ScoreLowerBound = search.ScoreLowerBound == null ? 0 : search.ScoreLowerBound;
                        search.ScoreUpperBound = search.ScoreUpperBound == null ? 10 : search.ScoreUpperBound;

                        if (search.ScoreLowerBound > search.ScoreUpperBound)
                        {
                            ModelState.AddModelError("ScoreLowerBound", "Invalid Range");
                            return View(search);
                        }

                        search.primaryName  = search.primaryName == null ? "" : search.primaryName;
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
                                       where element.Title.Contains(search.primaryName)
                                         && element.Artist.Contains(artistName)//#Critical Difference! (contain sides was opposite) 
                                         && search.ScoreLowerBound <= element.AvgScore
                                         && element.AvgScore <= search.ScoreUpperBound
                                         && element.Genre.Contains(genre)
                                         
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
                    
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    case "Artist":
                        if (search.primaryName != null)
                        {
                            var artist = db.Artists.FirstOrDefault(a => a.Name == search.primaryName);

                            if (artist != null)
                            {
                                return RedirectToAction("Details", "Artist", new {id = artist.Id});
                            }

                        }

                        search.ScoreLowerBound = search.ScoreLowerBound == null ? 0 : search.ScoreLowerBound;
                        search.ScoreUpperBound = search.ScoreUpperBound == null ? 10 : search.ScoreUpperBound;

                        if (search.ScoreLowerBound > search.ScoreUpperBound)
                        {
                            ModelState.AddModelError("ScoreLowerBound", "Invalid Range");
                            return View(search);
                        }

                        var originCountry = search.GenreCountry != null ? search.GenreCountry : "";//#name changed
                        var name = search.primaryName == null ? "" : search.primaryName;

                        var artistQuery = db.Albums.Join(db.Artists,
                                                    album => album.ArtistId,
                                                    artist => artist.Id,
                                                    (album, artist) => new
                                                    {
                                                        //#different names
                                                        AlbumId = album.Id,//
                                                        Name = album.Artist,//
                                                        AlbumTitle = album.Title,//
                                                        ArtistId = artist.Id,
                                                        ReleaseDate = album.ReleaseDate,
                                                        AlbumAvgScore = album.AvgScore,//
                                                        Genre = album.Genre,
                                                        PageViewsAlbum = album.PageViews,
                                                        PageViewsArtist = artist.PageViews,
                                                        OriginCountry = artist.OriginCountry,
                                                        AvgScoreArtist = artist.AvgScore
                                                    });


                        var artistResults = (from element in artistQuery
                                       where
                                            element.Name.Contains(name)
                                         && search.ScoreLowerBound <= element.AvgScoreArtist
                                         && element.AvgScoreArtist <= search.ScoreUpperBound
                                         && element.OriginCountry.Contains(originCountry)

                                       select new
                                       {
                                           //#different items were selected => the ones that matches the Artitst's model
                                           Id = element.ArtistId,
                                           Name = element.Name,
                                           OriginCountry = element.OriginCountry,
                                           PageViews = element.PageViewsAlbum,
                                           AvgScore = element.AvgScoreArtist     
                                       }).ToList();


                        var resultsAsArtist = new List<Artist>();
                        artistResults.ForEach(anon => {
                            var a = new Artist
                            {
                                Id = anon.Id,  
                                Name = anon.Name,
                                OriginCountry=anon.OriginCountry,
                                PageViews = anon.PageViews,
                                AvgScore = anon.AvgScore

                            };
                            resultsAsArtist.Add(a);
                        });

                        TempData["results2"] = resultsAsArtist;
                        break;

                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    case "Review":
                        if (search.primaryName != null)
                        {
                            var album = db.Albums.Where(a => a.Title == search.primaryName).FirstOrDefault();
                            if (album != null)
                            {
                                return RedirectToAction("GetReviewsByAlbum", "Review", new { id = album.Id });
                            }

                        }
                        if (search.secondaryName != null)
                        {
                            var writtenBy___ = db.Reviews.Where(usr => usr.Username == search.secondaryName).FirstOrDefault();
                            if (writtenBy___ == null)
                            {
                                ModelState.AddModelError("secondaryName", "You cannot search for an user name that does not exist");
                                return View(search);
                            }

                        }

                        search.ScoreLowerBound = search.ScoreLowerBound == null ? 0 : search.ScoreLowerBound;
                        search.ScoreUpperBound = search.ScoreUpperBound == null ? 10 : search.ScoreUpperBound;

                        if (search.ScoreLowerBound > search.ScoreUpperBound)
                        {
                            ModelState.AddModelError("ScoreLowerBound", "Invalid Range");
                            return View(search);
                        }

                        //search.primaryName = search.primaryName == null ? "" : search.primaryName;
                        var writtenBy = search.secondaryName != null ? search.secondaryName : "";//#name changed
                        var albumTitle = search.primaryName == null ? "" : search.primaryName;

                        var reviewQuery = db.Reviews.GroupBy(review=>review.Username);


                        var reviewResults = new List<Review>();
                        reviewQuery.Where(element => element.Key.Contains(writtenBy)).ForEach(group=>
                        {
                            var relavent = group.Where(

                                 element => 
                                   element.AlbumTitle.Contains(albumTitle)
                                && search.ScoreLowerBound <= element.Score
                                && element.Score <= search.ScoreUpperBound);
                            
                            relavent.ForEach(entity =>
                            {
                                reviewResults.Add(new Review
                                {
                                    Score = entity.Score,
                                    Id = entity.Id,
                                    AlbumTitle = entity.AlbumTitle,
                                    AlbumId = entity.AlbumId,
                                    UserId = entity.UserId,
                                    Username = entity.Username,
                                    Text = entity.Text
                                }); 
                            });
                        });

                        TempData["results3"] = reviewResults;
                        break;
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    default:
                        break;
                }

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
