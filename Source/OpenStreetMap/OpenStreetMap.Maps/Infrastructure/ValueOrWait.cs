using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Proxoft.Maps.OpenStreetMap.Maps.Infrastructure;

internal class ValueOrWait<T> : IObservable<T>
{
    private readonly BehaviorSubject<(bool hasValue, T? value)> _value;

    private ValueOrWait(T? noValue)
    {
        _value = new BehaviorSubject<(bool hasValue, T? value)>((false, noValue));
    }

    public static ValueOrWait<T> Empty()
    {
        return new ValueOrWait<T>(default);
    }

    public void SetValue(T value)
    {
        _value.OnNext((true, value));
    }

    public IDisposable Subscribe(IObserver<T> observer)
    {
        return _value
            .Where(v => v.hasValue)
            .Select(v => v.value!)
            .Subscribe(observer);
    }
}
