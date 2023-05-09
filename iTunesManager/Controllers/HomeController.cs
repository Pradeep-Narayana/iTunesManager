using iTunesManager.Data;
using iTunesManager.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace iTunesManager.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            List<RecentAccessModel> recentAccessItems = new List<RecentAccessModel>();
            // Retrieve the top 10 items from the RecentAccessModel table
            using (var dbContext = new iTunesManagerDbContext())
            {
                recentAccessItems = dbContext.RecentAccessModels.Take(10).ToList();
            }               
            // Pass the view model to the view
            return View(recentAccessItems);
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Search(string term)
        {
            List<SearchResultModel> searchResultsList = CacheController.RetrieveFromCache(term);
            if(String.IsNullOrEmpty(term))
                return View(searchResultsList);
            try
            {
                ViewBag.value = term;
                

                if (searchResultsList == null)
                {
                    // Data not found in cache, retrieve from API
                    var url = "https://itunes.apple.com/search?term=" + term;
                    var client = new HttpClient();
                    var response = client.GetAsync(url).Result;
                    response.EnsureSuccessStatusCode();
                    var jsonString = response.Content.ReadAsStringAsync().Result;
                    var jsonObject = JObject.Parse(jsonString);
                    searchResultsList = jsonObject["results"].ToObject<List<SearchResultModel>>();
                    CacheController.InsertIntoCache(searchResultsList, term);
                }
                UpdateRecentAccessed(term);
                return View(searchResultsList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task UpdateRecentAccessed(string term)
        {
            using (var dbContext = new iTunesManagerDbContext())
            {
                // Retrieve the ClickCountModel object with the given artist and track names
                var recentAccessed = await dbContext.RecentAccessModels
                    .Where(c => c.ArtistName == term).FirstOrDefaultAsync();

                // Increment the click count
                if (recentAccessed == null)
                {
                    // No record found, create a new one
                    recentAccessed = new RecentAccessModel
                    {
                        ArtistName = term,
                        AccessTime = DateTime.Now
                    };
                    dbContext.RecentAccessModels.Add(recentAccessed);
                }
                else
                {
                    // Record found, increment the click count
                    recentAccessed.AccessTime = DateTime.Now;
                }
                // Save changes to the database
                await dbContext.SaveChangesAsync();
            }
        }

        public ActionResult ClickCounts()
        {
            // query the ClickCountModels table and retrieve the artists and the number of times they have been clicked
            List<ClickCountModel> clickCounts = new List<ClickCountModel>();
            using (var dbContext = new iTunesManagerDbContext())
            {
                clickCounts = dbContext.ClickCountModels.OrderByDescending(x=>x.ClickCount).ToList();
            }
            return View(clickCounts);            
        }

        [HttpPost]
        public async Task UpdateClickCount(string artistName, string trackName, string url)
        {
            Thread.Sleep(2000);
            using (var dbContext = new iTunesManagerDbContext())
            {
                // Retrieve the ClickCountModel object with the given artist and track names
                var clickCount = await dbContext.ClickCountModels
                    .Where(c => c.ArtistName == artistName && c.TrackName == trackName)
                    .FirstOrDefaultAsync();

                // Increment the click count
                if (clickCount == null)
                {
                    // No record found, create a new one
                    clickCount = new ClickCountModel
                    {
                        ArtistName = artistName,
                        TrackName = trackName,
                        ClickCount = 1
                    };
                    dbContext.ClickCountModels.Add(clickCount);
                }
                else
                {
                    // Record found, increment the click count
                    clickCount.ClickCount++;
                }
                // Save changes to the database
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
