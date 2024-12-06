using System;
using Microsoft.JSInterop;
using Proxoft.Maps.Core.Api.Shapes.Circles;

namespace Proxoft.Maps.OpenStreetMap.Maps.Models.Shapes;

internal sealed class OsmCircle(string id, Action<string> onRemove, IJSInProcessObjectReference jsModule) : Circle(id, onRemove, jsModule)
{
    protected override CircleType IfSupportedOrDefault(CircleType circleType)
    {
        return circleType;
    }
}
