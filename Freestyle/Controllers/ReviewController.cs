using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Freestyle.Contexts;
using Freestyle.Models;
using Microsoft.Ajax.Utilities;

namespace Freestyle.Controllers
{
    public class ReviewController : Controller
    {
        private MusicContext db = new MusicContext();

        // GET: Reviews
        public ActionResult Index()
        {
            return View(db.Reviews.ToList());
        }

        public ActionResult GetReviewPartial()
        {
            return PartialView("Tables/ReviewTablePartialView", db.Reviews.AsEnumerable());
        }

        public ActionResult GetReviewsByAlbum(int id)
        {
            return PartialView("Tables/ReviewTablePartialView", db.Reviews.Where(r => r.AlbumId == id).AsEnumerable());
        }
        public ActionResult GetReviewsByArtist(int id)
        {
            var reviews = new List<Review>();
            var albums = db.Albums.Where(album => album.ArtistId == id).ToList();
            if (albums.Count > 0)
            {
                albums.ForEach(a =>
                {
                    reviews.AddRange(db.Reviews.Where(r=>r.AlbumId == a.Id).AsEnumerable());
                });
                
            }

            return PartialView("Tables/ReviewTablePartialView", reviews);
        }

        public ActionResult GetReviewsByUser(int id)
        {
            return PartialView("Tables/ReviewTablePartialView", db.Reviews.Where(r => r.UserId == id).AsEnumerable());
        }

        // GET: Reviews/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }

        // GET: Reviews/Create
        public ActionResult Create(int? albumId)
        {
            if (albumId != null)
            {
                var albumTitle = "temp";
                db.Albums.Where(a => a.Id == albumId).Select(a => new {title = a.Title}).SingleOrDefault().IfNotNull(t=>albumTitle = t.title);
                return View(new Review{AlbumTitle = albumTitle});
            }
            return View(new Review { AlbumTitle = null });
        }

        
        // POST: Reviews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AlbumTitle,Text,Score")] Review review)
        {
            if (ModelState.IsValid)
            {
                if (Session["Authorized"].IfNotNull(a => a.Equals(true)))
                {
                    var album = db.Albums.FirstOrDefault(a => a.Title == review.AlbumTitle);
                    if (album == null)
                    {
                        ModelState.AddModelError("AlbumTitle","You cannot Review a non-existent Album");
                        return View();
                    }
                    int count = db.Reviews.Count(r => r.AlbumId == album.Id);

                    var artist = db.Artists.FirstOrDefault(a => a.Id == album.ArtistId);
                    if (artist == null)
                    {
                        return View();
                    }

                    int x = 0;
                    db.Albums.ForEach(a =>
                    {
                        if (a.ArtistId == artist.Id) x += db.Reviews.Count(r => r.AlbumId == a.Id);
                    });

                    album.AvgScore = ((album.AvgScore * count) + review.Score) / (count + 1);
                    artist.AvgScore = ((artist.AvgScore * x) + review.Score) / (x + 1);

                    review.UserId = int.Parse(Session["UserId"].IfNotNull(uid => uid.ToString()));
                    review.Username = Session["Username"].ToString();
                    review.AlbumId = album.Id;
                        
                    db.Reviews.Add(review);
                    db.SaveChanges();
                    return RedirectToAction("Details", new{id=review.Id});
                }

                return RedirectToAction("SignIn", "EndUser");
                
            }

            return View(review);
        }

        // GET: Reviews/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            if (Session["Authorized"] == null || Session["Role"].IfNotNull(role => role.Equals("User")) && Session["UserId"].IfNotNull(uid => !uid.Equals(id)))
            {
                return RedirectToAction("Details", new { id });
            }

            return View(review);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserId,Username,AlbumId,AlbumTitle,Text,Score")] Review review)
        {
            if (ModelState.IsValid)
            {
                //
                //int oldScore = db.Entry(review).Entity.Score;
                int oldScore = db.Reviews.FirstOrDefault(r => r.Id == review.Id).Score;
                
                var album = db.Albums.FirstOrDefault(a => a.Id == review.AlbumId);
                if (album == null)
                {
                    return RedirectToAction("Details", new { id = review.Id });
                }
                var artist = db.Artists.FirstOrDefault(a => a.Id == album.ArtistId);
                if (artist == null)
                {
                    return RedirectToAction("Details", new{id=review.Id});
                }

                int albumReviewCount = db.Reviews.Count(r => r.AlbumId == album.Id);
                int artistReviewCount = 0;
                db.Albums.ForEach(a =>
                {
                    if (a.ArtistId == artist.Id) artistReviewCount += db.Reviews.Count(r => r.AlbumId == a.Id);
                });
                album.AvgScore = ((album.AvgScore * albumReviewCount) + review.Score - oldScore) / (albumReviewCount);
                artist.AvgScore = ((artist.AvgScore * artistReviewCount) + review.Score - oldScore) / (artistReviewCount);

                db.Reviews.AddOrUpdate(review);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = review.Id });
            }
            return View(review);
        }

        // GET: Reviews/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           

            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            if (Session["Authorized"] == null || (Session["Role"].IfNotNull(role => role.Equals("User")) && Session["UserId"].IfNotNull(uid => !uid.Equals(review.UserId))))
            {
                return RedirectToAction("Details", new {id });
            }
            
            return View(review);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Review review = db.Reviews.Find(id);
            Album album = db.Albums.FirstOrDefault(a=>a.Id==review.AlbumId);
            Artist artist = db.Artists.FirstOrDefault(a => a.Id == album.ArtistId);
            int count = db.Reviews.Count(r => r.AlbumId == album.Id);
            var x = 0;

            db.Albums.Where(a => a.ArtistId == artist.Id).ForEach(a =>
            {
                x += db.Reviews.Count(r => r.AlbumId == a.Id);
            });

            if (count == 1)
            {
                album.AvgScore = 0;
            }
            else
            {
                album.AvgScore = (album.AvgScore * count - review.Score) / (count - 1);
            }

            if (x == 1)
            {
                artist.AvgScore = 0;
            }
            else
            {
                artist.AvgScore = ((artist.AvgScore * x) - review.Score) / (x - 1);
            }

            album.AvgScore = (album.AvgScore * count - review.Score) / (count - 1);
            artist.AvgScore = ((artist.AvgScore * x) - review.Score) / (x - 1);

            db.Reviews.Remove(review);
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
            var list = TempData["results"];
            return View((IEnumerable<Review>)list);
        }
    }
}
