using System;
using System.Reactive.Linq;

namespace Proxoft.Maps.Core.Api.Maps
{
    public sealed class NoMap : IMap
    {
        public static readonly NoMap Instance = new();

        private NoMap()
        {
        }

        public IObservable<LatLng> OnCenter => Observable.Never<LatLng>();

        public void Dispose()
        {
        }
    }
}
