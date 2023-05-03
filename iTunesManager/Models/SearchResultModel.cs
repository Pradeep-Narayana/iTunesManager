using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iTunesManager.Models
{
    public class SearchResultModel
    {
        public string ArtistName { get; set; }
        public string TrackName { get; set; }
        public string TrackViewUrl { get; set; }
        public List<SearchResultModel> Results { get; set; }
    }
}