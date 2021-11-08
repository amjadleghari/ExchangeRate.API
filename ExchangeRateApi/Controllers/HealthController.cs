using System;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace ExchangeRate.API.Controllers
{
    public class HealthHandler
    {
        private readonly HealthCheckService _healthCheckService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public HealthHandler(HealthCheckService healthCheckService, IWebHostEnvironment webHostEnvironment)
        {
            _healthCheckService = healthCheckService;
            _webHostEnvironment = webHostEnvironment;
        }
        
    }
}
