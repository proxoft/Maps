using System;
using Proxoft.Maps.Core.Api.Shapes;

namespace Proxoft.Maps.Core.Api.Factories;

public interface IMapObjectsFactory
{
    Marker CreateMarker(Action<string> onRemove);

    Polygon CreatePolygon(Action<string> onRemove);
}
