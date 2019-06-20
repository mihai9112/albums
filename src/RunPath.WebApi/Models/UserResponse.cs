using System.Collections.Generic;
using Newtonsoft.Json;
using RunPath.Domain.Models;
using RunPath.WebApi.Models.Hypermedia;

namespace RunPath.WebApi.Models
{
    public class UserResponse
    {
        public IDictionary<string, string> ResponseData { get; }
        public List<Album> Albums { get; }

        [JsonProperty("_links")]
        public UserDiscovery Links { get; }

        public UserResponse(
            UserDiscovery userDiscovery,
            IDictionary<string, string> responseData,
            List<Album> albums
        )
        {
            ResponseData = responseData;
            Links = userDiscovery;
            Albums = albums;
        }
    }
}