import { findMapWrapper } from './maps_0.7.0.js';

console.log("osm marker_0.7.0.js loaded");

var markerWrappers = [];

//--Markers--------------------------------------

export function AddMarker(markerId, options, iconOptions, mapId, netRef) {
    let mapWrapper = findMapWrapper(mapId);
    let marker = L.marker([options.position.latitude, options.position.longitude], {
        draggable: options.draggable,
        opacity: options.opacity.value,
    });

    setIcon(marker, iconOptions);

    mapWrapper.map.addLayer(marker);

    let markerWrapper = createMarkerWrapper(markerId, marker, mapWrapper.map, netRef, options.traceJs);
    markerWrappers.push(markerWrapper);

    markerWrapper.log(`Added to the map ${mapId}`);
}

export function RemoveMarker(markerId) {
    let i = findMarkerWrapperIndex(markerId);
    let wrapper = markerWrappers.splice(i, 1);
    wrapper[0].log("removing from map");
    wrapper[0].disconnect();
    wrapper[0].marker.remove();
    wrapper[0].log("removed from map");
}

export function SetMarkerDraggable(markerId, draggable) {
    let wrapper = findMarkerWrapper(markerId);
    if (draggable) {
        wrapper.log("set dragging enabled");
        wrapper.marker.dragging.enable();
    } else {
        wrapper.log("set dragging disabled");
        wrapper.marker.dragging.disable();
    }
}

export function SetMarkerPosition(markerId, position) {
    let wrapper = findMarkerWrapper(markerId);
    wrapper.log(`setting position ${position.latitude} : ${position.longitude}`);
    wrapper.settingLatLng = true;
    wrapper.marker.setLatLng([position.latitude, position.longitude]);
}

export function SetMarkerOpacity(markerId, opacity) {
    let wrapper = findMarkerWrapper(markerId);
    wrapper.log(`setting opacity ${opacity}`);
    wrapper.marker.setOpacity(opacity);
}

export function SetMarkerIcon(markerId, iconOptions) {
    let wrapper = findMarkerWrapper(markerId);
    wrapper.log("changing icon");
    wrapper.log(iconOptions);
    setIcon(wrapper.marker, iconOptions);
}

function setIcon(marker, iconOptions) {
    let i = marker.getIcon();

    switch (iconOptions.discriminator) {
        case "HtmlIcon":
            i = createDivIcon(iconOptions);
            break;
        case "ImageIcon":
            i = createPngIcon(iconOptions);
            break;
    }

    marker.setIcon(i);
}

function findMarkerWrapper(markerId) {
    let i = findMarkerWrapperIndex(markerId);
    return i === -1
        ? null
        : markerWrappers[i];
}

function findMarkerWrapperIndex(markerId) {
    let i = markerWrappers.findIndex(me => me.markerId === markerId);
    return i;
}

