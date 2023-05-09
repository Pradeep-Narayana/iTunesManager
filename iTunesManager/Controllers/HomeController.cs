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

namespace iTunesManager.Controllers
{
    public class HomeController : Controller
    {
        //private readonly SqliteConnection sqliteConnection;
        //private SqliteDbContext dbContext;
        private SqlConnection con;
        private iTunesManagerDbContext dbContext;
        //public HomeController()
        //{

        //    // Create a connection to the database
        //    //string connectionString = "Data Source=iTunesManagerDb.db";
        //    //try
        //    //{
        //    //    using (SQLiteConnection connection = new SQLiteConnection(connectionString))
        //    //    {
        //    //        connection.Open();
        //    //        Console.WriteLine("Connection successful");

        //    //    }
        //    //}
        //    //catch(Exception ex)
        //    //{
        //    //    Console.WriteLine("Connection failed :"+ex.Message);
        //    //}

        //}

        private void InitializeDb()
        {
            dbContext = new iTunesManagerDbContext();

            //var clicks = new List<ClickCountModel>
            //    {
            //        new ClickCountModel { ArtistName = "Artist1", TrackName = "Track1", ClickCount = 5 },
            //        new ClickCountModel { ArtistName = "Artist2", TrackName = "Track2", ClickCount = 3 },
            //        new ClickCountModel { ArtistName = "Artist3", TrackName = "Track3", ClickCount = 8 },
            //        new ClickCountModel { ArtistName = "Artist4", TrackName = "Track4", ClickCount = 10 }
            //    };

            //foreach (var click in clicks)
            //{
            //    dbContext.ClickCountModels.Add(click);
            //}

            //dbContext.SaveChanges();
        }
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

                    //foreach (var result in searchResultsList)
                    //{
                    //    result.ClickCount = 0;
                    //}
                    // Insert into cache
                    CacheController.InsertIntoCache(searchResultsList, term);
                }
                //UpdateClickCount(searchResultsList, term);
                return View(searchResultsList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult ClickCounts()
        {
            //InitializeDb();
            List<ClickCountModel> clickCounts = new List<ClickCountModel>();
            using (var dbContext = new iTunesManagerDbContext())
            {
                clickCounts = dbContext.ClickCountModels.ToList();
            }
            return View(clickCounts);            
        }

        [HttpPost]
        public async Task UpdateClickCount(string artistName, string trackName, string url)
        {
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
