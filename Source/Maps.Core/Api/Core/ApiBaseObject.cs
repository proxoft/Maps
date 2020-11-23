using System;
using System.Reactive.Subjects;
using Microsoft.JSInterop;

namespace Proxoft.Maps.Core.Api
{
    public abstract class ApiBaseObject : IDisposable
    {
        private readonly Subject<Event> _events = new();

        protected ApiBaseObject(IJSInProcessObjectReference jsModule)
        {
            this.JsModule = jsModule;
        }

        public IJSInProcessObjectReference JsModule { get; }

        public IObservable<Event> OnEvent => _events;

        protected void Push(Event @event)
            => _events.OnNext(@event);

        protected void InvokeVoidJs(string identifier, params object[] args)
            => JsModule.InvokeVoid(identifier, args);

        protected void InvokeJs<TResult>(string identifier, params object[] args)
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
}
