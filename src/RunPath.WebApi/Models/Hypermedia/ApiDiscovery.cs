using Newtonsoft.Json;

namespace RunPath.WebApi.Models.Hypermedia
{
    public class ApiDiscovery
    {
        public Link Self { get; set; }

        [JsonProperty("albums-get")]
        public Link AlbumsGet { get; set; }

        public ApiDiscovery(
            Link self,
            Link albumsGet
        )
        {
            Self = self;
            AlbumsGet = albumsGet;
        }
    }
}
