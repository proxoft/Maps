using System;
using System.Reactive.Linq;

namespace Proxoft.Maps.Core.Api;

public sealed class NoMarker : IMarker
{
    public static readonly NoMarker Instance = new();

    public IObservable<Event> OnEvent => Observable.Never<Event>();

    public void Dispose()
    {
    }

    public void SetDraggable(bool draggable)
    {
    }

    public void SetIcon(IconOptions icon)
    {
    }

    public void SetOpacity(Opacity opacity)
    {
    }

    public void SetPosition(LatLng latLng)
    {
    }

    public void SetPosition(decimal latitude, decimal longitude)
    {
    }
}
