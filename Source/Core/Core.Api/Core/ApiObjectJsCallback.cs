using System;
using Microsoft.JSInterop;

namespace Proxoft.Maps.Core.Api.Core;

internal abstract class ApiObjectJsCallback<T> : IDisposable
    where T : ApiObjectJsCallback<T>
{
    private bool _disposed;
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

    public void Dispose()
    {
        this.Dispose(true);
        _disposed = true;

        GC.SuppressFinalize(this);
    }

    protected void Push(Event @event)
    {
        if (_disposed)
        {
            return;
        }

        _onEvent(@event);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            this.DotNetRef.Dispose();
        }
    }
}
