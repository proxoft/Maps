using System;
using System.Reactive.Linq;

namespace Proxoft.Maps.Core.Api.Shapes.Circles;

public sealed class NoCircle : ICircle
{
    public static readonly NoCircle Instance = new NoCircle();
    private NoCircle() { }

    public CircleType CircleType => CircleType.Marker;

    public string Id => "none-circle";

    public bool IsRemoved => true;

    public IObservable<Event> OnEvent => Observable.Empty<Event>();

    public void AddClass(params string[] classes)
    {
    }

    public LatLng GetLatLng()
    {
        return LatLng.None;
    }

    public decimal GetRadius()
    {
        return 0;
    }

    public void Remove()
    {
    }

    public void RemoveClass(params string[] classes)
    {
    }

    public void SetLatLng(LatLng latLng)
    {
    }

    public void SetRadius(decimal radius)
    {
    }

    public void SetStyle(Style style)
    {
    }

    public void Dispose()
    {
    }
}
