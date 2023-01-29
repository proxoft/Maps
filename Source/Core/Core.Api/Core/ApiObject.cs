using System;
using System.Reactive.Subjects;
using Microsoft.JSInterop;

namespace Proxoft.Maps.Core.Api;

public abstract class ApiObject : IApiObject
{
    private bool _isRemoved;
    private readonly Subject<Event> _events = new();

    protected ApiObject(
        string id,
        IJSInProcessObjectReference jsModule)
    {
        this.Id = id;
        this.JsModule = jsModule;
    }

    public string Id { get; }

    protected IJSInProcessObjectReference JsModule { get; }

    public IObservable<Event> OnEvent => _events;

    public bool IsRemoved => _isRemoved;

    public void Remove()
    {
        if (_isRemoved)
        {
            return;
        }

        _isRemoved = true;
    }

    protected abstract void ExecuteRemove();

    protected void Push(Event @event)
    {
        @event.SourceId = this.Id;
        _events.OnNext(@event);
    }

    protected virtual void InvokeVoidJs(string identifier, params object?[] args)
        => JsModule.InvokeVoid(identifier, args);

    protected virtual TResult InvokeJs<TResult>(string identifier, params object?[] args)
        => JsModule.Invoke<TResult>(identifier, args);

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _events.Dispose();
        }
    }
}
