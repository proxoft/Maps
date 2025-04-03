using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Proxoft.Maps.OpenStreetMap.Common;

namespace Proxoft.Maps.OpenStreetMap.Hosting.Builders;

internal class OpenStreetMapBuilder(IServiceCollection services, ServiceLifetime serviceLifetime) :
    IOpenStreetMapApiBuilder,
    IOpenStreetMapOptionsBuilder
{
    private readonly IServiceCollection _services = services;
    private readonly ServiceLifetime _serviceLifetime = serviceLifetime;

    private GeocoderBuilderBase _geocoderBuilder = NoGeocoderBuilder.Instance;
    private MapBuilderBase _mapBuilder = NoMapBuilder.Instance;

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
        _ = bool.TryParse(section["ConsoleTraceLogGeocoder"], out var traceGeocoder);
        _ = int.TryParse(section["StreetGeometryMaxIterations"], out var streetGeometryMaxIterations);

        return ((IOpenStreetMapOptionsBuilder)this).Configure(
            () => new OpenStreetMapOptions
                {
                    ResourcePath = resourcePath,
                    Language = language,
                    ConsoleLogExceptions = consoleLog,
                    ConsoleTraceLogGeocoder = traceGeocoder,
                    StreetGeometryMaxIterations = streetGeometryMaxIterations
            }
        );
    }

    IOpenStreetMapApiBuilder IOpenStreetMapApiBuilder.AddMaps()
    {
        _mapBuilder = new MapBuilder();
        return this;
    }

    IOpenStreetMapApiBuilder IOpenStreetMapApiBuilder.AddMaps(Action<IMapBuilder> builder)
    {
        _mapBuilder = new MapBuilder();
        builder( _mapBuilder);
        return this;
    }

    IOpenStreetMapApiBuilder IOpenStreetMapApiBuilder.AddGeocoder()
    {
        _geocoderBuilder = new GeocoderBuilder();
        return this;
    }

    IOpenStreetMapApiBuilder IOpenStreetMapApiBuilder.AddGeocoder(Action<IGeocoderBuilder> builder)
    {
        _geocoderBuilder = new GeocoderBuilder();
        builder(_geocoderBuilder);
        return this;
    }

    void IOpenStreetMapApiBuilder.Register()
    {
        _services.Add(_optionsDescriptor);
        _mapBuilder.Register(_services, _serviceLifetime);
        _geocoderBuilder.Register(_services, _serviceLifetime);
    }
}