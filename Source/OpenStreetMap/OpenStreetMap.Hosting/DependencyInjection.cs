using Proxoft.Maps.OpenStreetMap.Hosting.Builders;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IOpenStreetMapApiBuilder AddOpenStreetMaps(this IServiceCollection services, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            return new OpenStreetMapBuilder(services, serviceLifetime);
        }
    }
}
