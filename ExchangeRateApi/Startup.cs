using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRate.Extensions;
using ExchangeRate.HttpClients;
using ExchangeRate.Services;
using ExchangeRate.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using RestSharp;

namespace ExchangeRateApi
{
    public class Startup
    {
        /*public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        */
        public IServiceCollection ConfigureIConfigurationProvider(IServiceCollection services, string basePath)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var providers = new List<IConfigurationProvider>();
            foreach (var descriptor in services.Where(descriptor => descriptor.ServiceType == typeof(IConfiguration)).ToList())
            {
                var existingConfiguration = descriptor.ImplementationInstance as IConfigurationRoot;

                if (existingConfiguration is null)
                {
                    continue;
                }

                providers.AddRange(existingConfiguration.Providers);

                services.Remove(descriptor);
            }

            providers.AddRange(config.Providers);
            services.AddSingleton<IConfiguration>(new ConfigurationRoot(providers));
            return services;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ExchangeRate", Version = "v1" });
            });
            services.AddApiVersioning();
            services.AddTransient<IRestClient, RestClient>();
            services.AddTransient<IXEClient, XEClient>();
            services.AddTransient<IXEService, XEService>();
            services.ConfigurePollyPolicies();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ExchangeRate v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
