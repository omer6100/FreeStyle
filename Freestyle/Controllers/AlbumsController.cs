using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Freestyle.Models;

namespace Freestyle.Controllers
{
    [RoutePrefix("albums")]
    public class AlbumsController : Controller
    {
        // GET: Albums
        public ActionResult Index()
        {
            return View();
        }

        [Route("{title?}")]
        public ActionResult View(string title)
        {
            if (!String.IsNullOrEmpty(title))
            {
                return View("Album", GetAlbum(title));
            }
            return View();
        }
    }
}