using System;
using System.Linq;
using System.Reactive.Subjects;
using Microsoft.JSInterop;

namespace Proxoft.Maps.Core.Api;

public abstract class ApiObject : IApiObject
{
    private readonly Subject<Event> _events = new();
    private Action<string> _onRemove;
    private bool _isRemoved;

    protected ApiObject(
        string id,
        Action<string> onRemove,
        IJSInProcessObjectReference jsModule)
    {
        this.Id = id;
        _onRemove = onRemove;
        this.JsModule = jsModule;
    }

    public string Id { get; }

    protected IJSInProcessObjectReference JsModule { get; }

    internal Action<string> OnRemove
    {
        get => _onRemove;
        set => _onRemove = value;
    }

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

    protected void InvokeVoidJs(string identifier, params object?[] args)
    {
        this.InvokePureJs(identifier, new object?[] { this.Id }.Concat(args).ToArray());
    }

    protected TResult InvokeJs<TResult>(string identifier, params object?[] args)
    {
        return this.InvokePureJs<TResult>(identifier, new object?[] { this.Id }.Concat(args).ToArray());
    }

    protected void InvokePureJs(string identifier, params object?[] args)
    {
        if (this.IsRemoved)
        {
            throw new System.Exception($"The object {this.Id} has been removed from the map. Do not use it anymore. If necessary create new one");
        }

        this.JsModule.InvokeVoid(identifier, args);
    }

    protected TResult InvokePureJs<TResult>(string identifier, params object?[] args)
    {
        if (this.IsRemoved)
        {
            throw new System.Exception($"The object {this.Id} has been removed from the map. Do not use it anymore. If necessary create new one");
        }

        return this.JsModule.Invoke<TResult>(identifier, args);
    }

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            Console.WriteLine($"Disposing {this.Id}");

            this.Remove();
            _events.Dispose();
        }
    }
}
