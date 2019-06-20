using System.Collections.Generic;
using Newtonsoft.Json;
using RunPath.WebApi.Models.Hypermedia;

namespace RunPath.WebApi.Models
{
    public class AlbumResponse
    {
        public IDictionary<string, string> ResponseData { get; }

        [JsonProperty("_links")]
        public AlbumDiscovery Links { get; }
        
        public AlbumResponse(AlbumDiscovery albumDiscovery, IDictionary<string, string> responseData)
        {
            ResponseData = responseData;
            Links = albumDiscovery;
        }
    }
}