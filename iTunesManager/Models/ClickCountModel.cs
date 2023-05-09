using System.ComponentModel.DataAnnotations;

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
