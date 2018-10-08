using Microsoft.AspNetCore.Mvc;
using Phishtank.Common.Entities.External;
using Phishtank.Common.Exceptions;
using Phishtank.Common.Services.Abstractions;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace URLShortner.Controllers
{
    [ApiVersion("2018-10-01")]
    [Produces("application/json")]
    [Route("")]
    public class HomeController : Controller
    {
        private readonly IUrlService _urlService;

        public HomeController(IUrlService urlService)
        {
            _urlService = urlService ?? throw new ArgumentNullException(nameof(urlService));
        }

        [HttpGet]
        public IActionResult Get()
        {
            var welcome = new
            {
                Message = "Hi, I'm an interview project :)",
                Version = GetAssemblyVersion(),
                Date = DateTimeOffset.UtcNow
            };

            return Ok(welcome);
        }

        [HttpPost("shorten")]
        public async Task<IActionResult> ShortenURLAsync([FromBody] ShortenUrlRequest request)
        {
            if (request == null)
            {
                throw new InvalidShortenUrlRequestException("Please provide a valid url request");
            }

            if (string.IsNullOrEmpty(request.LongUrl) || !Uri.TryCreate(request.LongUrl, UriKind.Absolute, out Uri uri))
            {
                throw new InvalidShortenUrlRequestException("The URL is either missing or not a valid URI");
            }

            var shortUrl = await _urlService.ShortenUrlAsync(request.LongUrl.ToString());
            return Ok(shortUrl);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string GetAssemblyVersion()
        {
            return typeof(HomeController).Assembly.GetName().Version.ToString();
        }
    }
}
