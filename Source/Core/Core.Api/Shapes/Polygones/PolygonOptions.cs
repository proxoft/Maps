namespace Proxoft.Maps.Core.Api.Shapes.Polygones;

public record PolygonOptions
{
    public PolygonLatLng LatLngs { get; init; } = new();

    public Style Style { get; init; } = new Style();

    public bool TraceJs { get; init; }
}
