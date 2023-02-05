import { findMapWrapper } from './maps_0.6.0.js';

console.log("osm polygon_0.6.0.js loaded");

var polygonWrappers = [];

// --exports----------------------------------
export function AddPolygon(polygonId, options, mapId, netRef) {
    let mapWrapper = findMapWrapper(mapId);
    let outerCoords = options.latLngs.outerRing.map(ll => [ll.latitude, ll.longitude]);
    let holesCoords = options.latLngs.holes.map(hole => hole.map(ll => [ll.latitude, ll.longitude]));

    let coords = [outerCoords].concat(holesCoords);
    let polygon = L.polygon(coords, options.style);

    polygon.addTo(mapWrapper.map);

    let polygonWrapper = createPolygonWrapper(polygonId, polygon, mapWrapper.map, netRef, options.traceJs);
    polygonWrappers.push(polygonWrapper);

    polygonWrapper.log(`Added to the map ${mapId}`);
}

export function RemovePolygon(polygonId) {
    let i = findPolygonWrapperIndex(polygonId);
    let wrapper = polygonWrappers.splice(i, 1);
    wrapper[0].log("removing from map");
    wrapper[0].polygon.remove();
}

export function SetLatLngs(polygonId, latLngs) {
    let polygonWrapper = findPolygonWrapper(polygonId);
    polygonWrapper.log(`SetLatLngs ${latLngs}`);

    let outerCoords = options.latLngs.outerRing.map(ll => [ll.latitude, ll.longitude]);
    let holesCoords = options.latLngs.holes.map(hole => hole.map(ll => [ll.latitude, ll.longitude]));

    let coords = [outerCoords].concat(holesCoords);
    polygonWrapper.polygon.setLatLngs(coords);
}

export function GetLatLngs(polygonId) {
    let polygonWrapper = findPolygonWrapper(polygonId);
    polygonWrapper.log("GetLatLngs");

    let latLngs = polygonWrapper.polygon.getLatLngs();

    if (latLngs.length == 0) {
        return {
            outerRing: [],
            holes: []
        };
    }

    let outerRing = latLngs[0]
        .map(latlng => latLngToObject(latlng));

    let holes = latLngs
        .slice(1)
        .map(h =>
            h.map(latlng => latLngToObject(latlng))
        );

    return {
        outerRing: outerRing,
        holes: holes
    };
}

export function GetBounds(polygonId) {
    let polygonWrapper = findPolygonWrapper(polygonId);
    polygonWrapper.log("getBounds");

    let bounds = polygonWrapper.polygon.getBounds();

    return [
        {
            latitude: bounds.getSouth(),
            longitude: bounds.getWest()
        },
        {
            latitude: bounds.getNorth(),
            longitude: bounds.getEast()
        }
    ];
}

export function setStyle(polygonId, style) {
    let polygonWrapper = findPolygonWrapper(polygonId);
    polygonWrapper.log("setStyle");
    console.log(style);

    polygonWrapper.polygon.setStyle(style);
    polygonWrapper.polygon.redraw();
}

// -- private
function createPolygonWrapper(polygonId, polygon, map, netRef, enableLogging) {

    let wrapper = {
        polygonId: polygonId,
        parentMap: map,
        polygon: polygon, // polygon instance
        ref: netRef,      // net object reference

        invokeRef: function (...args) {
            try {
                wrapper.ref.invokeMethodAsync(...args);
            }
            catch (e) {
                console.log(e);
            }
        },

        log: function (m) {
            if (!enableLogging) {
                return;
            }
            console.log("[Polygon " + polygonId + "]:");
            console.log(m)
        }
    };

    //-- mouse events
    polygon.on("click", (e) => {
        wrapper.invokeRef("OnMouseClick", { latitude: e.latlng.lat, longitude: e.latlng.lng })
    });

    polygon.on("dblclick", (e) => {
        wrapper.invokeRef("OnMouseDoubleClick", { latitude: e.latlng.lat, longitude: e.latlng.lng })
    });

    polygon.on("mousedown", (e) => {
        wrapper.invokeRef("OnMouseDown", { latitude: e.latlng.lat, longitude: e.latlng.lng })
    });

    polygon.on("mouseup", (e) => {
        wrapper.invokeRef("OnMouseUp", { latitude: e.latlng.lat, longitude: e.latlng.lng })
    });

    polygon.on("mouseover", (e) => {
        wrapper.invokeRef("OnMouseEnter", { latitude: e.latlng.lat, longitude: e.latlng.lng })
    });

    polygon.on("mouseout", (e) => {
        wrapper.invokeRef("OnMouseLeave", { latitude: e.latlng.lat, longitude: e.latlng.lng })
    });

    return wrapper;
}

function findPolygonWrapper(polygonId) {
    let i = findPolygonWrapperIndex(polygonId);
    return i === -1
        ? null
        : polygonWrappers[i];
}

function findPolygonWrapperIndex(polygonId) {
    let i = polygonWrappers.findIndex(me => me.polygonId === polygonId);
    return i;
}

function latLngToObject(latlng) {
    return {
        latitude: latlng.lat,
        longitude: latlng.lng
    };
}