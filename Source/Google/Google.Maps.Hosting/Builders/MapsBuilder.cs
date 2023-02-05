using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Proxoft.Maps.Core.Api;
using Proxoft.Maps.Core.Api.Factories;
using Proxoft.Maps.Google.Common;
using Proxoft.Maps.Google.Maps;

namespace Google.Maps.Hosting.Builders
{
    internal class MapsBuilder : IGoogleApiConfigurationBuilder, IGoogleLoaderBuilder
    {
        private readonly IServiceCollection _services;
        private readonly ServiceLifetime _serviceLifetime;

        private ServiceDescriptor _configurationDescriptor = null!;

        public MapsBuilder(IServiceCollection services, ServiceLifetime serviceLifetime)
        {
            _services = services;
            _serviceLifetime = serviceLifetime;
        }

        IGoogleLoaderBuilder IGoogleApiConfigurationBuilder.Configure(Func<GoogleApiConfiguration> factory)
        {
            _configurationDescriptor = new ServiceDescriptor(typeof(GoogleApiConfiguration), (sp) => factory(), _serviceLifetime);
            return this;
        }

        IGoogleLoaderBuilder IGoogleApiConfigurationBuilder.Configure(IConfiguration configuration)
            => ((IGoogleApiConfigurationBuilder)this).Configure(configuration, "GoogleApi");

        IGoogleLoaderBuilder IGoogleApiConfigurationBuilder.Configure(IConfiguration configuration, string sectionPath)
            => ((IGoogleApiConfigurationBuilder)this).Configure(configuration.GetSection(sectionPath));

        IGoogleLoaderBuilder IGoogleApiConfigurationBuilder.Configure(IConfigurationSection section)
        {
            var apiKey = section["ApiKey"];
            var language = section["Language"];
            var region = section["region"];

            return ((IGoogleApiConfigurationBuilder)this).Configure(() => new GoogleApiConfiguration(apiKey, language, region));
        }

        IGoogleLoaderBuilder IGoogleApiConfigurationBuilder.UseInstance(GoogleApiConfiguration instance)
        {
            _configurationDescriptor = new ServiceDescriptor(typeof(GoogleApiConfiguration), instance);
            return this;
        }

        void IGoogleLoaderBuilder.Build()
        {
            _services.Add(_configurationDescriptor);
            _services.Add(new ServiceDescriptor(typeof(IMapFactory), typeof(MapFactory), _serviceLifetime));
        }
    }
}
