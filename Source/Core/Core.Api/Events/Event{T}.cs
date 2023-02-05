namespace Proxoft.Maps.Core.Api;

public abstract class Event<T> : Event
{
    protected Event(T value)
    {
        this.Value = value;
    }

    public T Value { get; }

    public static implicit operator T(Event<T> @event) => @event.Value;
}
