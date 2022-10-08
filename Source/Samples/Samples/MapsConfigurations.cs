namespace Proxoft.Maps.Samples;

public static class MapsConfigurations
{
    public static IServiceCollection ConfigureGoogleMapsServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddGoogleMaps(ServiceLifetime.Scoped)
            .Configure(configuration)
            .Build();

        return services;
    }

    public static IServiceCollection ConfigureOpenStreetMapsServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOpenStreetMaps(ServiceLifetime.Scoped)
            .Configure(configuration)
            .AddGeocoder()
            .Register();

        return services;
    }

    public static IServiceCollection ConfigureMapBoxServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMapBox(ServiceLifetime.Scoped)
            .Configure(configuration)
            .AddStaticMapService()
            .Register();

        return services;
    }
}
