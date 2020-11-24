using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Sample.Maps
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            // builder.Services.ConfigureGoogleMapsServices(builder.Configuration);
            builder.Services.ConfigureOpenStreetMapsServices(builder.Configuration);
            await builder.Build().RunAsync();
        }

        private static void ConfigureGoogleMapsServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddGoogleMaps(ServiceLifetime.Scoped)
                .Configure(configuration)
                .Build();
        }

        private static void ConfigureOpenStreetMapsServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOpenStreetMaps(ServiceLifetime.Scoped)
                .Configure(configuration)
                .AddGeocoder()
                .Register();
        }
    }
}
