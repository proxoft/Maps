using System;
using System.Reactive.Linq;

namespace Proxoft.Maps.Core.Api.Shapes.Rectangles;

public sealed class NoRectangle : IRectangle
{
    private NoRectangle()
    {
    }

    public static readonly NoRectangle Instance = new();

    public string Id => "none-rectangle";

    public bool IsRemoved => true;

    public IObservable<Event> OnEvent => Observable.Never<Event>();

    public void AddClass(params string[] classes)
    {
    }

    public void Dispose()
    {
    }

    public void Remove()
    {
    }

    public void RemoveClass(params string[] classes)
    {
    }

    public void SetBounds(LatLngBounds bounds)
    {
    }

    public void SetStyle(Style style)
    {
    }

    public LatLngBounds GetBounds()
    {
        return LatLngBounds.Empty;
    }

    public LatLng GetCenter()
    {
        return LatLng.None;
    }

    public void SetCenter(LatLng latLng)
    {
    }
}
