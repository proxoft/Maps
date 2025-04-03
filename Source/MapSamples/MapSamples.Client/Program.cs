using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Proxoft.Maps.MapSamples.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services
    .ConfigureOpenStreetMapsServices(builder.Configuration)
    .ConfigureMapBoxServices(builder.Configuration)
    ;

await builder
    .Build().RunAsync();
