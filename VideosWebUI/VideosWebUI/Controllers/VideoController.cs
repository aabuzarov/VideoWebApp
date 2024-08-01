using Microsoft.AspNetCore.Mvc;
using VideosWebUI.Services;

namespace VideosWebUI.Controllers
{
    public class VideoController : Controller
    {
        private readonly IVideoService _VideoService;

        public VideoController(IVideoService VideoService)
        {
            _VideoService = VideoService;
        }

        public async Task<IActionResult> Index()
        {
            var Videos = await _VideoService.GetVideosAsync();
            return View(Videos);
        }

        public async Task<IActionResult> Details(int id)
        {
            var Video = await _VideoService.GetVideoByIdAsync(id);
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
                await _VideoService.UploadVideoAsync(file);
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}