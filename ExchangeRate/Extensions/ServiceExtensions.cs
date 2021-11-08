using System;
using ExchangeRate.HttpClients;
using ExchangeRate.Services;
using ExchangeRate.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using Microsoft.OpenApi.Models;

namespace ExchangeRate.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            
            return services;
        }

        public static IServiceCollection ConfigureIConfigurationProvider(this IServiceCollection services, string basePath)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT")}.json", optional: true, reloadOnChange: true)
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
    }
}
