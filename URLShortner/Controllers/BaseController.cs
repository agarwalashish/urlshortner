using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace URLShortner.Controllers
{
    public class BaseController : Controller
    {
        public string IpAddress => HttpContext?.Connection?.RemoteIpAddress?.ToString();
    }
}
