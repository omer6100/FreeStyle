﻿using System;
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
    public class AlbumController : Controller
    {
        private MusicContext db = new MusicContext();

        // GET: Album
        public ActionResult Index()
        {
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
            return View(album);

        }

        public ActionResult Create()
        {
            return View();
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
                    int? albumId =existingAlbum.Id;
                    return RedirectToAction("Details", new { id = albumId });
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

            if (Session["Role"] == null || Session["Role"].IfNotNull(role=>role.Equals("User")))
            {
                return RedirectToAction("Index", "Home");
            }

            Album album = db.Albums.Find(id);
            if (album == null)
            {
                return HttpNotFound();
            }
            return View(album);
        }

        // POST: Album/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Artist,ReleaseDate, Genre, PageViews")] Album album)
        {
            if (ModelState.IsValid)
            {
                db.Entry(album).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
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

            if (Session["Role"] == null || Session["Role"].IfNotNull(role => role.Equals("User")))
            {
                return RedirectToAction("Index", "Home");
            }

            Album album = db.Albums.Find(id);
            if (album == null)
            {
                return HttpNotFound();
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
    }
}
