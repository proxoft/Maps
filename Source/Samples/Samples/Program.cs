using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Proxoft.Maps.Samples;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services
    .ConfigureOpenStreetMapsServices(builder.Configuration)
    .ConfigureMapBoxServices(builder.Configuration)
    ;

await builder.Build().RunAsync();
