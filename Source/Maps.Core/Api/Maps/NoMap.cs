using System;
using System.Reactive.Linq;
using Proxoft.Maps.Core.Api.Markers;

namespace Proxoft.Maps.Core.Api.Maps
{
    public sealed class NoMap : IMap
    {
        public static readonly NoMap Instance = new();

        private NoMap()
        {
        }

        public IObservable<Event> OnEvent => Observable.Never<Event>();

        public IMarker AddMarker(MarkerOptions options)
            => NoMarker.Instance;

        public void Dispose()
        {
        }

        public void PanTo(LatLng center)
        {
        }

        public void ZoomTo(int zoom)
        {
        }
    }
}
