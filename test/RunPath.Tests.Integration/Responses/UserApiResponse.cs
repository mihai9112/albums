using System.Collections.Generic;
using Newtonsoft.Json;

namespace RunPath.Tests.Integration.Responses
{
    public class UserApiResponse
    {
        public IDictionary<string, string> ResponseData { get; set; }
        public List<Album> Albums { get; set; }

        [JsonProperty("_links")]
        public Link Links { get; set; }
        

        public class Album
        {
            public int UserId { get; set; }
            public int Id { get; set; }
            public string Title { get; set; }
            public List<Photo> Photos { get; set; }
        }
        public class Photo
        {
            public int AlbumId { get; set; }
            public int Id { get; set; }
            public string Title { get; set; }
            public string Url { get; set; }
            public string ThumbnailUrl { get; set; }
        }

        public class Self
        {
            public string Href { get; set; }
        }

        public class Link
        {
            public Self Self { get; set; }
        }
    }
}