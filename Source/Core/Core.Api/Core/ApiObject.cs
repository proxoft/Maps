using System;
using System.Linq;
using System.Reactive.Subjects;
using Microsoft.JSInterop;

namespace Proxoft.Maps.Core.Api;

public abstract class ApiObject(
    string id,
    Action<string> onRemove,
    IJSInProcessObjectReference jsModule) : IApiObject
{
    private readonly Subject<Event> _events = new();
    private readonly Action<string> _onRemove = onRemove;
    private bool _isRemoved;
    private bool _disposed;

    public string Id { get; } = id;

    protected IJSInProcessObjectReference JsModule { get; } = jsModule;

    public IObservable<Event> OnEvent => _events;

    public bool IsRemoved => _isRemoved;

    public void Remove()
    {
        if (_isRemoved)
        {
            return;
        }

        _onRemove(this.Id);
        this.ExecuteRemove();
        _isRemoved = true;
    }

    protected abstract void ExecuteRemove();

    protected void Push(Event @event)
    {
        @event.SourceId = this.Id;
        _events.OnNext(@event);
    }

    protected void InvokeVoidJs(string jsMethodIdentifier, params object?[] args)
    {
        this.InvokePureJs(jsMethodIdentifier, [this.Id, .. args]);
    }

    protected TResult InvokeJs<TResult>(string jsMethodIdentifier, params object?[] args)
    {
        return this.InvokePureJs<TResult>(jsMethodIdentifier, [this.Id, .. args]);
    }

    protected void InvokePureJs(string jsMethodIdentifier, params object?[] args)
    {
        if (this.IsRemoved)
        {
            throw new Exception($"The object {this.Id} has been removed from the map. Do not use it anymore. If necessary create new one");
        }

        this.JsModule.InvokeVoid(jsMethodIdentifier, args);
    }

    protected TResult InvokePureJs<TResult>(string jsMethodIdentifier, params object?[] args)
    {
        if (this.IsRemoved)
        {
            throw new Exception($"The object {this.Id} has been removed from the map. Do not use it anymore. If necessary create new one");
        }

        return this.JsModule.Invoke<TResult>(jsMethodIdentifier, args);
    }

    public void Dispose()
    {
        if (_disposed)
        {
            Console.WriteLine($"disposing already disposed object {this.Id}");
        }

        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if(!disposing)
        {
            return;
        }

        _disposed = true;

        Console.WriteLine($"Disposing {this.Id}");
        this.Remove();

        _events.Dispose();

        Console.WriteLine($"Disposed {this.Id}");
    }
}
