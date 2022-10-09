namespace Proxoft.Maps.Core.Api;

public abstract class Event
{
    protected Event(EventSource source = EventSource.Js)
    {
        this.Source = source;
    }

    public string Name => this.GetType().Name;

    public EventSource Source { get; }
}
