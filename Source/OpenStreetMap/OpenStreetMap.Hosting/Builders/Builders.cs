using Microsoft.Extensions.DependencyInjection;
using Proxoft.Maps.Core.Api;
using Proxoft.Maps.OpenStreetMap.Maps;

namespace Proxoft.Maps.OpenStreetMap.Hosting.Builders
{
    internal class OpenStreetMapBuilder : IOpenStreetMapApiBuilder
    {
        private readonly IServiceCollection _services;
        private readonly ServiceLifetime _serviceLifetime;

        public OpenStreetMapBuilder(IServiceCollection services, ServiceLifetime serviceLifetime)
        {
            _services = services;
            _serviceLifetime = serviceLifetime;
        }

        void IOpenStreetMapApiBuilder.Register()
        {
            _services.Add(new ServiceDescriptor(typeof(IMapFactory), typeof(MapFactory), _serviceLifetime));
        }
    }
}
