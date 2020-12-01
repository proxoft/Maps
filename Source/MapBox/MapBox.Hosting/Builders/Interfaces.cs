using System;
using Microsoft.Extensions.Configuration;
using Proxoft.Maps.MapBox.Common;

namespace Proxoft.Maps.MapBox.Hosting.Builders
{
    public interface IMapBoxOptionsBuilder
    {
        IMapBoxApiBuilder Configure(Func<MapBoxOptions> factory);
        IMapBoxApiBuilder Configure(IConfiguration configuration);
        IMapBoxApiBuilder Configure(IConfiguration configuration, string sectionPath);
        IMapBoxApiBuilder Configure(IConfigurationSection configurationSection);

        /// <summary>
        /// Registers as singleton regardles of chosen ServiceLifetime.
        /// </summary>
        /// <param name="instance">The instance.</param>
        IMapBoxApiBuilder UseInstance(MapBoxOptions options);
    }

    public interface IMapBoxApiBuilder
    {
        IMapBoxApiBuilder AddStaticMapService();
        void Register();
    }
}
