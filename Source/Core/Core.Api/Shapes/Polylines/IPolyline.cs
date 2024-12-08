namespace Proxoft.Maps.Core.Api.Shapes.Polylines;

public interface IPolyline : IShape
{
    public LatLngBounds GetBounds();

    public LatLng[][] GetLatLngs();

    public void SetLatLngs(LatLng[] latLngs);

    public void SetLatLngs(LatLng[][] latLngs);
}
