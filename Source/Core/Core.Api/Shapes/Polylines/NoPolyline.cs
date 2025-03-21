﻿using System;
using System.Reactive.Linq;

namespace Proxoft.Maps.Core.Api.Shapes.Polylines;

public sealed class NoPolyline : IPolyline
{
    public static readonly NoPolyline Instance = new();

    private NoPolyline()
    {
    }

    public string Id => "none-polyline";

    public bool IsRemoved => false;

    public IObservable<Event> OnEvent => Observable.Never<Event>();

    public LatLngBounds GetBounds()
    {
        return LatLngBounds.Empty;
    }

    public LatLng[][] GetLatLngs()
    {
        return [];
    }

    public void Remove()
    {
    }

    public void SetLatLngs(LatLng[] latLngs)
    {
    }

    public void SetLatLngs(LatLng[][] latLngs)
    {
    }

    public void SetStyle(Style style)
    {
    }
    public void AddClass(params string[] classes)
    {
    }

    public void RemoveClass(params string[] classes)
    {
    }

    public void Dispose()
    {
    }
}
