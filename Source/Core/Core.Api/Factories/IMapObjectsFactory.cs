using System;
using Proxoft.Maps.Core.Api.Shapes.Circles;
using Proxoft.Maps.Core.Api.Shapes.Polygones;
using Proxoft.Maps.Core.Api.Shapes.Polylines;

namespace Proxoft.Maps.Core.Api.Factories;

public interface IMapObjectsFactory
{
    Marker CreateMarker(Action<string> onRemove);

    Polygon CreatePolygon(Action<string> onRemove);

    Polyline CreatePolyline(Action<string> onRemove);

    Circle CreateCircle(Action<string> onRemove);
}
