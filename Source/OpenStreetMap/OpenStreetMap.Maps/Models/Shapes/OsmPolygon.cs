using System;
using Microsoft.JSInterop;
using Proxoft.Maps.Core.Api.Shapes.Polygones;

namespace Proxoft.Maps.OpenStreetMap.Maps.Models.Shapes;

internal sealed class OsmPolygon(
    string polygonId,
    Action<string> onRemove,
    IJSInProcessObjectReference jsModule) : Polygon(polygonId, onRemove, jsModule)
{
}
