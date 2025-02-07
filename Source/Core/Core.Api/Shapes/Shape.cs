using Microsoft.JSInterop;
using System;

namespace Proxoft.Maps.Core.Api.Shapes;

public abstract class Shape(string id, Action<string> onRemove, IJSInProcessObjectReference jsModule) : ApiObject(id, onRemove, jsModule), IShape
{
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
