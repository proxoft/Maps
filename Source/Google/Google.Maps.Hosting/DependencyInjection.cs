using Google.Maps.Hosting.Builders;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class Dependencies
    {
        public static IGoogleApiConfigurationBuilder AddGoogleMaps(
            this IServiceCollection services,
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            return new MapsBuilder(services, serviceLifetime);
        }
    }
}
