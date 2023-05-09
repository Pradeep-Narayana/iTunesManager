namespace iTunesManager.Models
{
    public class SearchResultModel
    {
        public int TrackId { get; set; }
        public string ArtistName { get; set; }
        public string TrackName { get; set; }
        public string TrackViewUrl { get; set; }
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
        public int ClickCount { get; set; }
    }
}