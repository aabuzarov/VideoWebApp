using Microsoft.AspNetCore.Mvc;
using VideosWebUI.Models;
using VideosWebUI.Services;

namespace VideosWebUI.Controllers
{
    public class VideoController : Controller
    {
        private readonly IVideoService _videoService;
        private readonly IConfiguration _configuration;

        public VideoController(IVideoService VideoService, IConfiguration configuration)
        {
            _videoService = VideoService;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            var Videos = await _videoService.GetVideosAsync();
            return View(Videos);
        }

        public async Task<IActionResult> Details(int id)
        {
            var Video = await _videoService.GetVideoByIdAsync(id);
            if (Video == null)
            {
                return NotFound();
            }
            return View(Video);
        }

        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                await _videoService.UploadVideoAsync(file);
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Play(string fileName)
        {
            TempData["BaseApiUri"] = _configuration.GetValue<string>("BaseApiUri");

            var video = new Video { FileName = fileName };
            return View(video);
        }
    }
}