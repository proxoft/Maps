using System;

namespace Proxoft.Maps.Core.Api.Shapes;

public record PolygonOptions
{
    public PolygonLatLng LatLngs { get; init; } = new();

    public Style Style { get; init; } = new Style();

    public bool TraceJs { get; init; }
}
