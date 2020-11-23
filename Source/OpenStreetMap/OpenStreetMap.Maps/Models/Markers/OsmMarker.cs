using Microsoft.JSInterop;
using Proxoft.Maps.Core.Api;

namespace Proxoft.Maps.OpenStreetMap.Maps.Models.Markers
{
    internal class OsmMarker : MarkerBase<OsmMarker>, IMarker
    {
        private readonly string _markerId;

        public OsmMarker(string markerId, IJSInProcessObjectReference jsModule) : base(jsModule)
        {
            _markerId = markerId;
        }

        public void AddToMap(string mapId, MarkerOptions options)
        {
        }
    }
}
