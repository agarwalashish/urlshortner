using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace URLShortner.Controllers
{
    /// <summary>
    /// Base controller class
    /// </summary>
    public class BaseController : Controller
    {
        /// <summary>
        /// Returns remote IP Address of the HTTP request
        /// </summary>
        public string IpAddress => HttpContext?.Connection?.RemoteIpAddress?.ToString();
    }
}