function createMarkerWrapper(markerId, marker, map, netRef, enableLogging) {

    let wrapper = {
        markerId: markerId,
        parentMap: map,
        marker: marker, // marker instance
        ref: netRef,    // net object reference
        refId: netRef._id,
        settingLatLng: false,

        invokeRef: function (...args) {
            try {
                wrapper.log(`invoking ${args[0]}`);
                wrapper.ref.invokeMethodAsync(...args);
            }
            catch (e) {
                console.log("error in marker wrapper");
                console.log(e);
            }
        },

        disconnect: function () {
            wrapper.log("diconnecting marker events");

            //-- mouse events
            marker.off("click", wrapper._onClick);
            marker.off("dblclick", wrapper._onDblClick);
            marker.off("mousedown", wrapper._onMouseDown);
            marker.off("mouseup", wrapper._onMouseUp);
            marker.off("mouseover", wrapper._onMouseOver);
            marker.off("mouseout", wrapper._onMouseOut);

            //-- position changed
            marker.off("move", wrapper._onSetLatLngMove);

            //-- drag events
            marker.off("dragstart", wrapper._onDragStart);
            marker.off("movestart", wrapper._onDraggingMoveStart);
            marker.off("move", wrapper._onDraggingMove);
            marker.off("moveend", wrapper._onDraggingMoveEnd);
            marker.off("dragend", wrapper._onDragEnd);

            wrapper.log("diconnected marker events");
        },

        log: function (m) {
            if (!enableLogging) {
                return;
            }

            console.log(`[Marker ${wrapper.markerId}:${wrapper.refId}]: ${m}`);
        },

        // -- mouse events
        _onClick: function (e) {
            wrapper.invokeRef("OnMouseClick", { latitude: e.latlng.lat, longitude: e.latlng.lng })
        },

       _onDblClick: function (e) {
            wrapper.invokeRef("OnMouseDoubleClick", { latitude: e.latlng.lat, longitude: e.latlng.lng });
        },

       _onMouseDown: function (e) {
            wrapper.invokeRef("OnMouseDown", { latitude: e.latlng.lat, longitude: e.latlng.lng })
        },

       _onMouseUp: function(e) {
            wrapper.invokeRef("OnMouseUp", { latitude: e.latlng.lat, longitude: e.latlng.lng });
        },

       _onMouseOver: function (e) {
            wrapper.invokeRef("OnMouseEnter", { latitude: e.latlng.lat, longitude: e.latlng.lng });
        },

       _onMouseOut: function (e) {
            wrapper.invokeRef("OnMouseLeave", { latitude: e.latlng.lat, longitude: e.latlng.lng });
        },

        // -- position changed
       _onSetLatLngMove: function (e) {
            if (wrapper.settingLatLng === false) {
                return;
            }

            wrapper.invokeRef("OnPositionChanged", { latitude: e.latlng.lat, longitude: e.latlng.lng });
        },

        // -- drag events
       _onDragStart: function (e) {
            let position = e.target.getLatLng();
            wrapper.invokeRef("OnDrag", { latitude: position.lat, longitude: position.lng });
        },

       _onDraggingMoveStart: function (e) {
            let position = e.target.getLatLng();
            wrapper.invokeRef("OnDraggingStarted", { latitude: position.lat, longitude: position.lng });
        },

       _onDraggingMove: function (e) {
            let ignore = wrapper.settingLatLng;
            wrapper.settingLatLng = false;

            if (ignore) {
                return;
            }

            wrapper.invokeRef("OnDragging", { latitude: e.latlng.lat, longitude: e.latlng.lng });
        },

       _onDraggingMoveEnd: function (e) {
            let position = e.target.getLatLng();
            wrapper.invokeRef("OnDraggingEnd", { latitude: position.lat, longitude: position.lng });
        },

       _onDragEnd: function (e) {
            let position = e.target.getLatLng();
            wrapper.invokeRef("OnDrop", { latitude: position.lat, longitude: position.lng });
        }
    };

    //-- mouse events
    marker.on("click", wrapper._onClick);
    marker.on("dblclick", wrapper._onDblClick);
    marker.on("mousedown", wrapper._onMouseDown);
    marker.on("mouseup", wrapper._onMouseUp);
    marker.on("mouseover", wrapper._onMouseOver);
    marker.on("mouseout", wrapper._onMouseOut);

    //-- position changed
    marker.on("move", wrapper._onSetLatLngMove);

    //-- drag events
    marker.on("dragstart", wrapper._onDragStart);
    marker.on("movestart", wrapper._onDraggingMoveStart);
    marker.on("move", wrapper._onDraggingMove);
    marker.on("moveend", wrapper._onDraggingMoveEnd);
    marker.on("dragend", wrapper._onDragEnd);

    return wrapper;
}

function createDivIcon(iconOptions) {
    let options = {
        html: iconOptions.html,
        className: iconOptions.className,
        iconSize: [iconOptions.size.width, iconOptions.size.height],
        iconAnchor: [iconOptions.iconAnchor.x, iconOptions.iconAnchor.y]
    };

    return L.divIcon(options);
}

function createPngIcon(iconOptions) {
    let options = {

        iconUrl: iconOptions.url || "_content/Proxoft.Maps.OpenStreetMap.Maps/marker-icon.png",
        retinaUrl: iconOptions.retinaUrl || "_content/Proxoft.Maps.OpenStreetMap.Maps/marker-icon-2x.png",
        iconSize: [iconOptions.size.width, iconOptions.size.height],
        iconAnchor: [iconOptions.iconAnchor.x, iconOptions.iconAnchor.y],
        shadowUrl: iconOptions.shadowUrl || "_content/Proxoft.Maps.OpenStreetMap.Maps/marker-shadow.png",
        shadowSize: [iconOptions.shadowSize.width, iconOptions.shadowSize.height],
        shadowAnchor: [iconOptions.shadowAnchor.x, iconOptions.shadowAnchor.y]
    };

    return L.icon(options);
}