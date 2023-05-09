using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace iTunesManager.Models
{
    public class ClickCountModel
    {
        [Key]
        public int Id { get; set; }

        public string ArtistName { get; set; }

        public string TrackName { get; set; }

        public int ClickCount { get; set; }
    }
}
