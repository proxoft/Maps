import { findMapWrapper } from './map.{version}.js';

console.log("osm circle.{version}.js loaded");

var circleWrappers = [];

// --exports----------------------------------
export function AddCircle(circleId, options, mapId, netRef) {
    if (options.traceJs) {
        console.log(`addCircle >> circleId ${circleId }, options: ${ JSON.stringify(options) }, mapId ${ mapId }`)
    }

    let mapWrapper = findMapWrapper(mapId);
    let latLng = [options.latLng.latitude, options.latLng.longitude];
    console.log(latLng);
    let circle = L.circleMarker(latLng, options.radius);
    circle.addTo(mapWrapper.map);

    let circleWrapper = createCircleWrapper(circleId, circle, mapWrapper.map, netRef, options.traceJs);
    circleWrappers.push(circleWrapper);

    circleWrapper.log(`addCircle >>>> Added to the map ${mapId}`);
}

export function RemoveCircle(circleId) {
    let i = findCircleWrapperIndex(circleId);
    let wrapper = circleWrappers.splice(i, 1);
    wrapper[0].log("removeCircle >>");
    wrapper[0].disconnect();
    wrapper[0].circle.remove();
    wrapper[0].log("removeCircle >>>> removed from map");
}

export function SetLatLng(circleId, latlng) {
    let circleWrapper = findCircleWrapper(circleId);
    circleWrapper.log(`setLatLngs >> latLngs ${JSON.stringify(latlng)}`);
    circleWrapper.circle.setLatLng([latLngs.latitude, latlng.longitude]);
}

export function GetLatLng(circleId) {
    let circleWrapper = findCircleWrapper(circleId);
    circleWrapper.log("getLatLngs >>");

    let latLngs = circleWrapper.circle.getLatLng();

    return { latitude: latLngs[0], longitude: latLngs[1] };
}

export function SetRadius(circleId, radius) {
    let circleWrapper = findCircleWrapper(circleId);
    circleWrapper.log(`setRadiu >> ${radius}`);
    circleWrapper.circle.setRadius(radius);
}

export function GetRadius(circleId) {
    let circleWrapper = findCircleWrapper(circleId);
    circleWrapper.log(`getRadius >>`);
    let radius = circleWrapper.circle.getRadius(radius);
    return radius;
}

export function SetStyle(circleId, style) {
    let circleWrapper = findCircleWrapper(circleId);
    circleWrapper.log(`setStyle >> style ${JSON.stringify(style)}`);

    circleWrapper.circle.setStyle(style);
    circleWrapper.circle.redraw();
}

export function AddClass(circleId, classNames) {
    let circleWrapper = findCircleWrapper(circleId);
    circleWrapper.log(`addClass >> className ${JSON.stringify(classNames)}`);

    let element = circleWrapper.circle._path;
    let classes = classNames
        .split(" ")
        .filter(s => s.trim().length > 0);

    for (var i = 0; i < classes.length; i++) {
        let cs = classes[i];
        element.classList.add(cs);
    }
}

export function RemoveClass(circleId, classNames) {
    let circleWrapper = findCircleWrapper(circleId);
    circleWrapper.log(`removeClass >> className ${JSON.stringify(classNames)}`);

    let element = circleWrapper.circle._path;
    let classes = classNames
        .split(" ")
        .filter(s => s.trim().length > 0);

    for (var i = 0; i < classes.length; i++) {
        let cs = classes[i];
        element.classList.remove(cs);
    }
}

// -- private
function createCircleWrapper(circleId, circle, map, netRef, enableLogging) {

    let wrapper = {
        circleId: circleId,
        parentMap: map,
        circle: circle, // circle instance
        ref: netRef,      // net object reference
        refId: netRef._id,

        invokeRef: function (...args) {
            try {
                wrapper.log(`${args[0]}`);
                wrapper.ref.invokeMethodAsync(...args);
            }
            catch (e) {
                console.log(`error in circle wrapper: ${wrapper.circleId}`);
                console.log(e);
            }
        },

        disconnect: function () {
            //-- mouse events
            circle.off("click", wrapper._onClick);
            circle.off("dblclick", wrapper._onDblClick);
            circle.off("mousedown", wrapper._onMouseDown);
            circle.off("mouseup", wrapper._onMouseUp);
            circle.off("mouseover", wrapper._onMouseOver);
            circle.off("mouseout", wrapper._onMouseOut);
        },

        log: function (m) {
            if (!enableLogging) {
                return;
            }

            console.log(`[circle ${wrapper.circleId}:${wrapper.refId}]: ${m}`);
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
    circle.on("click", wrapper._onClick);
    circle.on("dblclick", wrapper._onDblClick);
    circle.on("mousedown", wrapper._onMouseDown);
    circle.on("mouseup", wrapper._onMouseUp);
    circle.on("mouseover", wrapper._onMouseOver);
    circle.on("mouseout", wrapper._onMouseOut);

    return wrapper;
}

function findCircleWrapper(circleId) {
    let i = findCircleWrapperIndex(circleId);
    return i === -1
        ? null
        : circleWrappers[i];
}

function findCircleWrapperIndex(circleId) {
    let i = circleWrappers.findIndex(me => me.circleId === circleId);
    return i;
}