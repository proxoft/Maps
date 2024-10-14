using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Proxoft.Maps.Core.Abstractions.StaticMaps;
using Proxoft.Maps.MapBox.Common;
using Proxoft.Maps.MapBox.StaticMaps;

namespace Proxoft.Maps.MapBox.Hosting.Builders
{
    internal class MapBoxBuilder :
        IMapBoxOptionsBuilder,
        IMapBoxApiBuilder
    {
        private readonly IServiceCollection _services;
        private readonly ServiceLifetime _serviceLifetime;

        private readonly List<ServiceDescriptor> _serviceDescriptors = new();
        private ServiceDescriptor? _optionsDescriptor;

        internal MapBoxBuilder(IServiceCollection services, ServiceLifetime serviceLifetime)
        {
            _services = services;
            _serviceLifetime = serviceLifetime;
        }

        IMapBoxApiBuilder IMapBoxOptionsBuilder.Configure(Func<MapBoxOptions> factory)
        {
            _optionsDescriptor = new ServiceDescriptor(typeof(MapBoxOptions), (sp) => factory(), _serviceLifetime);
            return this;
        }

        IMapBoxApiBuilder IMapBoxOptionsBuilder.Configure(IConfiguration configuration)
            => ((IMapBoxOptionsBuilder)this).Configure(configuration, "MapBoxApi");

        IMapBoxApiBuilder IMapBoxOptionsBuilder.Configure(IConfiguration configuration, string sectionPath)
            => ((IMapBoxOptionsBuilder)this).Configure(configuration.GetSection(sectionPath));

        IMapBoxApiBuilder IMapBoxOptionsBuilder.Configure(IConfigurationSection configurationSection)
        {
            var accessToken = configurationSection["AccessToken"];
            return ((IMapBoxOptionsBuilder)this).Configure(() => new MapBoxOptions { AccessToken = accessToken ?? "" } );
        }

        IMapBoxApiBuilder IMapBoxOptionsBuilder.UseInstance(MapBoxOptions options)
        {
            _optionsDescriptor = new ServiceDescriptor(typeof(MapBoxOptions), options);
            return this;
        }

        IMapBoxApiBuilder IMapBoxApiBuilder.AddStaticMapService()
        {
            _serviceDescriptors.Add(new ServiceDescriptor(typeof(IStaticMapService), typeof(StaticMapService), _serviceLifetime));
            return this;
        }

        void IMapBoxApiBuilder.Register()
        {
            if (_optionsDescriptor is null)
            {
                throw new Exception("no options have been registered");
            }

            _services.Add(_optionsDescriptor);

            foreach (var sd in _serviceDescriptors)
            {
                _services.Add(sd);
            }
        }
    }
}
