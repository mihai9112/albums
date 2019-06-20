using System.Collections.Generic;
using Newtonsoft.Json;
using RunPath.Domain.Models;
using RunPath.WebApi.Models.Hypermedia;

namespace RunPath.WebApi.Models
{
    public class PhotosResponse
    {
        public IDictionary<string, string> ResponseData { get; }
        public List<Photo> Photos { get; }
        
        [JsonProperty("_links")]
        public PhotosDiscovery Links { get; }

        public PhotosResponse(
            PhotosDiscovery photosDiscovery, 
            IDictionary<string, string> responseData,
            List<Photo> photos
        )
        {
            ResponseData = responseData;
            Links = photosDiscovery;
            Photos = photos;
        }
    }
}