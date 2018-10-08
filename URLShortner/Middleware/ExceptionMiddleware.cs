using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Phishtank.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace URLShortner.Middleware
{
    /// <summary>
    /// Middleware to handle any exceptions being thrown by the API
    /// </summary>
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        /// <summary>
        /// Handles custom API Exceptions
        /// </summary>
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
            //_logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ApiException apiEx)
            {
                var ex = new
                {
                    Code = apiEx.StatusCode,
                    Message = apiEx.Message
                };

                // First log the exception 
                //_logger.Error("API error", apiEx);

                var result = JsonConvert.SerializeObject(ex);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)apiEx.StatusCode;
                await context.Response.WriteAsync(result);
            }
            catch (Exception ex)
            {
                //_logger.Error<Exception>("Unknown API error", ex);

                var result = JsonConvert.SerializeObject(ex);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsync(result);
            }
        }

    }
}

