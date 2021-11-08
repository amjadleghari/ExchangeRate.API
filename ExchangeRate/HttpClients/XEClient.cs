
using System.Collections.Generic;
using System.Threading.Tasks;
using ExchangeRate.Entities;
using ExchangeRate.Results;
using ExchangeRate.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


using Polly;
using Polly.Registry;

using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serialization.Json;

namespace ExchangeRate.HttpClients
{
    public class XEClient: IXEClient
    {
        private readonly ILogger<XEClient> _logger;
        private readonly XERateConnectorFields _XERateConnectorFields;
        private readonly IAsyncPolicy<IRestResponse> _retryPolicy;

        private readonly IDictionary<string, string> _envMap = new Dictionary<string, string>()
        {
            { "Development", "dev" },
            { "Staging", "stg" },
            { "UAT", "uat" },
            { "Production", "prod" }
        };

        public XEClient(ILogger<XEClient> logger, IConfiguration config, IWebHostEnvironment env, IReadOnlyPolicyRegistry<string> policyRegistry)
        {
            _logger = logger;
            var _config = config;
            _retryPolicy = policyRegistry.Get<IAsyncPolicy<IRestResponse>>("HttpRetryStrategy");
            _XERateConnectorFields = config.GetSection("XERateConnector").Get<XERateConnectorFields>();
               
        }

        public async Task<XERatesResponse> GetExchangeRatesAsync(XERatesRequest currencyRequest)
        {
            var client = new RestClient($"{_XERateConnectorFields.Host}convert_from.json")
            {
                Timeout = -1
            };

            client.Authenticator = new HttpBasicAuthenticator(_XERateConnectorFields.AccountId, _XERateConnectorFields.ApiKey);
            client.UseSerializer(
                () => new JsonSerializer { DateFormat = "yyyy-MM-ddTHH:mm:ss.FFFFFFFZ" }
            );

            var request = new RestRequest(Method.GET);

            if(!string.IsNullOrEmpty(currencyRequest.baseCurrency))
                request.AddParameter("from", currencyRequest.baseCurrency);

            request.AddParameter("to", currencyRequest.toCurrency);

            if(currencyRequest.Amount > 0)
                request.AddParameter("amount", currencyRequest.Amount);

            if (currencyRequest.margin > 0)
                request.AddParameter("margin", currencyRequest.margin);

            var retVal = await client.ExecuteAsync<ApiResult<XERatesResponse>>(request);
            return retVal.Data.Data;
        }

        public async Task<List<Currency>> GetCurrenciesAsync()
        {
            var client = new RestClient($"{_XERateConnectorFields.Host}currencies")
            {
                Timeout = -1
            };

            client.Authenticator = new HttpBasicAuthenticator(_XERateConnectorFields.AccountId, _XERateConnectorFields.ApiKey);
            client.UseSerializer(
                () => new JsonSerializer { DateFormat = "yyyy-MM-ddTHH:mm:ss.FFFFFFFZ" }
            );
            var request = new RestRequest(Method.GET);
            
            var retVal = await client.ExecuteAsync<ApiResult<XECurrenciesResponse>>(request);
            return retVal.Data.Data.currencies;
        }
    }
}
