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
    public class UsersController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IAlbumsRepository _albumsRepository;

        public UsersController(ILogger logger, IAlbumsRepository albumsRepository)
        {
            _logger = (logger?.ForContext<UsersController>()).ThrowIfNull(nameof(logger));
            _albumsRepository = albumsRepository.ThrowIfNull(nameof(albumsRepository));
        }

        [HttpGet("{userId}/albums")]
        public async Task<IActionResult> GetUserAlbums(string userId)
        {
            if(!int.TryParse(userId, out var validUserId))
                return UnprocessableEntity();
            
            var albumsForUser = await _albumsRepository.GetAlbumByUserId(validUserId);
        
            if(!albumsForUser.Any())
                return NotFound();

            return Ok(
                new UserResponse(
                    HypermediaLinkBuilder.ForUsersDiscovery(Url, validUserId),
                    new Dictionary<string,string>() { {"userId", userId }},
                    albumsForUser
                )
            );
        }
    }
}