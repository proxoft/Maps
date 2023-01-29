using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Proxoft.Maps.Core.Api;
using Proxoft.Maps.Core.Api.Maps;
using Proxoft.Maps.Core.Api.Shapes;
using Proxoft.Maps.OpenStreetMap.Maps.Models.Markers;
using Proxoft.Maps.OpenStreetMap.Maps.Models.Shapes;

namespace Proxoft.Maps.OpenStreetMap.Maps.Models.Maps
{
    internal class OsmMap : Map
    {
        private readonly List<OsmMarker> _markers = new();
        private readonly Hooks _markerHooks;

        private readonly List<OsmPolygon> _polygons = new();

        private readonly IIdFactory _idFactory;
        private readonly OsmModules _modules;

        private OsmMap(
            string mapId,
            IIdFactory idFactory,
            OsmModules modules) : base(mapId, modules.Map)
        {
            _markerHooks = new()
            {
                OnRemove = this.OnMarkerRemove
            };
            _idFactory = idFactory;
            _modules = modules;
        }

        public override IMarker AddMarker(MarkerOptions options)
        {
            OsmMarker marker = new (_idFactory.NextMarkerId(), _modules.Marker, _markerHooks);

            _markers.Add(marker);
            marker.AddToMap(this.MapId, options);
            return marker;
        }

        public override IPolygon AddPolygon(PolygonOptions options)
        {
            OsmPolygon polygon = new(_idFactory.NextPolygonId(), _modules.Polygons, this.OnPolygonRemoved);
            _polygons.Add(polygon);

            polygon.AddToMap(this.MapId, options);

            return polygon;
        }

        public static OsmMap Create(
            IIdFactory idFactory,
            MapOptions options,
            ElementReference hostElement,
            OsmModules modules)
        {
            string mapId = idFactory.NextMapId();
            var map = new OsmMap(mapId, idFactory, modules);
            map.Initialize(options, hostElement);
            return map;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _markerHooks.OnRemove = _ => { };

                foreach(var m in _markers)
                {
                    m.Dispose();
                }

                _markers.Clear();
            }

            base.Dispose(disposing);
        }

        private void OnMarkerRemove(string markerId)
        {
            var i = _markers.FindIndex(m => m.Id == markerId);
            _markers.RemoveAt(i);
        }

        private void OnPolygonRemoved(string polygonId)
        {
            var i = _polygons.FindIndex(p => p.Id == polygonId);
            _polygons.RemoveAt(i);
        }
    }
}
