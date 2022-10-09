using Microsoft.JSInterop;

namespace Proxoft.Maps.Core.Api;

public abstract class ApiBaseObject<T> : ApiBaseObject
    where T : ApiBaseObject<T>
{
    

    protected ApiBaseObject(IJSInProcessObjectReference jsModule) : base(jsModule)
    {
        this.SelfRef = DotNetObjectReference.Create((T)this);
    }

    

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
