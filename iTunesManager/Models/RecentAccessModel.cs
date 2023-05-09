using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iTunesManager.Models
{
    public class RecentAccessModel
    {
        public int Id { get; set; }
        public string ArtistName { get; set; }
        public DateTime AccessTime { get; set; }
    }
}