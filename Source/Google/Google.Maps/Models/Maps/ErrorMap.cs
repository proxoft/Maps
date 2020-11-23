using System;
using System.Reactive.Linq;
using Proxoft.Maps.Core.Api;

namespace Proxoft.Maps.Google.Maps.Models.Maps
{
    internal class ErrorMap : IMap
    {
        public IObservable<LatLng> OnCenter => Observable.Never<LatLng>();

        public void Dispose()
        {
        }
    }
}
