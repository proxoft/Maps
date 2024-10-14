using System;

namespace Proxoft.Maps.Core.Api.Shapes.Polygones;

public class PolygonLatLng
{
    public static readonly PolygonLatLng Empty = new();

    public LatLng[] OuterRing { get; set; } = Array.Empty<LatLng>();

    public LatLng[][] Holes { get; set; } = Array.Empty<LatLng[]>();
}
