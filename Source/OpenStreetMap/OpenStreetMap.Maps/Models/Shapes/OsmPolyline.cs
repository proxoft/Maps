using System;
using Microsoft.JSInterop;
using Proxoft.Maps.Core.Api.Shapes.Polylines;

namespace Proxoft.Maps.OpenStreetMap.Maps.Models.Shapes;

internal class OsmPolyline : Polyline
{
    public OsmPolyline(string id, Action<string> onRemove, IJSInProcessObjectReference jsModule) : base(id, onRemove, jsModule)
    {
    }
}
