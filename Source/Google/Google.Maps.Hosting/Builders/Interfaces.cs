using System;
using Microsoft.Extensions.Configuration;
using Proxoft.Maps.Google.Common;

namespace Google.Maps.Hosting.Builders
{
    public interface IGoogleApiConfigurationBuilder
    {
        IGoogleLoaderBuilder Configure(Func<GoogleApiConfiguration> factory);

        /// <summary>
        /// Expected configuration:
        /// "GoogleApi": {
        ///     "ApiKey": "AIza...",
        ///     "Language": "en",
        ///     "Region": "en"
        /// }
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="sectionPath"></param>
        IGoogleLoaderBuilder Configure(IConfiguration configuration);

        /// <summary>
        /// Expected configuration:
        /// "{sectionPath}": {
        ///     "ApiKey": "AIza...",
        ///     "Language": "en",
        ///     "Region": "en"
        /// }
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="sectionPath"></param>
        IGoogleLoaderBuilder Configure(IConfiguration configuration, string sectionPath);

        IGoogleLoaderBuilder Configure(IConfigurationSection section);

        /// <summary>
        /// Registers as singleton regardles of chosen ServiceLifetime.
        /// </summary>
        /// <param name="instance">The instance.</param>
        IGoogleLoaderBuilder UseInstance(GoogleApiConfiguration instance);
    }

    public interface IGoogleLoaderBuilder
    {
        void Build();
    }
}
