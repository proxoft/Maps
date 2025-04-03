using System;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Proxoft.Maps.OpenStreetMap.Common;
using Proxoft.Maps.OpenStreetMap.Geocoding.Parsing;
using Proxoft.Maps.OpenStreetMap.Maps;

namespace Proxoft.Maps.OpenStreetMap.Hosting.Builders;

public interface IOpenStreetMapOptionsBuilder
{
    IOpenStreetMapApiBuilder Configure(Func<OpenStreetMapOptions> factory);
    IOpenStreetMapApiBuilder Configure(IConfiguration configuration);
    IOpenStreetMapApiBuilder Configure(IConfiguration configuration, string sectionPath);
    IOpenStreetMapApiBuilder Configure(IConfigurationSection configurationSection);
    IOpenStreetMapApiBuilder UseInstance(OpenStreetMapOptions options);
}

public interface IOpenStreetMapApiBuilder
{
    IOpenStreetMapApiBuilder AddMaps();

    IOpenStreetMapApiBuilder AddMaps(Action<IMapBuilder> builder);

    IOpenStreetMapApiBuilder AddGeocoder();

    IOpenStreetMapApiBuilder AddGeocoder(Action<IGeocoderBuilder> builder);

    void Register();
}

public interface IMapBuilder
{
    IMapBuilder UseIdFactory<TIdFactory>() where TIdFactory : IIdFactory;
}

public interface IGeocoderBuilder
{
    IGeocoderBuilder UseParser<TParser>() where TParser : IOsmResultParser;

    IGeocoderBuilder UseHttpClient(Action<HttpClient> configureClient);
}