using System.Linq;
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
    public class UserAlbumTests
    {
        private TestServer _testServer;

        [SetUp]
        public void SetUp()
        {
            var httpClient = new HttpClientBuilder()
                .WithGetUserAlbumSuccessfulResponse()
                .WithGetAlbumPhotosSuccessfulResponse()
                .Build();
            var builder = TestWebHostBuilder.BuildTestWebHostForStartUp<Startup>(httpClient);
            _testServer = new TestServer(builder);
        }

        [Test]
        public async Task Getting_Existing_User_With_Album_And_Photos_Must_Return_200_Ok()
        {
            // Given existing user
            var existingUserId = "1";
            var httpClient = ApiHelper.CreateHttpClient(_testServer);

            //When I want to get user's albums with photos
            //And the user id is valid
            //And there is one album linked to the user
            //And one photo in the album
            var response = await httpClient.GetAsync($"users/{existingUserId}/albums");
            var responseContent = await response.Content.ReadAsStringAsync();

            //Then the response is present
            Assert.That(response, Is.Not.Null);
            Assert.That(responseContent, Is.Not.Null);

            //And the callee receives 200 status code.
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(responseContent, Is.Not.Empty);

            var responseObj = SerializerHelper.DeserializeFrom<UserApiResponse>(responseContent);

            Assert.That(responseObj.ResponseData.Count, Is.EqualTo(1));
            Assert.That(responseObj.Links, Is.Not.Null);

            Assert.That(responseObj.Albums.Count, Is.EqualTo(1));
            Assert.That(responseObj.Albums.ElementAt(0).Photos.Count, Is.EqualTo(1));
        }

        [Test]
        public Task Getting_Existing_User_With_With_Album_And_No_Photos_Must_Return_200_Ok()
        {
            //Given I have an existing user
            //When I want to get user's albums with no photos
            //And the user id is valid
            //And there is one album linked to the user
            //And no photos in the album
            //Then the response is present
            return Task.CompletedTask;
        }

        [Test]
        public Task Getting_No_User_Must_Return_404_NotFound()
        {
            return Task.CompletedTask;
        }
    }
}