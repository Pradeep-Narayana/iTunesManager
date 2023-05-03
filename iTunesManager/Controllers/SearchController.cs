using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using iTunesManager.Models;
using Newtonsoft.Json;

namespace iTunesManager.Controllers
{
    public class SearchController : Controller
    {
        public async Task<ActionResult> Search(string term)
        {
            var url = "https://itunes.apple.com/search?term=" + term;

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<SearchResultModel>(json);

                return View(data.Results);
            }
        }
    }

}