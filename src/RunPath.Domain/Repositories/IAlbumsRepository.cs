using System.Collections.Generic;
using System.Threading.Tasks;
using Optional;
using RunPath.Domain.Models;

namespace RunPath.Domain.Repositories
{
    public interface IAlbumsRepository
    {  
        Task<List<Album>> GetAlbumByUserId(int userId);
        Task<List<Album>> GetAlbums();
        Task<Option<Album>> GetAlbumDetails(int albumId);
    }
}