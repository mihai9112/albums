using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using RunPath.Tests.Integration.Builders;
using RunPath.WebApi.Models;
using RunPath.WebApi.Models.Hypermedia;

namespace RunPath.Tests.Integration.Helpers
{
    public static class ApiHelper
    {

        public static HttpClient CreateHttpClient(TestServer testServer)
        {
            var client = testServer.CreateClient();
            client.DefaultRequestHeaders.Add("ContentType", "application/json");

            return client;
        }
    }
}