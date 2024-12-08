namespace Proxoft.Maps.Core.Api.Shapes.Circles;

public class CircleOptions
{
    public LatLng LatLng { get; set; } = LatLng.None;

    public CircleType CircleType { get; set; } = CircleType.Marker;

    /// <summary>
    /// Radius dependes on CircleType (if more are supported).
    /// If "marker", radius is in pixels
    /// If "circle", radius is in meters
    /// </summary>
    public decimal Radius { get; set; }

    public Style Style { get; set; } = new();

    public bool TraceJs { get; set; } = true;
}
