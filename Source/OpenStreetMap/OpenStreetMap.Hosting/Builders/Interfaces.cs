using System;
using Microsoft.Extensions.Configuration;
using Proxoft.Maps.OpenStreetMap.Common;
using Proxoft.Maps.OpenStreetMap.Geocoding.Parsing;

namespace Proxoft.Maps.OpenStreetMap.Hosting.Builders
{
    public interface IOpenStreetMapOptionsBuilder
    {
        IOpenStreetMapApiBuilder Configure(Func<OpenStreetMapOptions> factory);
        IOpenStreetMapApiBuilder Configure(IConfiguration configuration);
        IOpenStreetMapApiBuilder Configure(IConfiguration configuration, string sectionPath);
        IOpenStreetMapApiBuilder Configure(IConfigurationSection configurationSection);

        /// <summary>
        /// Registers as singleton regardles of chosen ServiceLifetime.
        /// </summary>
        /// <param name="instance">The instance.</param>
        IOpenStreetMapApiBuilder UseInstance(OpenStreetMapOptions options);
    }

    public interface IGeocoderBuilder
    {
        IGeocoderBuilder UseParser<TParser>() where TParser : IOsmResultParser;
    }

    public interface IOpenStreetMapApiBuilder
    {
        IOpenStreetMapApiBuilder AddGeocoder();
        IOpenStreetMapApiBuilder AddGeocoder(Action<IGeocoderBuilder> builder);

        void Register();
    }
}
