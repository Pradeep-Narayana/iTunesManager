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

        public string WrapperType { get; set; }
        public string ArtworkUrl100 { get; set; }
        public string ArtworkUrl30 { get; set; }
        public string ArtistViewUrl { get; set; }
        public string PreviewUrl { get; set; }
        public string CollectionViewUrl { get; set; }
        public string TrackPrice { get; set; }
        public string ReleaseDate { get; set; }
        public string Country { get; set; }
        public string PrimaryGenreName { get; set; }

    }
}