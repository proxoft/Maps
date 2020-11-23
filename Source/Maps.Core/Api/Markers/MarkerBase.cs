using Microsoft.JSInterop;

namespace Proxoft.Maps.Core.Api
{
    public abstract class MarkerBase<T> : ApiBaseObject<T>, IMarker
        where T : MarkerBase<T>
    {
        protected MarkerBase(IJSInProcessObjectReference jsModule) : base(jsModule)
        {
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
            }

            base.Dispose(disposing);
        }
    }
}
