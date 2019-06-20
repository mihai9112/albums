using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Serilog;
using RunPath.Domain.Configuration;
using RunPath.Domain.Repositories;
using System.Net.Http;

namespace RunPath.Tests.Integration.Fixtures
{
    public static class TestWebHostBuilder
    {
        public static IWebHostBuilder BuildTestWebHostForStartUp<TStartUp>(HttpClient httpClient) where TStartUp : class
        {
            var logger = new Mock<ILogger>();

            logger.Setup(x => x.ForContext<object>()).Returns(logger.Object);
            logger.Setup(x => x.ForContext(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<bool>())).Returns(logger.Object);

            var builder = new WebHostBuilder()
                .UseEnvironment("Testing")
                .ConfigureServices(x =>
                {
                    x.AddTransient<IAlbumsRepository, AlbumsRepository>();
                    x.AddTransient<IPhotosRepository, PhotosRepository>();
                    x.AddSingleton(httpClient);
                    x.AddSingleton(logger.Object);
                    x.Configure<JsonPlaceholderOptions>(options => {
                        options.RootUrl = Configuration._thirdPartyApi;
                    });
                })
                .UseStartup<TStartUp>();

            return builder;
        }
    }
}