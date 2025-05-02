using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using WebAPi_Scrapping_Youtube.Models;

namespace WebAPi_Scrapping_Youtube.Services
{
    public class YoutubeService
    {
        private readonly HttpClient _httpClient;
        private readonly string _channelUrl = "https://www.youtube.com/channel/UCvtT19MZW8dq5Wwfu6B0oxw";


        public YoutubeService()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
        }


        public async Task<List<Video>> GetVideos(string? filterBy = null)
        {
            string url;
            if (filterBy == null)
            {
                url = $"{_channelUrl}/videos";
            }
            else
            {
                url = $"{_channelUrl}/videos/{filterBy}";
            }

            //Peticion 
            var html = await _httpClient.GetStringAsync(url);

            var jsonMatch = Regex.Match(html, @"var ytInitialData\s*=\s*({.*?});</script>", RegexOptions.Singleline);

            if (!jsonMatch.Success)
            {
                Console.WriteLine("No se pudieron obtener los videos");
                return [];
            }
            var json = jsonMatch.Groups[1].Value;


            var jsonObject = JObject.Parse(json);

            //extraer el listado de videos
            var videos = jsonObject.SelectTokens("$..richItemRenderer.content.videoRenderer").Where(v => v != null);

            var result = new List<Video>();


            //tomar los videos y pasarlos a un modelo
            foreach (JToken video in videos)
            {
                try
                {
                    var newVideo = new Video
                    {
                        Id = video["videoId"].ToString(),
                        Title = video["title"]["runs"]![0]!["text"]!.ToString(),
                        ViewCount = video["shortViewCountText"]?["simpleText"]?.ToString(),
                        Duration = video["thumbnailOverlays"]?[0]?["thumbnailOverlayTimeStatusRenderer"]?["text"]?["simpleText"]?.ToString(),
                        Url = $"https://www.youtube.com/watch?v={video["videoId"]}",
                        ThumbnailUrl = video["thumbnail"]!["thumbnails"]!.Last!["url"]!.ToString(),
                        PublishedDate = video["publishedTimeText"]?["simpleText"]?.ToString(),
                    };
                    result.Add(newVideo);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al procesar los videos: {ex.Message}");
                    throw;
                }
            }

            return result;
        }

        public async Task<string> GetVideoExample(string? value = null)
        {
            return value;
        }
    }
}
