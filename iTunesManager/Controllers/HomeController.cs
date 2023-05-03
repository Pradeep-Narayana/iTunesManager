using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iTunesManager.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Search(string Term)
        {
            //ViewBag.Message = "Your Search page.";

            //return View();
            try
            {
                var result = RedirectToAction("Search", "SearchController", new { term = Term});

                ViewBag.value = Term;
                return View(result);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}