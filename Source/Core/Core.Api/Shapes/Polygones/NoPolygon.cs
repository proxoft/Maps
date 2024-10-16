﻿using System;
using System.Reactive.Linq;

namespace Proxoft.Maps.Core.Api.Shapes.Polygones;

public sealed class NoPolygon : IPolygon
{
    public static readonly NoPolygon Instance = new();

    private NoPolygon()
    {
        this.OnEvent = Observable.Never<Event>();
    }

    public IObservable<Event> OnEvent { get; }

    public string Id => "none-polygon";

    public bool IsRemoved => false;

    public void Dispose()
    {
    }

    public LatLngBounds GetBounds()
    {
        return LatLngBounds.Empty;
    }

    public PolygonLatLng GetLatLngs()
    {
        return PolygonLatLng.Empty;
    }

    public void Remove()
    {
    }

    public void SetLatLng(PolygonLatLng latLngs)
    {
    }

    public void SetStyle(Style style)
    {
    }
}
