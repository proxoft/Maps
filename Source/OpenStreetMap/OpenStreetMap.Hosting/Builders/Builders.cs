using Microsoft.Extensions.DependencyInjection;
using Proxoft.Maps.Core.Api;
using Proxoft.Maps.OpenStreetMap.Maps;

namespace Proxoft.Maps.OpenStreetMap.Hosting.Builders
{
    internal class OpenStreetMapBuilder :
        IOpenStreetMapApiBuilder,
        IOpenStreetMapGeocodingBuilder
    {
        private readonly IServiceCollection _services;
        private readonly ServiceLifetime _serviceLifetime;

        public OpenStreetMapBuilder(IServiceCollection services, ServiceLifetime serviceLifetime)
        {
            _services = services;
            _serviceLifetime = serviceLifetime;
        }

        public IOpenStreetMapApiBuilder AddGeocoder(string language)
        {
            throw new System.NotImplementedException();
        }

        void IOpenStreetMapApiBuilder.Register()
        {
            _services.Add(new ServiceDescriptor(typeof(IMapFactory), typeof(MapFactory), _serviceLifetime));
        }
    }
}
