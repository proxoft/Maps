using System;

namespace Proxoft.Maps.Core.Api;

public interface IApiObject : IDisposable
{
    string Id { get; }

    IObservable<Event> OnEvent { get; }
}
