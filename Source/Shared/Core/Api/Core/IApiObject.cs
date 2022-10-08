using System;

namespace Proxoft.Maps.Core.Api
{
    public interface IApiObject : IDisposable
    {
        IObservable<Event> OnEvent { get; }
    }
}
