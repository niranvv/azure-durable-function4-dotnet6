using Learn.DurableFunction.ActivityFunctions;
using Learn.DurableFunction.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Learn.DurableFunction.Startup))]
namespace Learn.DurableFunction
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<IGitHubAPIService, GitHubAPIService>();
        }
    }
}
