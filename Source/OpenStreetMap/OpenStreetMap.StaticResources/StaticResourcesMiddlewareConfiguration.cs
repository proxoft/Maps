using Microsoft.AspNetCore.Builder;
using Proxoft.Maps.OpenStreetMap.StaticResources.FileProviders;

namespace Proxoft.Maps.OpenStreetMap.StaticResources;

public static class StaticResourcesMiddlewareConfiguration
{
    public static IApplicationBuilder UseOpenStreetMapAssets(
        this IApplicationBuilder app, string requestPath = "/openStreetMap")
    {
        app.UseStaticFiles(new StaticFileOptions()
        {
            RequestPath = requestPath,
            FileProvider = ResourceFileProvider.Initialize()
        });

        return app;
    }
}
