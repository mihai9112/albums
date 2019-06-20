using System.Net;
using System.Net.Http;
using System.Text;
using RichardSzalay.MockHttp;
using RunPath.Tests.Integration.Fixtures;

namespace RunPath.Tests.Integration.Builders
{
    public class HttpClientBuilder
    {
        private readonly MockHttpMessageHandler _mockedHandler;

        public HttpClientBuilder()
        {
            _mockedHandler = new MockHttpMessageHandler();
        }

        public HttpClient Build()
        {
            return _mockedHandler.ToHttpClient();
        }

        public HttpClientBuilder WithGetAlbumDetailsSuccessfulResponse()
        {
            var albumDetailsResponse = "{\"userId\": 1,\"id\": 1,\"title\": \"quidem molestiae enim\"}";
            _mockedHandler
                .When($"{Configuration._thirdPartyApi}/albums/1")
                .Respond(req => new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(albumDetailsResponse, Encoding.UTF8, "application/json")
                });
            return this;
        }

        public HttpClientBuilder WithGetUserAlbumSuccessfulResponse()
        {
            var userAlbumResponse = "[{\"userId\":1,\"id\":1,\"title\":\"quidemmolestiaeenim\"}]";
            _mockedHandler
                .When($"{Configuration._thirdPartyApi}/albums?userId=1")
                .Respond(req => new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(userAlbumResponse, Encoding.UTF8, "application/json")
                });
            return this;
        }

        public HttpClientBuilder WithGetAlbumPhotosSuccessfulResponse()
        {
            var albumPhotosResponse = "[{\"albumId\":1,\"id\":1,\"title\":\"accusamusbeataeadfaciliscumsimiliquequisunt\",\"url\":\"https:test-placeholder\",\"thumbnailUrl\":\"https:test-placeholder\"}]";
            _mockedHandler
                .When($"{Configuration._thirdPartyApi}/photos?albumId=1")
                .Respond(req => new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(albumPhotosResponse, Encoding.UTF8, "application/json")
                });
            return this;
        }
    }
}