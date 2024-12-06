using System;
using System.Reactive.Linq;

namespace Proxoft.Maps.Core.Api.Shapes.Polygones;

public sealed class NoPolygon : IPolygon
{
    public static readonly NoPolygon Instance = new();

    private NoPolygon()
    {
        this.OnEvent = Observable.Never<Event>();
    }

    public IObservable<Event> OnEvent { get; } = Observable.Empty<Event>();

    public string Id => "none-polygon";

    public bool IsRemoved => false;

    public void AddClass(params string[] classes)
    {
    }

    public void Dispose()
    {
    }

    public LatLngBounds GetBounds()
    {
        return LatLngBounds.Empty;
    }

    public PolygonLatLng GetLatLngs()
    {
        return PolygonLatLng.Empty;
    }

    public void Remove()
    {
    }

    public void RemoveClass(params string[] classes)
    {
    }

    public void SetLatLng(PolygonLatLng latLngs)
    {
    }

    public void SetStyle(Style style)
    {
    }
}
