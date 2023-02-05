using System;
using Microsoft.JSInterop;
using Proxoft.Maps.Core.Api.Shapes;

namespace Proxoft.Maps.OpenStreetMap.Maps.Models.Shapes;

internal class OsmPolygon : Polygon
{
    public OsmPolygon(
        string polygonId,
        Action<string> onRemove,
        IJSInProcessObjectReference jsModule) : base(polygonId, onRemove, jsModule)
    {
    }
}
