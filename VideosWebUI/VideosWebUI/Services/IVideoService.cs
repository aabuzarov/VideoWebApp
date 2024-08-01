using VideosWebUI.Models;

namespace VideosWebUI.Services
{
    public interface IVideoService
    {
        Task<Video> GetVideoByIdAsync(int id);
        Task<IEnumerable<Video>> GetVideosAsync();
        Task UploadVideoAsync(IFormFile file);
    }
}