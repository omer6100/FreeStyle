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
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
namespace Freestyle.Controllers
{
    public class AdvancedSearchController : Controller
    {
        private MusicContext db = new MusicContext();

       
        //public ActionResult Index()
        //{

        //    return View(db.Albums.ToList());
        //}
        public ActionResult Index(string jBy)
        {
            if(jBy == "artist")
            {
                var join =
                 from alb in db.Albums

                 join art in db.Artists on alb.ArtistId equals art.Id

                 select new { alb.Artist, alb.AvgScore };

                var albumList = new List<Album>();
                foreach (var t in join)
                {
                    albumList.Add(new Album()
                    {
                        Artist = t.Artist,
                        AvgScore = t.AvgScore
                    });
                }
                return View(albumList);
            }
            return View();
        }
            //#######################################################
            public ActionResult Result()
        {
            var join =
                 from alb in db.Albums
                 
                 join art in db.Artists on alb.ArtistId equals art.Id
                 
                 select new { alb.Artist, alb.AvgScore};

            var albumList = new List<Album>();
            foreach (var t in join)
            {
                albumList.Add(new Album()
                {
                    Artist = t.Artist,
                    AvgScore = t.AvgScore
                });
            }
            return View(albumList);
        }


        // GET: AdvancedSearch/Details/5
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
            return View(album);
        }

        // GET: AdvancedSearch/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdvancedSearch/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Artist,ArtistId,ReleaseDate,Genre,AvgScore,PageViews")] Album album)
        {
            if (ModelState.IsValid)
            {
                db.Albums.Add(album);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(album);
        }

        // GET: AdvancedSearch/Edit/5
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
            return View(album);
        }

        // POST: AdvancedSearch/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Artist,ArtistId,ReleaseDate,Genre,AvgScore,PageViews")] Album album)
        {
            if (ModelState.IsValid)
            {
                db.Entry(album).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(album);
        }

        // GET: AdvancedSearch/Delete/5
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
            return View(album);
        }

        // POST: AdvancedSearch/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Album album = db.Albums.Find(id);
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
