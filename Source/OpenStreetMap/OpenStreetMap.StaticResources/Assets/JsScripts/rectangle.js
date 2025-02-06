import { findMapWrapper } from './map.{version}.js';

console.log("osm rectangle.{version}.js loaded");

var rectangleWrappers = [];

// --exports----------------------------------
export function AddRectangle(rectangleId, options, mapId, netRef) {
    if (options.traceJs) {
        console.log(`addRectangle >> rectangleId ${rectangleId}, options: ${JSON.stringify(options)}, mapId ${mapId}`)
    }

    let mapWrapper = findMapWrapper(mapId);

    let latLngBounds = toLatLngBounds(options.bounds);
    let rectangle = L.rectangle(latLngBounds, options.style);

    rectangle.addTo(mapWrapper.map);

    let rectangleWrapper = createRectangleWrapper(rectangleId, rectangle, mapWrapper.map, netRef, options.traceJs);
    rectangleWrappers.push(rectangleWrapper);

    rectangleWrapper.log(`addRectangle >>>> Added to the map ${mapId}`);
}

export function RemoveRectangle(rectangleId) {
    let i = findRectangleWrapperIndex(rectangleId);
    let wrapper = rectangleWrappers.splice(i, 1);
    wrapper[0].log("removeRectangle >>");
    wrapper[0].disconnect();
    wrapper[0].rectangle.remove();
    wrapper[0].log("removeRectangle >>>> removed from map");
}

export function SetBounds(rectangleId, bounds) {
    let rectangleWrapper = findRectangleWrapper(rectangleId);
    rectangleWrapper.log(`setBounds >> ${JSON.stringify(bounds)}`);

    let latLngBounds = toLatLngBounds(options.bounds);
    rectangleWrapper.rectangle.setBounds(latLngBounds);
}

export function GetBounds(rectangleId) {
    let rectangleWrapper = findRectangleWrapper(rectangleId);
    rectangleWrapper.log("getBounds >>");

    let bounds = rectangleWrapper.rectangle.getBounds();

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

export function SetStyle(rectangleId, style) {
    let rectangleWrapper = findRectangleWrapper(rectangleId);
    rectangleWrapper.log(`setStyle >> style ${JSON.stringify(style)}`);

    rectangleWrapper.rectangle.setStyle(style);
    rectangleWrapper.rectangle.redraw();
}

export function AddClass(rectangleId, classNames) {
    let rectangleWrapper = findRectangleWrapper(rectangleId);
    rectangleWrapper.log(`addClass >> className ${JSON.stringify(classNames)}`);

    let element = rectangleWrapper.rectangle._path;
    let classes = classNames
        .split(" ")
        .filter(s => s.trim().length > 0);

    for (var i = 0; i < classes.length; i++) {
        let cs = classes[i];
        element.classList.add(cs);
    }
}

export function RemoveClass(rectangleId, classNames) {
    let rectangleWrapper = findRectangleWrapper(rectangleId);
    rectangleWrapper.log(`removeClass >> className ${JSON.stringify(classNames)}`);

    let element = rectangleWrapper.rectangle._path;
    let classes = classNames
        .split(" ")
        .filter(s => s.trim().length > 0);

    for (var i = 0; i < classes.length; i++) {
        let cs = classes[i];
        element.classList.remove(cs);
    }
}

// -- private
function createRectangleWrapper(rectangleId, rectangle, map, netRef, enableLogging) {

    let wrapper = {
        rectangleId: rectangleId,
        parentMap: map,
        rectangle: rectangle, // rectangle instance
        ref: netRef,      // net object reference
        refId: netRef._id,

        invokeRef: function (...args) {
            try {
                wrapper.log(`${args[0]}`);
                wrapper.ref.invokeMethodAsync(...args);
            }
            catch (e) {
                console.log(`error in rectangle wrapper: ${wrapper.polylineId}`);
                console.log(e);
            }
        },

        disconnect: function () {
            //-- mouse events
            rectangle.off();
        },

        log: function (data) {
            if (!enableLogging) {
                return;
            }

            console.log(`[Rectangle ${wrapper.rectangleId}:${wrapper.refId}]: `, data);
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
    rectangle.on("click", wrapper._onClick);
    rectangle.on("dblclick", wrapper._onDblClick);
    rectangle.on("mousedown", wrapper._onMouseDown);
    rectangle.on("mouseup", wrapper._onMouseUp);
    rectangle.on("mouseover", wrapper._onMouseOver);
    rectangle.on("mouseout", wrapper._onMouseOut);

    return wrapper;
}

function findRectangleWrapper(rectangleId) {
    let i = rectangleWrappers(rectangleId);
    return i === -1
        ? null
        : rectangleWrappers[i];
}

function findRectangleWrapperIndex(rectangleId) {
    let i = rectangleWrappers.findIndex(me => me.rectangleId === rectangleId);
    return i;
}

function toLatLngBounds(netBounds) {
    let latLngBounds = L.latLngBounds(
        L.latLng(netBounds.south, netBounds.west),
        L.latLng(netBounds.north, netBounds.east),
    );
    return latLngBounds;
}