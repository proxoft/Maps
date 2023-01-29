using System;
using Microsoft.JSInterop;
using Proxoft.Maps.Core.Api;

namespace Proxoft.Maps.OpenStreetMap.Maps.Models.Markers
{
    internal class OsmMarker : Marker, IMarker
    {
        public OsmMarker(
            string markerId,
            Action<string> onRemove,
            IJSInProcessObjectReference jsModule) : base(markerId, onRemove, jsModule)
        {
        }
    }
}
