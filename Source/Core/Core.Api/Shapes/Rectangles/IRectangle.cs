namespace Proxoft.Maps.Core.Api.Shapes.Rectangles;

public interface IRectangle : IShape
{
    LatLngBounds GetBounds();

    void SetBounds(LatLngBounds bounds);

    LatLng GetCenter();

    void SetCenter(LatLng latLng);
}
