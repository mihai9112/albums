using Newtonsoft.Json;

namespace RunPath.WebApi.Models.Hypermedia
{
    public class PhotosDiscovery
    {
        public Link Self { get; set; }

        [JsonProperty("album-details-get")]
        public Link AlbumDetailsGet { get; set; }

        public PhotosDiscovery(
            Link self,
            Link albumDetailsGet
        )
        {
            Self = self;
            AlbumDetailsGet = albumDetailsGet;
        }
    }
}