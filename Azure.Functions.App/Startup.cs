using Azure.Functions.Contracts;
using Azure.Functions.Contracts.Models;
using Azure.Functions.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Azure.Functions.App.Startup))]
namespace Azure.Functions.App
{
    public class Startup: FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddScoped<IScanImage, ScanImage>();
            builder.Services.AddOptions<ComputervisionConfig>()
                .Configure<IConfiguration>((settings, configuration) =>
                {
                    configuration.GetSection("ComputerVision").Bind(settings);
                });
        }
    }
}
