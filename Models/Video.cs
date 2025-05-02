namespace WebAPi_Scrapping_Youtube.Models
{
    public class Video
    {
        public required string Id { get; set; }
        public required string Title { get; set; }
        public required string Url { get; set; }
        public required string ThumbnailUrl { get; set; }
        public string? ViewCount { get; set; }
        public string? LikeCount { get; set; }
        public string? Duration { get; set; }
        public string? PublishedDate { get; set; }
    }
}
