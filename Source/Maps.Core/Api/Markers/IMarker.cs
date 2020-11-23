using System;

namespace Proxoft.Maps.Core.Api
{
    public interface IMarker : IDisposable
    {
        IObservable<Event> OnEvent { get; }

        void SetPosition(decimal latitude, decimal longitude);
        void SetPosition(LatLng latLng);

        void SetDraggable(bool draggable);

        void SetOpacity(Opacity opacity);
    }
}
