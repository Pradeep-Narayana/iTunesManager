using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using iTunesManager.Models;
using System.Net.Http;

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

        public ActionResult Search(string term)
        {
            //ViewBag.Message = "Your Search page.";

            //return View();
            //try
            //{
            //    //var result = RedirectToAction("Search", "SearchController", new { term = Term});

            //ViewBag.value = Term;
            //return View(result);
            try
            {
                var url = "https://itunes.apple.com/search?term=" + term;
                var client = new HttpClient();
                var response = client.GetAsync(url).Result;
                response.EnsureSuccessStatusCode();

                var jsonString = response.Content.ReadAsStringAsync().Result;
                var searchResults = JsonConvert.DeserializeObject<SearchResultModel>(jsonString);

                ViewBag.value = term;
                return View(searchResults.Results.ToList());
            }
            catch (Exception ex)
                {
                    throw ex;
                }
            }
    }
}