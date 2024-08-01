using VideosWebUI.Models;

namespace VideosWebUI.Services
{
    public class VideoService : IVideoService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl = "https://localhost:7179/api/Video"; // move that to settings

        public VideoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Video>> GetVideosAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Video>>(_apiUrl);
        }

        public async Task<Video> GetVideoByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Video>($"{_apiUrl}{id}");
        }

        public async Task UploadVideoAsync(IFormFile file)
        {
            using (var content = new MultipartFormDataContent())
            {
                using (var stream = file.OpenReadStream())
                {
                    content.Add(new StreamContent(stream), "file", file.FileName);
                    var response = await _httpClient.PostAsync(_apiUrl, content);
                    response.EnsureSuccessStatusCode();
                }
            }
        }
    }
}