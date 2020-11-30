using Microsoft.JSInterop;
using Proxoft.Maps.Core.Api;

namespace Proxoft.Maps.OpenStreetMap.Maps.Models.Markers
{
    internal class OsmMarker : MarkerBase<OsmMarker>, IMarker
    {
        public OsmMarker(string markerId, IJSInProcessObjectReference jsModule) : base(markerId, jsModule)
        {
        }
    }
}
