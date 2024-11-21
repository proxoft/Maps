using Microsoft.JSInterop;
using System;

namespace Proxoft.Maps.Core.Api.Shapes;

public abstract class Shape : ApiObject, IShape
{
    protected Shape(string id, Action<string> onRemove, IJSInProcessObjectReference jsModule): base(id, onRemove, jsModule)
    {
    }

    public void AddClass(params string[] classes)
    {
        this.InvokeVoidJs("AddClass", string.Join(" ", classes));
    }

    public void RemoveClass(params string[] classes)
    {
        this.InvokeVoidJs("RemoveClass", string.Join(" ", classes));
    }

    public void SetStyle(Style style)
    {
        this.InvokeVoidJs("SetStyle", style);
    }
}
