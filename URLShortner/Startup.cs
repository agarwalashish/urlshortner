using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
            services.AddMvc();

            // Add versionings
            services.AddApiVersioning(o =>
            {
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(new DateTime(2018, 10, 01));
                o.ApiVersionReader = new HeaderApiVersionReader("x-csco-api");
            });

            services.AddSingleton(Configuration);
            services.AddSingleton<IUrlService, UrlService>();

            IUrlRepository urlRepository = new UrlRepository(Configuration);
            urlRepository.InitializeAsync().GetAwaiter().GetResult();

            IPhishSubmissionsRepository phishRepository = new PhishSubmissionsRepository(Configuration);
            phishRepository.InitializeAsync().GetAwaiter().GetResult();

            services.AddSingleton(phishRepository);
            services.AddSingleton(urlRepository);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ExceptionMiddleware>();
            app.UseMvc();
        }
    }
}
