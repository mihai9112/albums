using Serilog;
using System.Net;
using FluentValidation.AspNetCore;
using FluentValidation.Attributes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RunPath.Domain.Configuration;
using RunPath.WebApi.Filters;
using RunPath.WebApi.Middleware;
using ILogger = Serilog.ILogger;
using RunPath.Domain.Repositories;

namespace RunPath.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;

            Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.WithProperty("HostName", GetHostName())
                .Enrich.WithProperty("Environment", environment.EnvironmentName)
                .CreateLogger();
        }

        public ILogger Logger { get; }
        public IConfiguration Configuration { get; }
        private IHostingEnvironment Environment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<JsonPlaceholderOptions>(Configuration.GetSection("ThirdPartyApi"));
            
            services.AddHttpClient<IAlbumsRepository, AlbumsRepository>();
            services.AddHttpClient<IPhotosRepository, PhotosRepository>();
            
            services.AddSingleton(Logger);

            AddServices(services);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseMiddleware<RequestLoggingMiddleware>();
            app.UseMvc();
        }

        public void ConfigureTestingServices(IServiceCollection services)
        {
            services.AddOptions();
            AddServices(services);
        }

        private static string GetHostName()
        {
            var hostName = string.Empty;

            try
            {
                hostName = Dns.GetHostName();
            }
            catch
            {
                // ignored
            }

            return hostName;
        }

        private void AddServices(IServiceCollection services)
        {
            services
                .AddMvcCore(x =>
                {
                    x.Filters.Add(new ValidateRequestModelFilter(new AttributedValidatorFactory()));
                    x.Filters.Add(new ValidateRequestModelStateFilter()); 
                })
                .AddJsonFormatters(x =>
                {
                    x.NullValueHandling = NullValueHandling.Ignore;
                    x.ContractResolver = new DefaultContractResolver()
                    {
                        NamingStrategy = new SnakeCaseNamingStrategy()
                    };
                })
                .AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<Startup>())
                .SetCompatibilityVersion(CompatibilityVersion.Latest);

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddSerilog(Logger);
                loggingBuilder.AddConsole();
                loggingBuilder.AddDebug();
            });
        }
    }
}
