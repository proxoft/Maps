using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Proxoft.Maps.Core.Abstractions.Geocoding;
using Proxoft.Maps.OpenStreetMap.Geocoding;
using Proxoft.Maps.OpenStreetMap.Geocoding.Parsing;

namespace Proxoft.Maps.OpenStreetMap.Hosting.Builders;


internal abstract class GeocoderBuilderBase : IGeocoderBuilder
{
    public abstract IGeocoderBuilder UseHttpClient(Action<HttpClient> options);

    public abstract IGeocoderBuilder UseParser<TParser>() where TParser : IOsmResultParser;

    public abstract void Register(IServiceCollection services, ServiceLifetime serviceLifetime);
}

internal class GeocoderBuilder : GeocoderBuilderBase
{
    private readonly Uri _uri = new("https://nominatim.openstreetmap.org/");
    private Type _parser = typeof(OsmDefaultResultParser);
    private Action<HttpClient> _configureClient = _ => { };

    public override void Register(IServiceCollection services, ServiceLifetime serviceLifetime)
    {
        services.Add(new ServiceDescriptor(typeof(IGeocoder), typeof(OsmGeocoder), serviceLifetime));
        services.Add(new ServiceDescriptor(typeof(IOsmResultParser), _parser, serviceLifetime));
        services.AddHttpClient<IGeocoder, OsmGeocoder>(http =>
        {
            http.BaseAddress = _uri;
            _configureClient(http);
        });
    }

    public override IGeocoderBuilder UseHttpClient(Action<HttpClient> configureClient)
    {
        _configureClient = configureClient;
        return this;
    }

    public override IGeocoderBuilder UseParser<TParser>()
    {
        _parser = typeof(TParser);
        return this;
    }
}

internal class NoGeocoderBuilder : GeocoderBuilderBase
{
    public static readonly NoGeocoderBuilder Instance = new();

    private NoGeocoderBuilder()
    {
    }

    public override void Register(IServiceCollection services, ServiceLifetime serviceLifetime)
    {
    }

    public override IGeocoderBuilder UseHttpClient(Action<HttpClient> options)
    {
        return this;
    }

    public override IGeocoderBuilder UseParser<TParser>()
    {
        return this;
    }
}
