using System;
using Microsoft.JSInterop;

namespace Proxoft.Maps.Core.Api.Core;

internal abstract class JsCallback : IDisposable
{
    private static long _id;

    protected JsCallback()
    {
        this.Id = ++_id;
        Console.WriteLine($"created js callback {this.Id}");
    }

    public long Id { get; }

    protected bool Disposed { get; private set; }

    public void Dispose()
    {
        Console.WriteLine($"disposing js callback {this.Id}");

        this.Dispose(true);
        this.Disposed = true;

        Console.WriteLine($"disposed js callback {this.Id}");

        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
    }
}

internal abstract class ApiObjectJsCallback<T> : JsCallback
    where T : ApiObjectJsCallback<T>
{
    private readonly Action<Event> _onEvent;

    protected ApiObjectJsCallback(Action<Event> onEvent)
    {
        _onEvent = onEvent;
        this.DotNetRef = DotNetObjectReference.Create((T)this);
    }

    public DotNetObjectReference<T> DotNetRef { get; }

    [JSInvokable]
    public void OnMouseClick(LatLng latLng)
        => this.Push(new MouseClickEvent(latLng));

    [JSInvokable]
    public void OnMouseDoubleClick(LatLng latLng)
        => this.Push(new MouseDoubleClickEvent(latLng));

    [JSInvokable]
    public void OnMouseDown(LatLng latLng)
        => this.Push(new MouseDownEvent(latLng));

    [JSInvokable]
    public void OnMouseUp(LatLng latLng)
        => this.Push(new MouseUpEvent(latLng));

    [JSInvokable]
    public void OnMouseEnter(LatLng latLng)
        => this.Push(new MouseEnterEvent(latLng));

    [JSInvokable]
    public void OnMouseMove(LatLng latLng)
        => this.Push(new MouseMoveEvent(latLng));

    [JSInvokable]
    public void OnMouseLeave(LatLng latLng)
        => this.Push(new MouseLeaveEvent(latLng));

    protected void Push(Event @event)
    {
        if (this.Disposed)
        {
            return;
        }

        _onEvent(@event);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            this.DotNetRef.Dispose();
        }

        base.Dispose(disposing);
    }
}
