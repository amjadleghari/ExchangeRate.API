using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ExchangeRate.Entities;
using Version = ExchangeRate.Entities.Version;
using ExchangeRate.Results;

namespace ExchangeRate.API.Controllers
{
    [Route("v{version:apiVersion}/api/version")]
    [ApiController]
    [ApiVersion("1.0")]
    public class VersionController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly string _env;


        public VersionController(ILogger<VersionController> logger, IWebHostEnvironment env)
        {
            _env = env.EnvironmentName;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResult<Version>>> GetVersion()
        {
            Assembly assembly = typeof(VersionController).Assembly;
            AssemblyName assemblyName = assembly.GetName();
            var result = new ApiResult<Version>();
            try
            {
                var version = new Version()
                {
                    Info = assemblyName.Version.ToString(),
                    Environment = _env
                };

                result = new ApiResult<Version>(version);
                return Ok(result);
            }
            catch (Exception ex)
            {
                result.Success = false;
                _logger.LogError($"GetVersion -> exception: {ex.Message}");
                result.AddError(ex.Message, "TechnicalError");
                return StatusCode(500, result);
            }
        }
    }
}
