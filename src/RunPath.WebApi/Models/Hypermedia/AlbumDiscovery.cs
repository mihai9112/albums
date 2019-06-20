using Newtonsoft.Json;

namespace RunPath.WebApi.Models.Hypermedia
{
    public class AlbumDiscovery
    {
        public Link Self { get; set; }

        [JsonProperty("photos-get")]
        public Link PhotosGet { get; set; }

        [JsonProperty("user-album-get")]
        public Link UserAlbumGet { get; set; }

        public AlbumDiscovery() {}

        public AlbumDiscovery
        (
            Link self,
            Link photosGet,
            Link userAlbumGet
        ) 
        {
            Self = self;
            PhotosGet = photosGet;
            UserAlbumGet = userAlbumGet;
        }
    }
}