import { findMapWrapper } from './maps_0.8.0.js';

console.log("osm polyline_0.8.0.js loaded");

var polylineWrappers = [];

// --exports----------------------------------
export function AddPolyline(polylineId, options, mapId, netRef) {
    if (options.traceJs) {
        console.log(`addPolyline >> polylineId ${polylineId }, options: ${ JSON.stringify(options) }, mapId ${ mapId }`)
    }

    let mapWrapper = findMapWrapper(mapId);

    let latLngs = options.lines.map(line => line.map(ll => [ll.latitude, ll.longitude]));
    let polyline = L.polyline(latLngs, options.style);

    polyline.addTo(mapWrapper.map);

    let polylineWrapper = createPolylineWrapper(polylineId, polyline, mapWrapper.map, netRef, options.traceJs);
    polylineWrappers.push(polylineWrapper);

    polylineWrapper.log(`addPolyline >>>> Added to the map ${mapId}`);
}

export function RemovePolyline(polylineId) {
    let i = findPolylineWrapperIndex(polylineId);
    let wrapper = polylineWrappers.splice(i, 1);
    wrapper[0].log("removePolyline >>");
    wrapper[0].disconnect();
    wrapper[0].polyline.remove();
    wrapper[0].log("removePolyline >>>> removed from map");
}

export function SetLatLngs(polylineId, lines) {
    let polylineWrapper = findPolylineWrapper(polylineId);
    polylineWrapper.log(`setLatLngs >> latLngs ${JSON.stringify(latLngs)}`);

    let latLngs = lines.map(line => line.map(ll => [ll.latitude, ll.longitude]));
    polylineWrapper.polyline.setLatLngs(latLngs);
}

export function GetLatLngs(polylineId) {
    let polylineWrapper = findPolylineWrapper(polylineId);
    polylineWrapper.log("getLatLngs >>");

    let latLngs = polylineWrapper.polyline.getLatLngs();

    return latLngs;
}

export function GetBounds(polylineId) {
    let polylineWrapper = findPolylineWrapper(polylineId);
    polylineWrapper.log("getBounds >>");

    let bounds = polylineWrapper.polyline.getBounds();

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

export function SetStyle(polylineId, style) {
    let polylineWrapper = findPolylineWrapper(polylineId);
    polylineWrapper.log(`setStyle >> style ${JSON.stringify(style)}`);

    polylineWrapper.polyline.setStyle(style);
    polylineWrapper.polyline.redraw();
}

// -- private
function createPolylineWrapper(polylineId, polyline, map, netRef, enableLogging) {

    let wrapper = {
        polylineId: polylineId,
        parentMap: map,
        polyline: polyline, // polyline instance
        ref: netRef,      // net object reference
        refId: netRef._id,

        invokeRef: function (...args) {
            try {
                wrapper.log(`${args[0]}`);
                wrapper.ref.invokeMethodAsync(...args);
            }
            catch (e) {
                console.log(`error in polyline wrapper: ${wrapper.polylineId}`);
                console.log(e);
            }
        },

        disconnect: function () {
            //-- mouse events
            polyline.off("click", wrapper._onClick);
            polyline.off("dblclick", wrapper._onDblClick);
            polyline.off("mousedown", wrapper._onMouseDown);
            polyline.off("mouseup", wrapper._onMouseUp);
            polyline.off("mouseover", wrapper._onMouseOver);
            polyline.off("mouseout", wrapper._onMouseOut);
        },

        log: function (m) {
            if (!enableLogging) {
                return;
            }

            console.log(`[Polyline ${wrapper.polylineId}:${wrapper.refId}]: ${m}`);
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
    polyline.on("click", wrapper._onClick);
    polyline.on("dblclick", wrapper._onDblClick);
    polyline.on("mousedown", wrapper._onMouseDown);
    polyline.on("mouseup", wrapper._onMouseUp);
    polyline.on("mouseover", wrapper._onMouseOver);
    polyline.on("mouseout", wrapper._onMouseOut);

    return wrapper;
}

function findPolylineWrapper(polylineId) {
    let i = findPolylineWrapperIndex(polylineId);
    return i === -1
        ? null
        : polylineWrappers[i];
}

function findPolylineWrapperIndex(polylineId) {
    let i = polylineWrappers.findIndex(me => me.polylineId === polylineId);
    return i;
}