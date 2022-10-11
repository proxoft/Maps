using System;
using System.Reactive.Linq;

namespace Proxoft.Maps.Core.Api;

public sealed class NoMap : IMap
{
    public static readonly NoMap Instance = new();

    private NoMap()
    {
    }

    public IObservable<Event> OnEvent => Observable.Never<Event>();

    public ApiStatus Status => ApiStatus.NotAvailable;

    public IMarker AddMarker(MarkerOptions options)
        => NoMarker.Instance;

    public void Dispose()
    {
    }

    public void FitBounds(LatLngBounds bounds)
    {
    }

    public void FitBounds(LatLngBounds bounds, Padding padding, ZoomLevel zoom)
    {
    }

    public void PanTo(LatLng center)
    {
    }

    public void SetCenter(LatLng position)
    {
    }

    public void ZoomTo(ZoomLevel zoom)
    {
    }
}
