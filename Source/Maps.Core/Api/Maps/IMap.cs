using System;

namespace Proxoft.Maps.Core.Api
{
    public interface IMap: IDisposable
    {
        IObservable<LatLng> OnCenter { get; }

        void PanTo(LatLng center);
    }
}
