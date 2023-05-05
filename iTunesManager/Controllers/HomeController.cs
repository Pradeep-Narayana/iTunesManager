using iTunesManager.Models;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
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

        public ActionResult Search(string term)
        {
            try
            {
                ViewBag.value = term;
                SearchResultModel searchResults = CacheController.RetrieveFromCache(term);

                if (searchResults == null)
                {
                    // Data not found in cache, retrieve from API
                    var url = "https://itunes.apple.com/search?term=" + term;
                    var client = new HttpClient();
                    var response = client.GetAsync(url).Result;
                    response.EnsureSuccessStatusCode();

                    var jsonString = response.Content.ReadAsStringAsync().Result;
                    searchResults = JsonConvert.DeserializeObject<SearchResultModel>(jsonString);

                    // Insert data to cache
                    CacheController.InsertIntoCache(searchResults, term);
                }
                return View(searchResults.Results.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
