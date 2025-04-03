namespace Proxoft.Maps.MapSamples.Client;

public static class MapsConfigurations
{
    public static IServiceCollection ConfigureOpenStreetMapsServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .ConfigureOpenStreetMapsServices(ServiceLifetime.Scoped)
            .Configure(configuration)
            .AddMaps()
            .AddGeocoder()
            .Register();

        return services;
    }

    public static IServiceCollection ConfigureMapBoxServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddMapBox(ServiceLifetime.Scoped)
            .Configure(configuration)
            .AddStaticMapService()
            .Register();

        return services;
    }
}
