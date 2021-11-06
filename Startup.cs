using Learn.DurableFunction.ActivityFunctions;
using Learn.DurableFunction.Models;
using Learn.DurableFunction.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

[assembly: FunctionsStartup(typeof(Learn.DurableFunction.Startup))]
namespace Learn.DurableFunction
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var serviceProvider = builder.Services.BuildServiceProvider();
            var existingConfig = serviceProvider.GetRequiredService<IConfiguration>();

            var configBuilder = new ConfigurationBuilder()
                                                .AddConfiguration(existingConfig)
                                                .AddEnvironmentVariables();

            var buildConfig = configBuilder.Build();
            var appConfigConenctionString = buildConfig["AppConfigurationConnectionString"];

            configBuilder.AddAzureAppConfiguration(appConfigConenctionString);
            //configBuilder.AddAzureAppConfiguration(options => options.Connect(appConfigConenctionString);
            buildConfig = configBuilder.Build();
            builder.Services.Replace(new ServiceDescriptor(typeof(IConfiguration), buildConfig));

            builder.Services.Configure<GitHubAPIConfig>(buildConfig.GetSection("GitHub"));

            builder.Services.AddSingleton<IGitHubAPIService, GitHubAPIService>();
        }
    }
}
