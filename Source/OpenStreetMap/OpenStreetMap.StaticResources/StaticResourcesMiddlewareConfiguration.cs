using Microsoft.AspNetCore.Builder;
using Proxoft.Maps.OpenStreetMap.StaticResources.FileProviders;

namespace Proxoft.Maps.OpenStreetMap.StaticResources;

public static class StaticResourcesMiddlewareConfiguration
{
    public static IApplicationBuilder UseOpenStreetMapAssets(
        this IApplicationBuilder app, string requestPath = "/openStreetMap")
    {

        ResourceFileProvider resourceFileProvider = ResourceFileProvider.Initialize();
        StaticFileOptions options = new()
        {
            RequestPath = requestPath,
            FileProvider = resourceFileProvider,
            ContentTypeProvider = resourceFileProvider
        };

        app.UseStaticFiles(options);

        return app;
    }
}
