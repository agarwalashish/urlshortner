using System;
using System.IO;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Phishtank.Common.Persistence;
using Phishtank.Common.Persistence.Abstractions;
using Phishtank.Common.Services;
using Phishtank.Common.Services.Abstractions;
using URLShortner.Middleware;

namespace Phish.Hosting.URLShortner
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddMemoryCache();

            services.Configure<IpRateLimitPolicies>(Configuration.GetSection("IpRateLimiting"));
            services.Configure<IpRateLimitPolicies>(Configuration.GetSection("IpRateLimitPolicies"));

            // Add versionings
            services.AddApiVersioning(o =>
            {
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(new DateTime(2018, 10, 01));
                o.ApiVersionReader = new HeaderApiVersionReader("x-csco-api");
            });

            var loggerFactory = new LoggerFactory().AddConsole();
            
            services.AddSingleton(Configuration);
            services.AddSingleton(loggerFactory);
            services.AddSingleton<IUrlService, UrlService>();

            IUrlRepository urlRepository = new UrlRepository(Configuration, loggerFactory);
            urlRepository.InitializeAsync().GetAwaiter().GetResult();

            IPhishSubmissionsRepository phishRepository = new PhishSubmissionsRepository(Configuration, loggerFactory);
            phishRepository.InitializeAsync().GetAwaiter().GetResult();

            services.AddSingleton(phishRepository);
            services.AddSingleton(urlRepository);

            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), string.Empty)),
                RequestPath = ""
            });

            app.UseMiddleware<ExceptionMiddleware>();
            app.UseMvc();
        }
    }
}
