namespace Proxoft.Maps.Core.Api.Shapes.Circles;

public interface ICircle : IShape
{
    CircleType CircleType { get; }

    void SetLatLng(LatLng latLng);

    LatLng GetLatLng();

    /// <summary>
    /// Radius dependes on CircleType (if more are supported).
    /// If "marker", radius is in pixels
    /// If "circle", radius is in meters
    /// </summary>
    void SetRadius(decimal radius);

    decimal GetRadius();
}
