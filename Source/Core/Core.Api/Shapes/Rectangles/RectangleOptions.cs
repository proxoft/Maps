namespace Proxoft.Maps.Core.Api.Shapes.Rectangles;

public class RectangleOptions
{
    public LatLngBounds Bounds { get; init; } = LatLngBounds.Empty;

    public bool Draggable { get; init; }

    public Style Style { get; init; } = new Style();

    public bool TraceJs { get; init; }
}
