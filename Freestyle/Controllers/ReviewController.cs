using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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
            return PartialView("ReviewTablePartialView", db.Reviews.AsEnumerable());
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
            return View();
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
                        return View();
                    }

                    int count = db.Reviews.Count(r => r.AlbumId == album.Id);

                    album.AvgScore = ((album.AvgScore * count) + review.Score) / (count + 1);
                    review.UserId = int.Parse(Session["UserId"].IfNotNull(uid => uid.ToString()));
                    review.Username = Session["Username"].ToString();
                    review.AlbumId = album.Id;
                        
                    db.Reviews.Add(review);
                    db.SaveChanges();
                    return RedirectToAction("Details", new{id=review.Id});
                }
                else
                {
                    return RedirectToAction("SignIn", "EndUser");
                }
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
            return View(review);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserId,AlbumId,Text,Score")] Review review)
        {
            if (ModelState.IsValid)
            {
                db.Entry(review).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
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
            return View(review);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Review review = db.Reviews.Find(id);
            Album album = db.Albums.FirstOrDefault(a=>a.Id==review.AlbumId);
            int count = db.Reviews.Count(r => r.AlbumId == album.Id);
            album.AvgScore = (album.AvgScore * count - review.Score) / (count - 1);
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
    }
}
