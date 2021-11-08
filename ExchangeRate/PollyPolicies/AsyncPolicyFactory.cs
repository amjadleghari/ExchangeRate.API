using System;
using System.Collections.Generic;
using Polly;
using RestSharp;
using NLog;

namespace ExchangeRate.PollyPolicies
{
    public class AsyncPolicyFactory
    {
        private readonly Logger logger = LogManager.GetLogger(typeof(AsyncPolicyFactory).FullName);

        public IAsyncPolicy<IRestResponse> GetHttpTransientErrorRetryPolicies(int retryCount) =>
            Policy.HandleResult<IRestResponse>(
                    r => new List<int> { 408, 500, 502, 503, 504 }.Contains((int)r.StatusCode)
                )
                .WaitAndRetryAsync(
                    retryCount,
                    attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                    (exception, timeSpan, retries, context) =>
                    {
                        if (retryCount != retries)
                            return;

                        // only log if the final retry fails
                        var msg = $"Retry {retries} of {context.PolicyKey} due to: {exception}.";
                        logger.Error(msg, exception);
                    })
                .WithPolicyKey(PolicyKeys.HttpTransientErrorAsyncRetryPolicy);
    }
}
