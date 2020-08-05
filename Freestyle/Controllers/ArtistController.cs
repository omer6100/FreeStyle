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
    public class ArtistController : Controller
    {
        private MusicContext db = new MusicContext();

        // GET: Artists
        public ActionResult Index()
        {
            return View(db.Artists.ToList());
        }

        // GET: Artists/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Artist artist = db.Artists.Find(id);
            if (artist == null)
            {
                return HttpNotFound();
            }
            artist.PageViews++;
            db.SaveChanges();
            return View(artist);
        }

        // GET: Artists/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Artists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name, OriginCountry")] Artist artist)
        {
            if (ModelState.IsValid)
            {
                var existingArtist = db.Artists.Where(a => a.Name == artist.Name && a.OriginCountry == artist.OriginCountry)
                    .Select(a => new {a.Id}).SingleOrDefault();
                if (existingArtist == null)
                {
                    db.Artists.Add(artist);
                    db.SaveChanges();
                    return RedirectToAction("Details", new {id = artist.Id});
                }

                return RedirectToAction("Details", new { id = existingArtist.Id });
            }

            return View(artist);
        }

        // GET: Artists/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (Session["Role"] == null || Session["Role"].IfNotNull(role => role.Equals("User")))
            {
                return RedirectToAction("Index", "Home");
            }
            Artist artist = db.Artists.Find(id);
            if (artist == null)
            {
                return HttpNotFound();
            }
            return View(artist);
        }

        // POST: Artists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,OriginCountry,PageViews")] Artist artist)
        {
            if (ModelState.IsValid)
            {
                db.Entry(artist).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = artist.Id });
            }

            return View(artist);
        }

        // GET: Artists/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (Session["Role"] == null || Session["Role"].IfNotNull(role => role.Equals("User")))
            {
                return RedirectToAction("Index", "Home");
            }
            Artist artist = db.Artists.Find(id);
            if (artist == null)
            {
                return HttpNotFound();
            }
            return View(artist);
        }

        // POST: Artists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Artist artist = db.Artists.Find(id);

            var albums = db.Albums.Where(a => a.ArtistId == artist.Id);
            if (albums.ToList().Count == 0)
            {
                db.Artists.Remove(artist);
                return RedirectToAction("Index");
            }

            List<Review> reviews = new List<Review>();

            db.Reviews.ForEach(r =>
            {
                albums.ForEach(a =>
                {
                    if (r.AlbumId == a.Id) reviews.Add(r);
                });
            });


            db.Artists.Remove(artist);
            db.Reviews.RemoveRange(reviews.AsEnumerable());
            db.Albums.RemoveRange(albums);
            db.Artists.Remove(artist);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult GetDiscog(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            var discog = db.Albums.Where(a => a.ArtistId == id).ToList();

            
            return PartialView("Tables/AlbumTablePartialView",discog);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult GetArtistsPartial()
        {
            return PartialView("Tables/ArtistTablePartialView", db.Artists.AsEnumerable());
        }
    }
}
