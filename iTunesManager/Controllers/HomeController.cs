using iTunesManager.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data.SQLite;
using Microsoft.Data.Sqlite;
using iTunesManager.Data;
using System.Threading.Tasks;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Threading;

namespace iTunesManager.Controllers
{
    public class HomeController : Controller
    {
        private SqlConnection con;
        private iTunesManagerDbContext dbContext;
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
                List<SearchResultModel> searchResultsList = CacheController.RetrieveFromCache(term);

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


        // Using cache for click count

        //public ActionResult ViewClickCounts()
        //{
        //    var artistCounts = CacheController.GetArtistClickCounts();

        //    // Get the top 10 most accessed artists
        //    var topArtists = artistCounts.OrderByDescending(x => x.Value)
        //                                 .Take(10)
        //                                 .ToList();

        //    // Create a list of models to display in the view
        //    var modelList = new List<ArtistCountModel>();
        //    foreach (var artist in topArtists)
        //    {
        //        modelList.Add(new ArtistCountModel { ArtistId = artist.Key, ClickCount = artist.Value });
        //    }

        //    return View(modelList);
        //}


        //[HttpPost]
        //public void UpdateClickCount(int trackId)
        //{
        //    try
        //    {
        //        // Retrieve data from cache
        //        string term = ViewBag.value;
        //        List<SearchResultModel> searchResultsList = CacheController.RetrieveFromCache(term);

        //        // Find the result item with the matching track ID and increment the click count
        //        var result = searchResultsList.FirstOrDefault(r => r.TrackId == trackId);
        //        if (result != null)
        //        {
        //            result.ClickCount++;
        //        }

        //        // Update cache with the new data
        //        CacheController.InsertIntoCache(searchResultsList, term);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }
}
