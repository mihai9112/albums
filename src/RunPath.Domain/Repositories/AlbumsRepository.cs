using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Optional;
using RunPath.Domain.Configuration;
using RunPath.Domain.Extensions;
using RunPath.Domain.Models;
using Serilog;

namespace RunPath.Domain.Repositories
{
    public class AlbumsRepository : IAlbumsRepository
    {
        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;
        private readonly JsonPlaceholderOptions _jsonPlaceholderOptions;
        private readonly IPhotosRepository _photosRepository;
        private readonly string _albumsUrl = "albums";

        public AlbumsRepository(ILogger logger, HttpClient httpClient, IOptions<JsonPlaceholderOptions> jsonPlaceholderOptions, IPhotosRepository photosRepository)
        {
            _logger = (logger?.ForContext<AlbumsRepository>()).ThrowIfNull(nameof(logger));
            _httpClient = httpClient.ThrowIfNull(nameof(httpClient));
            _photosRepository = photosRepository.ThrowIfNull(nameof(photosRepository));
            _jsonPlaceholderOptions = jsonPlaceholderOptions?.Value.ThrowIfNull(nameof(jsonPlaceholderOptions));
        }

        public async Task<List<Album>> GetAlbumByUserId(int userId)
        {
            var tasks = new Dictionary<int, Task<List<Photo>>>();

            var userAlbumResponse = await _httpClient.GetAsync($"{_jsonPlaceholderOptions.RootUrl}/{_albumsUrl}?userId={userId}");
            
            if(userAlbumResponse.StatusCode != HttpStatusCode.OK)
            {
                return new List<Album>();
            }

            var _albumsDto = JsonConvert.DeserializeObject<List<AlbumsDto>>(
                await userAlbumResponse.Content.ReadAsStringAsync());
            
            _albumsDto.ForEach(
                a => tasks.Add(a.Id, _photosRepository.GetPhotosByAlbumId(a.Id))
            );

            await Task.WhenAll(tasks.Values);
            var albums = new List<Album>();
            foreach(var task in tasks)
            {
                var selectedAlbumDto = _albumsDto.FirstOrDefault(x => x.Id == task.Key);
                albums.Add(
                    new Album(selectedAlbumDto.UserId, selectedAlbumDto.Id, selectedAlbumDto.Title, task.Value.Result)
                );
            }
            return albums;
        }

        public async Task<List<Album>> GetAlbums()
        {
            var albumResponse = await _httpClient.GetAsync($"{_jsonPlaceholderOptions.RootUrl}/{_albumsUrl}");

            if(albumResponse.StatusCode != HttpStatusCode.OK)
            {
                return new List<Album>();
            }

            var _albumsDto = JsonConvert.DeserializeObject<List<AlbumsDto>>(
                await albumResponse.Content.ReadAsStringAsync());

            return _albumsDto.Select(a => 
                new Album(a.UserId, a.Id, a.Title)).ToList();
        }

        public async Task<Option<Album>> GetAlbumDetails(int albumId)
        {
            var albumDetailsResponse = await _httpClient.GetAsync($"{_jsonPlaceholderOptions.RootUrl}/{_albumsUrl}/{albumId}");

            if(albumDetailsResponse.StatusCode != HttpStatusCode.OK)
            {
                return Option.None<Album>();
            }
            
            var _albumsDto = JsonConvert.DeserializeObject<AlbumsDto>(
                await albumDetailsResponse.Content.ReadAsStringAsync());

            return Option.Some(new Album(_albumsDto.UserId, _albumsDto.Id, _albumsDto.Title));
        }

        private class AlbumsDto
        {
            public int UserId { get; set; }
            public int Id { get; set; }
            public string Title { get; set; }
        }
    }
}