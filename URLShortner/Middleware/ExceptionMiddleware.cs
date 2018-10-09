using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Phishtank.Common.Exceptions;
using System;
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
        public ExceptionMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory != null ? 
                      loggerFactory.CreateLogger(typeof(ExceptionMiddleware).GetType().Name) : 
                      throw new ArgumentNullException(nameof(loggerFactory));
        }

        /// <summary>
        /// Invokes logic of middleware
        /// </summary>
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
                _logger.LogError("API error", ex.Message, ex.Code);

                var result = JsonConvert.SerializeObject(ex);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)apiEx.StatusCode;
                await context.Response.WriteAsync(result);
            }
            catch (Exception ex)
            {
                // Log exception
                _logger.LogError("Unknown API error", ex);

                var result = JsonConvert.SerializeObject(ex);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsync(result);
            }
        }
    }
}

