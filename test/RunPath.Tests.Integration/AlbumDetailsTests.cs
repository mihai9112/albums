using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using NUnit.Framework;
using RunPath.Tests.Integration.Builders;
using RunPath.Tests.Integration.Fixtures;
using RunPath.Tests.Integration.Helpers;
using RunPath.Tests.Integration.Responses;
using RunPath.WebApi;

namespace RunPath.Tests.Integration
{
    public class AlbumDetailsTests
    {
        private TestServer _testServer;

        [SetUp]
        public void SetUp()
        {
            var httpClient = new HttpClientBuilder()
                .WithGetAlbumDetailsSuccessfulResponse()
                .Build();
            var builder = TestWebHostBuilder.BuildTestWebHostForStartUp<Startup>(httpClient);
            _testServer = new TestServer(builder);
        }

        [Test]
        public async Task Getting_Existing_Album_Must_Return_200_Ok()
        {
            // Given existing album
            var existingAlbumId = "1";
            var httpClient = ApiHelper.CreateHttpClient(_testServer);

            // When I want to get album details
            //And the album id is valid.
            var response = await httpClient.GetAsync($"albums/{existingAlbumId}");
            var responseContent = await response.Content.ReadAsStringAsync();

            //Then the response is present
            Assert.That(response, Is.Not.Null);
            Assert.That(responseContent, Is.Not.Null);

            //And the callee receives 200 status code.
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(responseContent, Is.Not.Empty);

            var responseObj = SerializerHelper.DeserializeFrom<AlbumApiResponse>(responseContent);

            Assert.That(responseObj.ResponseData.Count, Is.EqualTo(3));
            Assert.That(responseObj.Links.self.href, Is.EqualTo("http://localhost/albums/1"));
        }

        [Test]
        public Task Getting_Non_Existing_Album_Must_Return_404_NotFound()
        {
            return Task.CompletedTask;
        }
    }
}