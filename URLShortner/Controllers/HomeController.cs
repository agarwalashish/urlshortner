using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace URLShortner.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        [HttpGet("shorten")]
        public async Task<IActionResult> ShortenURLAsync([FromQuery] string url)
        {
            return Ok(url);
        }
    }
}
