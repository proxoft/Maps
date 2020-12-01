using Proxoft.Maps.MapBox.Hosting.Builders;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IMapBoxOptionsBuilder AddMapBox(this IServiceCollection services, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
            => new MapBoxBuilder(services, serviceLifetime);
    }
}
