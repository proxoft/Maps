using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Proxoft.Maps.Core.Abstractions.Geocoding;
using Proxoft.Maps.OpenStreetMap.Common;
using Proxoft.Maps.OpenStreetMap.Geocoding;
using Proxoft.Maps.OpenStreetMap.Maps;
using Proxoft.Maps.Core.Api.Factories;

namespace Proxoft.Maps.OpenStreetMap.Hosting.Builders;

internal class OpenStreetMapBuilder(IServiceCollection services, ServiceLifetime serviceLifetime) :
    IOpenStreetMapApiBuilder,
    IOpenStreetMapOptionsBuilder,
    IGeocoderBuilder
{
    private readonly IServiceCollection _services = services;
    private readonly ServiceLifetime _serviceLifetime = serviceLifetime;

    private readonly List<ServiceDescriptor> _serviceDescriptors = new();
    private ServiceDescriptor _optionsDescriptor = new(typeof(OpenStreetMapOptions), new OpenStreetMapOptions());

    IOpenStreetMapApiBuilder IOpenStreetMapOptionsBuilder.UseInstance(OpenStreetMapOptions options)
    {
        _optionsDescriptor = new ServiceDescriptor(typeof(OpenStreetMapOptions), options);
        return this;
    }

    IOpenStreetMapApiBuilder IOpenStreetMapOptionsBuilder.Configure(Func<OpenStreetMapOptions> factory)
    {
        _optionsDescriptor = new ServiceDescriptor(typeof(OpenStreetMapOptions), (sp) => factory(), _serviceLifetime);
        return this;
    }

    IOpenStreetMapApiBuilder IOpenStreetMapOptionsBuilder.Configure(IConfiguration configuration)
        => ((IOpenStreetMapOptionsBuilder)this).Configure(configuration, "OpenStreetMapApi");

    IOpenStreetMapApiBuilder IOpenStreetMapOptionsBuilder.Configure(IConfiguration configuration, string sectionPath)
        => ((IOpenStreetMapOptionsBuilder)this).Configure(configuration.GetSection(sectionPath));

    IOpenStreetMapApiBuilder IOpenStreetMapOptionsBuilder.Configure(IConfigurationSection section)
    {
        string resourcePath = section["ResourcePath"] ?? "/openStreetMap";
        string language = section["Language"] ?? "en";
        _ = bool.TryParse(section["ConsoleLogExceptions"], out var consoleLog);

        return ((IOpenStreetMapOptionsBuilder)this).Configure(
            () => new OpenStreetMapOptions
                {
                    ResourcePath = resourcePath,
                    Language = language,
                    ConsoleLogExceptions = consoleLog
                }
        );
    }

    IOpenStreetMapApiBuilder IOpenStreetMapApiBuilder.AddGeocoder()
        => ((IOpenStreetMapApiBuilder)this).AddGeocoder(b => b.UseParser<OsmDefaultResultParser>());

    IOpenStreetMapApiBuilder IOpenStreetMapApiBuilder.AddGeocoder(Action<IGeocoderBuilder> builder)
    {
        _serviceDescriptors.Add(new ServiceDescriptor(typeof(IGeocoder), typeof(OsmGeocoder), _serviceLifetime));
        builder(this);
        return this;
    }

    IGeocoderBuilder IGeocoderBuilder.UseParser<TParser>()
    {
        _serviceDescriptors.Add(new ServiceDescriptor(typeof(IOsmResultParser), typeof(TParser), _serviceLifetime));
        return this;
    }

    void IOpenStreetMapApiBuilder.Register()
    {
        _services.Add(_optionsDescriptor);
        foreach(var sd in _serviceDescriptors)
        {
            _services.Add(sd);
        }

        _services.Add(new ServiceDescriptor(typeof(IIdFactory), typeof(IdFactory), _serviceLifetime));
        _services.Add(new ServiceDescriptor(typeof(IMapFactory), typeof(MapFactory), _serviceLifetime));
    }
}
