using System.Collections.Generic;
using Newtonsoft.Json;

namespace RunPath.Tests.Integration.Responses
{
    public class AlbumApiResponse
    {
        public IDictionary<string, string> ResponseData { get; set; }

        [JsonProperty("_links")]
        public Link Links { get; set; }
        
        public class Self
        {
            public string href { get; set; }
        }

        public class Link
        {
            public Self self { get; set; }
        }
    }
}