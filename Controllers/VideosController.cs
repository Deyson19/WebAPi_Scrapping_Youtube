using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPi_Scrapping_Youtube.Services;

namespace WebAPi_Scrapping_Youtube.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideosController(YoutubeService youtubeService) : ControllerBase
    {
        private readonly YoutubeService _youtubeService = youtubeService;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _youtubeService.GetVideos();

            return Ok(result);
        }

        [HttpGet("PopularVideos")]
        public async Task<IActionResult> GetPopular()
        {
            var result = await _youtubeService.GetVideos("?view=0&sort=p");

            return Ok(result);
        }
    }
}
