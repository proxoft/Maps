import { findMapWrapper } from './maps_0.10.1.js';

console.log("osm polygon_0.10.1.js loaded");

var polygonWrappers = [];

// --exports----------------------------------
export function AddPolygon(polygonId, options, mapId, netRef) {
    if (options.traceJs) {
        console.log(`addPolygon >> polygonId ${ polygonId }, options: ${ JSON.stringify(options) }, mapId ${ mapId }`)
    }

    let mapWrapper = findMapWrapper(mapId);

    let outerCoords = options.latLngs.outerRing.map(ll => [ll.latitude, ll.longitude]);
    let holesCoords = options.latLngs.holes.map(hole => hole.map(ll => [ll.latitude, ll.longitude]));

    let coords = [outerCoords].concat(holesCoords);
    let polygon = L.polygon(coords, options.style);

    polygon.addTo(mapWrapper.map);

    let polygonWrapper = createPolygonWrapper(polygonId, polygon, mapWrapper.map, netRef, options.traceJs);
    polygonWrappers.push(polygonWrapper);

    polygonWrapper.log(`addPolygon >>>> Added to the map ${mapId}`);
}

export function RemovePolygon(polygonId) {
    let i = findPolygonWrapperIndex(polygonId);
    let wrapper = polygonWrappers.splice(i, 1);
    wrapper[0].log("removePolygon >>");
    wrapper[0].disconnect();
    wrapper[0].polygon.remove();
    wrapper[0].log("removePolygon >>>> removed from map");
}

export function SetLatLngs(polygonId, latLngs) {
    let polygonWrapper = findPolygonWrapper(polygonId);
    polygonWrapper.log(`setLatLngs >> latLngs ${JSON.stringify(latLngs)}`);

    let outerCoords = options.latLngs.outerRing.map(ll => [ll.latitude, ll.longitude]);
    let holesCoords = options.latLngs.holes.map(hole => hole.map(ll => [ll.latitude, ll.longitude]));

    let coords = [outerCoords].concat(holesCoords);
    polygonWrapper.polygon.setLatLngs(coords);
}

export function GetLatLngs(polygonId) {
    let polygonWrapper = findPolygonWrapper(polygonId);
    polygonWrapper.log("getLatLngs >>");

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
    polygonWrapper.log("getBounds >>");

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

export function SetStyle(polygonId, style) {
    let polygonWrapper = findPolygonWrapper(polygonId);
    polygonWrapper.log(`setStyle >> style ${JSON.stringify(style)}`);

    polygonWrapper.polygon.setStyle(style);
    polygonWrapper.polygon.redraw();
}

export function AddClass(polygonId, classNames) {
    let polygonWrapper = findPolygonWrapper(polygonId);
    polygonWrapper.log(`addClass >> className ${JSON.stringify(classNames)}`);

    let element = polygonWrapper.polygon._path;
    let classes = classNames
        .split(" ")
        .filter(s => s.trim().length > 0);

    for (var i = 0; i < classes.length; i++) {
        let cs = classes[i];
        element.classList.add(cs);
    }
}

export function RemoveClass(polygonId, classNames) {
    let polygonWrapper = findPolygonWrapper(polygonId);
    polygonWrapper.log(`removeClass >> className ${JSON.stringify(classNames)}`);

    let element = polygonWrapper.polygon._path;
    let classes = classNames
        .split(" ")
        .filter(s => s.trim().length > 0);

    for (var i = 0; i < classes.length; i++) {
        let cs = classes[i];
        element.classList.remove(cs);
    }
}

// -- private
function createPolygonWrapper(polygonId, polygon, map, netRef, enableLogging) {

    let wrapper = {
        polygonId: polygonId,
        parentMap: map,
        polygon: polygon, // polygon instance
        ref: netRef,      // net object reference
        refId: netRef._id,

        invokeRef: function (...args) {
            try {
                wrapper.log(`${args[0]}`);
                wrapper.ref.invokeMethodAsync(...args);
            }
            catch (e) {
                console.log("error in polygon wrapper");
                console.log(e);
            }
        },

        disconnect: function () {
            //-- mouse events
            polygon.off("click", wrapper._onClick);
            polygon.off("dblclick", wrapper._onDblClick);
            polygon.off("mousedown", wrapper._onMouseDown);
            polygon.off("mouseup", wrapper._onMouseUp);
            polygon.off("mouseover", wrapper._onMouseOver);
            polygon.off("mouseout", wrapper._onMouseOut);
        },

        log: function (m) {
            if (!enableLogging) {
                return;
            }

            console.log(`[Polygon ${wrapper.polygonId}:${wrapper.refId}]: ${m}`);
        },

        _onClick: function (e) {
            wrapper.invokeRef("OnMouseClick", { latitude: e.latlng.lat, longitude: e.latlng.lng });
        },

        _onDblClick: function (e) {
            wrapper.invokeRef("OnMouseDoubleClick", { latitude: e.latlng.lat, longitude: e.latlng.lng });
        },

        _onMouseDown: function (e) {
            wrapper.invokeRef("OnMouseDown", { latitude: e.latlng.lat, longitude: e.latlng.lng });
        },

        _onMouseUp: function (e) {
            wrapper.invokeRef("OnMouseUp", { latitude: e.latlng.lat, longitude: e.latlng.lng });
        },

        _onMouseOver: function (e) {
            wrapper.invokeRef("OnMouseEnter", { latitude: e.latlng.lat, longitude: e.latlng.lng });
        },

        _onMouseOut: function (e) {
            wrapper.invokeRef("OnMouseLeave", { latitude: e.latlng.lat, longitude: e.latlng.lng });
        }
    };

    //-- mouse events
    polygon.on("click", wrapper._onClick);
    polygon.on("dblclick", wrapper._onDblClick);
    polygon.on("mousedown", wrapper._onMouseDown);
    polygon.on("mouseup", wrapper._onMouseUp);
    polygon.on("mouseover", wrapper._onMouseOver);
    polygon.on("mouseout", wrapper._onMouseOut);

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