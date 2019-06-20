using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RunPath.Domain.Configuration;
using RunPath.Domain.Extensions;
using RunPath.Domain.Models;
using Serilog;

namespace RunPath.Domain.Repositories
{
    public class PhotosRepository : IPhotosRepository
    {
        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;
        private readonly JsonPlaceholderOptions _jsonPlaceholderOptions;
        private readonly string _photosUrl = "photos?albumId";

        
        public PhotosRepository(ILogger logger, HttpClient httpClient, IOptions<JsonPlaceholderOptions> jsonPlaceholderOptions)
        {
            _logger = (logger?.ForContext<AlbumsRepository>()).ThrowIfNull(nameof(logger));
            _httpClient = httpClient.ThrowIfNull(nameof(httpClient));
            _jsonPlaceholderOptions = jsonPlaceholderOptions?.Value.ThrowIfNull(nameof(jsonPlaceholderOptions));
        }

        public async Task<List<Photo>> GetPhotosByAlbumId(int albumId)
        {
            var photosResponse = await _httpClient.GetAsync($"{_jsonPlaceholderOptions.RootUrl}/{_photosUrl}={albumId}");

            if(photosResponse.StatusCode != HttpStatusCode.OK)
            {
                return new List<Photo>();
            }

            var _photosDto = JsonConvert.DeserializeObject<List<PhotoDto>>(
                await photosResponse.Content.ReadAsStringAsync());

            return _photosDto.Select(p => 
                new Photo(p.AlbumId, p.Id, p.Title, p.Url, p.ThumbnailUrl)).ToList();
        }

        public class PhotoDto
        {
            public int AlbumId { get; set; }
            public int Id { get; set; }
            public string Title { get; set; }
            public string Url { get; set; }
            public string ThumbnailUrl { get; set; }
        }
    }
}