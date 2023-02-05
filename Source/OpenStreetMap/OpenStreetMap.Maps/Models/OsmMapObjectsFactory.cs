using System;
using Microsoft.Extensions.Options;
using System.Reflection;
using Proxoft.Maps.Core.Api;
using Proxoft.Maps.Core.Api.Maps;
using Proxoft.Maps.Core.Api.Shapes;
using Proxoft.Maps.OpenStreetMap.Maps.Models.Maps;
using Proxoft.Maps.OpenStreetMap.Maps.Models.Markers;
using Proxoft.Maps.OpenStreetMap.Maps.Models.Shapes;
using Proxoft.Extensions.Options;
using Microsoft.AspNetCore.Components;
using Proxoft.Maps.Core.Api.Factories;

namespace Proxoft.Maps.OpenStreetMap.Maps.Models;

internal class OsmMapObjectsFactory : IMapObjectsFactory
{
    private readonly IIdFactory _idFactory;
    private readonly OsmModules _osmModules;

    public OsmMapObjectsFactory(
        IIdFactory idFactory,
        OsmModules osmModules)
    {
        _idFactory = idFactory;
        _osmModules = osmModules;
    }

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
}
