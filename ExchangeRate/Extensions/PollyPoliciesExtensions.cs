using Microsoft.Extensions.DependencyInjection;
using Polly.Registry;
using ExchangeRate.PollyPolicies;

namespace ExchangeRate.Extensions
{
    public static class PollyPolicyExtensions
    {
        public static void ConfigurePollyPolicies(this IServiceCollection services)
        {
            var asyncPolicyFactory = new AsyncPolicyFactory();

            PolicyRegistry registry = new PolicyRegistry()
            {
                { "HttpRetryStrategy", asyncPolicyFactory.GetHttpTransientErrorRetryPolicies(3) }
            };

            services.AddSingleton<IReadOnlyPolicyRegistry<string>>(registry);
        }
    }
}
