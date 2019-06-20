using Microsoft.AspNetCore.Mvc;
using RunPath.WebApi.Models;
using RunPath.WebApi.Models.Hypermedia;

namespace RunPath.WebApi.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        [HttpGet("", Name = "Home")]
        public IActionResult Get()
        {
            var model = new HypermediaLinks<ApiDiscovery>(HypermediaLinkBuilder.ForServiceDiscovery(Url));
            return Ok(model);
        }
    }
}