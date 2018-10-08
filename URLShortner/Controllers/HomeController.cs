using Microsoft.AspNetCore.Mvc;
using Phishtank.Common.Services.Abstractions;
using System;
using System.Threading.Tasks;

namespace URLShortner.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        private readonly IUrlService _urlService;

        public HomeController(IUrlService urlService)
        {
            _urlService = urlService ?? throw new ArgumentNullException(nameof(urlService));
        }

        [HttpGet("shorten")]
        public async Task<IActionResult> ShortenURLAsync([FromQuery] string url)
        {
            var shortUrl = await _urlService.ShortenUrlAsync(url);
            return Ok(shortUrl);
        }
    }
}
