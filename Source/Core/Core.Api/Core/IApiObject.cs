using System;

namespace Proxoft.Maps.Core.Api;

public interface IApiObject : IDisposable
{
    string Id { get; }

    bool IsRemoved { get; }

    IObservable<Event> OnEvent { get; }

    void Remove();
}
