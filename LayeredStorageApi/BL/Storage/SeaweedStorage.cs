using Project.Common.Interfaces.Data;
using System.Net.Http;
using System.Text.Json;

namespace LayeredStorageApi.BL.Storage
{
    public class SeaweedStorage : IFileStorage
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        public SeaweedStorage(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _baseUrl = configuration.GetValue<string>("FileService:SeaweedUrl");

        }
        public async Task<bool> DeleteAsync(string fileId)
        {
            var detelUrl = _baseUrl + $":8080/{fileId}";
            var response= await _httpClient.DeleteAsync(detelUrl);
            return response.IsSuccessStatusCode;
            
        }

        public async Task<Stream?> DownloadAsync(string fileId)
        {
            var uploadUrl = _baseUrl + $":9333/{fileId}";
            var response = await _httpClient.GetAsync(uploadUrl);
            return response.IsSuccessStatusCode ? await response.Content.ReadAsStreamAsync() : null;
        }

        public async Task<string?> UploadAsync(Stream fileStream, string fileName, string contentType)
        {
            var uploadUrl = _baseUrl + $":9333/submit";
            var content = new MultipartFormDataContent
        {
            { new StreamContent(fileStream), "file", fileName }
        };

            var response = await _httpClient.PostAsync(uploadUrl, content);
            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            var fileId = JsonDocument.Parse(json).RootElement.GetProperty("fid").GetString();
            return fileId;
        }
    }
}
