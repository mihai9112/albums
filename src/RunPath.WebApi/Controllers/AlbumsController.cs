using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RunPath.Domain.Extensions;
using RunPath.Domain.Repositories;
using RunPath.WebApi.Models;
using RunPath.WebApi.Models.Hypermedia;
using Serilog;

namespace RunPath.WebApi.Controllers
{
    [Route("[controller]")]
    public class AlbumsController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IAlbumsRepository _albumsRepository;
        private readonly IPhotosRepository _photosRepository;

        public AlbumsController(ILogger logger, IAlbumsRepository albumsRepository, IPhotosRepository photosRepository)
        {
            _logger = (logger?.ForContext<AlbumsController>()).ThrowIfNull(nameof(logger));
            _albumsRepository = albumsRepository.ThrowIfNull(nameof(albumsRepository));
            _photosRepository = photosRepository.ThrowIfNull(nameof(photosRepository));
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var albums = await _albumsRepository.GetAlbums();
            
            if(!albums.Any())
                return NotFound();

            return Ok(
                albums.Select(
                    x => new AlbumResponse(
                        HypermediaLinkBuilder.ForAlbumDiscovery(Url, x.Id, x.UserId), 
                        new Dictionary<string,string>{
                            { "id", x.Id.ToString() },
                            { "userId" , x.UserId.ToString() },
                            { "title", x.Title }
                        })));
        }

        [HttpGet("{albumId}/photos")]
        public async Task<IActionResult> GetPhotos(string albumId)
        {
            if(!int.TryParse(albumId, out var validAlbumId))
                return UnprocessableEntity();

            var photos = await _photosRepository.GetPhotosByAlbumId(validAlbumId);

            if(!photos.Any())
                return NotFound();
            
            return Ok(
                new PhotosResponse(
                    HypermediaLinkBuilder.ForPhotosDiscovery(Url, validAlbumId), 
                    new Dictionary<string, string> { { "albumId", validAlbumId.ToString() }},
                    photos
                )
            );
        }

        [HttpGet("{albumId}")]
        public async Task<IActionResult> GetAlbumDetails(string albumId)
        {
            if(!int.TryParse(albumId, out var validAlbumId))
                return UnprocessableEntity();
            
            var albumDetailsOption = await _albumsRepository.GetAlbumDetails(validAlbumId);
            if (!albumDetailsOption.TryUnwrap(out var albumDetails))
            {
                _logger.Warning($"Cannot find album details with album id: {albumId}.");
                return NotFound();
            }

            var links = HypermediaLinkBuilder.ForAlbumDetailsDiscovery(Url, validAlbumId, albumDetails.UserId);

            return Ok(
                new AlbumResponse(
                    links,
                    new Dictionary<string, string> {
                        { "id", albumDetails.Id.ToString() },
                        { "userId", albumDetails.UserId.ToString() },
                        { "title", albumDetails.Title }
                    }
                )
            );
        }
    }
}