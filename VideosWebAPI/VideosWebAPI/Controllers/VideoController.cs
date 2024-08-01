using Microsoft.AspNetCore.Mvc;
using VideosWebAPI.Models;

namespace VideosWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoController : ControllerBase
    {

        private readonly string _mediaFolder = Path.Combine(Directory.GetCurrentDirectory(), "Media");

        public VideoController()
        {
            if (!Directory.Exists(_mediaFolder))
            {
                Directory.CreateDirectory(_mediaFolder);
            }
        }

        [HttpGet]
        public IActionResult GetVideos()
        {
            var videos = Directory.GetFiles(_mediaFolder)
                                  .Select(Path.GetFileName)
                                  .Select(fileName => new Video { FileName = fileName })
                                  .ToList();
            return Ok(videos);
        }

        [HttpGet("{fileName}")]
        public IActionResult GetVideo(string fileName) //original working
        {
            var filePath = Path.Combine(_mediaFolder, fileName);
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return new FileStreamResult(stream, "video/mp4");
        }

        [HttpPost]
        [RequestSizeLimit(209715200)] // 200MB
        public IActionResult UploadVideo([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            if (Path.GetExtension(file.FileName).ToLower() != ".mp4")
            {
                return BadRequest("Only MP4 files are allowed.");
            }

            var filePath = Path.Combine(_mediaFolder, file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return Ok(new { file.FileName });
        }
    }
}