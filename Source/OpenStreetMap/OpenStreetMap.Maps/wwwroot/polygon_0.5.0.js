import { findMapWrapper } from './maps_0.5.0.js';

console.log("osm polygon_0.5.0.js loaded");

var polygonWrappers = [];

// --exports----------------------------------
export function AddPolygon(polygonId, options, mapId, netRef) {
    console.log(options);

    let mapWrapper = findMapWrapper(mapId);
    let outerCoords = options.latLngs.outerRing.map(ll => [ll.latitude, ll.longitude]);
    let holesCoords = options.latLngs.holes.map(hole => hole.map(ll => [ll.latitude, ll.longitude]));

    let coords = [outerCoords].concat(holesCoords);
    console.log(coords);
    let polygon = L.polygon(coords, options.style);

    polygon.addTo(mapWrapper.map);

    let polygonWrapper = createPolygonWrapper(polygonId, polygon, mapWrapper.map, netRef, options.traceJs);
    polygonWrappers.push(polygonWrapper);

    polygonWrapper.log(`Added to the map ${mapId}`);
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