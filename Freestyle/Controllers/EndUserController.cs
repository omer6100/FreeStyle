﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using Freestyle.Contexts;
using Freestyle.Models;
using Microsoft.Ajax.Utilities;

namespace Freestyle.Controllers
{
    public class EndUserController : Controller
    {
        private MusicContext db = new MusicContext();

        // GET: EndUser
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: EndUser/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EndUser endUser = db.Users.Find(id);
            if (endUser == null)
            {
                return HttpNotFound();
            }
            return View(endUser);
        }


        // GET: EndUser/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: EndUser/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "Id,Email,Password,Username")] EndUser endUser)
        {
            if (ModelState.IsValid)
            {
                var validInput = db.Users.Where(u => u.Username == endUser.Username || u.Email == endUser.Email)
                    .ToList();
                if (validInput.Count > 0)
                {
                    return View(endUser);
                }
                db.Users.Add(endUser);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            return View(endUser);

        }

        public ActionResult SignIn()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignIn([Bind(Include = "Email, Password")] EndUser endUser)
        {
            if (ModelState.IsValid)
            {
                var existingUser =
                    db.Users.Where(u => u.Email == endUser.Email && u.Password == endUser.Password).Select(u=>new{u.Id,u.Username}).SingleOrDefault();

                if (existingUser == null)
                {
                    return View(endUser);
                }

                Session["UserId"] =  existingUser.Id;
                Session["Role"] = existingUser.Id == 1 ? "Admin" : "User";
                Session["Username"] = existingUser.Username;
                Session["Authorized"] = true;
                return RedirectToAction("Index", "Home");
            }

            return View(endUser);
        }


        public ActionResult SignOut()
        {
            Session["UserId"] = null;
            Session["Role"] = "Guest";
            Session["Username"] = null;
            Session["Authorized"] = false;

            return RedirectToAction("Index", "Home");
        }

        // GET: EndUser/Edit/5
        public ActionResult Update(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (Session["Role"] == null || !Session["UserId"].IfNotNull(userId => userId.Equals(id)))
            {
                return RedirectToAction("Index", "Home");
            }


            EndUser endUser = db.Users.Find(id);
            if (endUser == null)
            {
                return HttpNotFound();
            }
            return View(endUser);
        }

        // POST: EndUser/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update([Bind(Include = "Id,Email,Password,Username")] EndUser endUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(endUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(endUser);
        }

        // GET: EndUser/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (Session["Role"] == null || !Session["UserId"].IfNotNull(userId => userId.Equals(id)))
            {
                return RedirectToAction("Index", "Home");
            }
            EndUser endUser = db.Users.Find(id);
            if (endUser == null)
            {
                return HttpNotFound();
            }
            return View(endUser);
        }

        // POST: EndUser/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EndUser endUser = db.Users.Find(id);
            db.Users.Remove(endUser);
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