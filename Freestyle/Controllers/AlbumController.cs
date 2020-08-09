using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Freestyle.Contexts;
using Freestyle.Models;
using Microsoft.Ajax.Utilities;

namespace Freestyle.Controllers
{
    public class AlbumController : Controller
    {
        private MusicContext db = new MusicContext();

        // GET: Album
        public ActionResult Index()
        {
            var albumQuery = db.Albums.GroupBy(alb => alb.Title, sc => sc.AvgScore, (alb, sc) => new { title = alb, score = sc });
            ViewBag.albums = new JavaScriptSerializer().Serialize(albumQuery);

            return View(db.Albums.ToList().OrderBy(a=>a.Title));
        }

        public IEnumerable<Album> GetAlbums()
        {
            return db.Albums.AsEnumerable();
        }

        public ActionResult GetAlbumsPartial()
        {
            return PartialView("Tables/AlbumTablePartialView", GetAlbums());
        }

        public ActionResult GetRecommended()
        {
            if (Session["Last Album Visits"] != null)
            {
                var visits = ((List<Album>)Session["Last Album Visits"]).AsQueryable();
                var genre = visits.GroupBy(album => album.Genre)
                    .OrderByDescending(group => group.Count())
                    .First().Key;

                var recommendedAlbum = db.Albums.Where(album => album.Genre == genre).OrderBy(album => Guid.NewGuid())
                    .First();

                return PartialView("RecommendDetails", new Album
                {
                    Id = recommendedAlbum.Id,
                    Artist = recommendedAlbum.Artist,
                    ArtistId = recommendedAlbum.ArtistId,
                    Title = recommendedAlbum.Title,
                    ReleaseDate = recommendedAlbum.ReleaseDate,
                    Genre = recommendedAlbum.Genre,
                    AvgScore = recommendedAlbum.AvgScore
                });
            }

            return null;
        }

        // GET: Album/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album album = db.Albums.Find(id);
            if (album == null)
            {
                return HttpNotFound();
            }

            album.PageViews++;
            db.SaveChanges();

            if (Session["Last Album Visits"] != null)
            {
                var list = ((List<Album>)Session["Last Album Visits"]);
                if (list.Count == list.Capacity)
                {
                    list.RemoveAt(list.Count - 1);
                }
                list.Insert(0, album);
            }
            //graph : x-> reviewDate , y-> reviewScore
            //rCount => rCount, (r, sc) => new { date = r, score = sc });
            //var reviewQuery = db.Reviews.GroupBy(review => review.AlbumTitle, (album, x) => new { AlbumName = album, count = x.Count()});

            var reviewScoreQuery = db.Reviews.Where(a => a.AlbumId == id).GroupBy(rScore => rScore.Score, (s, c) => new { reviewScore = s, count = c.Count() });
            ViewBag.reviews = new JavaScriptSerializer().Serialize(reviewScoreQuery);
            //
            return View(album);

        }

        public ActionResult Create(int? artistId)
        {
            var name = "";
            if (artistId != null)
            {
                var artist = db.Artists.FirstOrDefault(a => a.Id == artistId);
                if (artist != null)
                {
                    name = artist.Name;
                }
            }
            return View(new Album { Artist = name, ReleaseDate = new DateTime(1970, 1, 1) });
        }

        // POST: Album/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Title,Artist,ReleaseDate, Genre")] Album album)
        {
            if (ModelState.IsValid)
            {
                var existingAlbum = db.Albums.Where(a => a.Title == album.Title && a.Artist == album.Artist)
                    .Select(a => new {a.Id}).SingleOrDefault();

                if (existingAlbum == null)
                {
                    if (album.ReleaseDate > DateTime.Now)
                    {
                        ModelState.AddModelError("ReleaseDate", "You can't Create an Album That isn't Released Yet");
                        return View(album);
                    }

                    var existingArtist = db.Artists.Where(a => a.Name == album.Artist).
                        Select(a => new {a.Id}).SingleOrDefault();

                    if (existingArtist == null)
                    {
                        var artist = new Artist{Name=album.Artist, OriginCountry = "Not Available"};
                        db.Artists.Add(artist);
                        db.SaveChanges();

                        album.ArtistId = artist.Id;
                        db.Albums.Add(album);
                        db.SaveChanges();
                        
                        return RedirectToAction("Details", new {id = album.Id});
                        //return Details(albumId);
                    }
                    else
                    {
                        album.ArtistId = existingArtist.Id;
                        db.Albums.Add(album);

                        db.SaveChanges();
                        return RedirectToAction("Details", new { id = album.Id });
                    }
                }
                else
                {
                    return RedirectToAction("Details", new { id = existingAlbum.Id });
                }
            }

            return View(album);
        }

        // GET: Album/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album album = db.Albums.Find(id);
            if (album == null)
            {
                return HttpNotFound();
            }
            if (Session["Role"] == null || Session["Role"].IfNotNull(role=>role.Equals("User")))
            {
                return RedirectToAction("Details", new{id=album.Id});
            }

            
            return View(album);
        }

        // POST: Album/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Artist,ReleaseDate, Genre, PageViews, ArtistId, AvgScore")] Album album)
        {
            if (ModelState.IsValid)
            {
                album.PageViews =  album.PageViews < 0 ? 0 : album.PageViews;
                db.Entry(album).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new{id=album.Id});
            }
            return View(album);
        }

        // GET: Album/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album album = db.Albums.Find(id);
            if (album == null)
            {
                return HttpNotFound();
            }
            if (Session["Role"] == null || Session["Role"].IfNotNull(role => role.Equals("User")))
            {
                return RedirectToAction("Details", new {id=album.Id});
            }
            return View(album);
        }

        // POST: Album/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Album album = db.Albums.Find(id);

            Artist artist = db.Artists.Find(album.ArtistId);
            var reviews = db.Reviews.Where(r => r.AlbumId == album.Id);

            if (reviews.ToList().Count == 0)
            {
                db.Albums.Remove(album);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var sum = reviews.Sum(r => r.Score);
            var reviewCount = reviews.ToList().Count;
            int totalCount = 0;

            db.Albums.Where(a => a.ArtistId == artist.Id).ForEach(a =>
            {
                totalCount += db.Reviews.Count(r => r.AlbumId == a.Id);
            });

            if (totalCount - reviewCount == 0)
            {
                artist.AvgScore = 0;

            }
            else
            {
                artist.AvgScore = (artist.AvgScore * totalCount - sum) / (totalCount - reviewCount);
            }

            db.Reviews.RemoveRange(reviews);
            db.Albums.Remove(album);
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
        public ActionResult SearchResult()
        {
            List<Album> list = (List<Album>)TempData["results"];
            if(list == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(list.AsEnumerable());
        }
    }
}
