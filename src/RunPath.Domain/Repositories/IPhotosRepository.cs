using System.Collections.Generic;
using System.Threading.Tasks;
using RunPath.Domain.Models;

namespace RunPath.Domain.Repositories
{
    public interface IPhotosRepository
    {
        Task<List<Photo>> GetPhotosByAlbumId(int albumId);
    }
}