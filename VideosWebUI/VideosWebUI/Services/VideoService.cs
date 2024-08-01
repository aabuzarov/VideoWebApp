using VideosWebUI.Models;

namespace VideosWebUI.Services
{
    public class VideoService : IVideoService
    {
        private readonly HttpClient _httpClient;
        private string _baseApiUrl; 

        public VideoService(HttpClient httpClient, string baseApiUrl)
        {
            _httpClient = httpClient;
            _baseApiUrl = baseApiUrl;
        }

        public async Task<IEnumerable<Video>> GetVideosAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Video>>(_baseApiUrl + "api/Video");
        }

        public async Task<Video> GetVideoByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Video>($"{_baseApiUrl}api/Video/{id}");
        }

        public async Task UploadVideoAsync(IFormFile file)
        {
            using (var content = new MultipartFormDataContent())
            {
                using (var stream = file.OpenReadStream())
                {
                    content.Add(new StreamContent(stream), "file", file.FileName);
                    var response = await _httpClient.PostAsync(_baseApiUrl + "api/Video/", content);
                    response.EnsureSuccessStatusCode();
                }
            }
        }
    }
}