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

    rectangleWrapper.setDraggable(options.draggable);

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

    let latLngBounds = toLatLngBounds(bounds);
    rectangleWrapper.rectangle.setBounds(latLngBounds);
}

export function GetBounds(rectangleId) {
    let rectangleWrapper = findRectangleWrapper(rectangleId);
    rectangleWrapper.log("getBounds >>");

    let bounds = rectangleWrapper.rectangle.getBounds();

    return toSwNeCorners(bounds);
}

export function SetRectangleDraggable(rectangleId, draggable) {
    let rectangleWrapper = findRectangleWrapper(rectangleId);
    rectangleWrapper.log("setRectangleDraggable >>");
    rectangleWrapper.setDraggable(draggable);
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
        draggable: false,
        isDragging: false,
        dragLatLngSwDelta: { lat: 0, lng: 0 },
        mapDraggingEnabled: false,
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

        setDraggable: function (draggable) {
            if (wrapper.draggable == draggable) {
                return;
            }

            wrapper.draggable = draggable;
            if (draggable) {
                wrapper.rectangle.on("mousedown", wrapper._mouseDownOnDrag);
                wrapper.rectangle.on("mouseup", wrapper._finishDragging);
                wrapper.rectangle.on("mouseout", wrapper._finishDragging);
            }
            else {
                wrapper.rectangle.off("mousedown", wrapper._mouseDownOnDrag);
                wrapper.rectangle.off("mouseup", wrapper._finishDragging);
                wrapper.rectangle.off("mouseout", wrapper._finishDragging);
            }
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
        },

        _mouseDownOnDrag: function (e) {
            let bounds = wrapper.rectangle.getBounds();
            let latDelta = e.latlng.lat - bounds.getSouth();
            let lngDelta = e.latlng.lng - bounds.getWest();
            wrapper.dragLatLngSwDelta = { lat: latDelta, lng: lngDelta };
            wrapper.isDragging = true;
            wrapper.mapDraggingEnabled = wrapper.parentMap.dragging.enabled();

            wrapper.parentMap.dragging.disable();

            wrapper.invokeRef("OnDraggingStarted", toSwNeCorners(bounds));
            wrapper.parentMap.on("mousemove", wrapper._mapMouseMove);
        },

        //_mouseUpOnDrag: function () {
        //    wrapper.parentMap.off("mousemove", wrapper._mapMouseMove);

        //    if (wrapper.isDragging) {
        //        let bounds = wrapper.rectangle.getBounds();
        //        wrapper.isDragging = false;
        //        wrapper.invokeRef("OnDraggingEnded", toSwNeCorners(bounds));
        //    }

        //    if (wrapper.mapDraggingEnabled) {
        //        wrapper.parentMap.dragging.enable();
        //    }
        //},

        //_mouseOutOnDrag: function () {
        //    wrapper._finishDragging()
        //},

        _mapMouseMove: function (e) {
            let bounds = wrapper.rectangle.getBounds();
            let latSize = bounds.getNorth() - bounds.getSouth();
            let lngSize = bounds.getEast() - bounds.getWest();
            let delta = wrapper.dragLatLngSwDelta;

            let sw = L.latLng(e.latlng.lat - delta.lat, e.latlng.lng - delta.lng);
            let ne = L.latLng(sw.lat + latSize, sw.lng + lngSize);

            let newBounds = L.latLngBounds(
                sw,
                ne
            );

            wrapper.rectangle.setBounds(newBounds);
            wrapper.invokeRef("OnDragging", toSwNeCorners(newBounds));
        },

        _finishDragging: function () {
            wrapper.parentMap.off("mousemove", wrapper._mapMouseMove);

            if (wrapper.isDragging) {
                let bounds = wrapper.rectangle.getBounds();
                wrapper.isDragging = false;
                wrapper.invokeRef("OnDraggingEnded", toSwNeCorners(bounds));
            }

            if (wrapper.mapDraggingEnabled) {
                wrapper.parentMap.dragging.enable();
            }
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
    let i = findRectangleWrapperIndex(rectangleId);
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

function toSwNeCorners(bounds) {
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