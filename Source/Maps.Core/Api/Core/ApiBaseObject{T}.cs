using Microsoft.JSInterop;

namespace Proxoft.Maps.Core.Api
{
    public abstract class ApiBaseObject<T> : ApiBaseObject
        where T : ApiBaseObject<T>
    {
        protected ApiBaseObject(IJSInProcessObjectReference jsModule)
        {
            this.SelfRef = DotNetObjectReference.Create((T)this);
            this.JsModule = jsModule;
        }

        protected IJSInProcessObjectReference JsModule { get; }
        protected DotNetObjectReference<T> SelfRef { get; private set; }

        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                this.SelfRef.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
