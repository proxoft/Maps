using System;
using Proxoft.Maps.Core.Api;
using Proxoft.Maps.Core.Api.Maps;
using Proxoft.Maps.OpenStreetMap.Maps.Models.Maps;
using Proxoft.Maps.OpenStreetMap.Maps.Models.Markers;
using Proxoft.Maps.OpenStreetMap.Maps.Models.Shapes;
using Microsoft.AspNetCore.Components;
using Proxoft.Maps.Core.Api.Factories;
using Proxoft.Maps.Core.Api.Shapes.Polylines;
using Proxoft.Maps.Core.Api.Shapes.Polygones;
using Proxoft.Maps.Core.Api.Shapes.Circles;

namespace Proxoft.Maps.OpenStreetMap.Maps.Models;

internal class OsmMapObjectsFactory(
    IIdFactory idFactory,
    OsmModules osmModules) : IMapObjectsFactory
{
    private readonly IIdFactory _idFactory = idFactory;
    private readonly OsmModules _osmModules = osmModules;

    public Map CreateMap(MapOptions options, ElementReference hostElement)
    {
        var map = new OsmMap(_idFactory.NextMapId(), this, _osmModules.Map);
        map.Initialize(options, hostElement);
        return map;
    }

    public Marker CreateMarker(Action<string> onRemove)
    {
        return new OsmMarker(_idFactory.NextMarkerId(), onRemove, _osmModules.Marker);
    }

    public Polygon CreatePolygon(Action<string> onRemove)
    {
        return new OsmPolygon(_idFactory.NextPolygonId(), onRemove, _osmModules.Polygon);
    }

    public Polyline CreatePolyline(Action<string> onRemove)
    {
        return new OsmPolyline(_idFactory.NextPolylineId(), onRemove, _osmModules.Polyline);
    }

    public Circle CreateCircle(Action<string> onRemove)
    {
        return new OsmCircle(_idFactory.NextCircleId(), onRemove, _osmModules.Circle);
    }
}
