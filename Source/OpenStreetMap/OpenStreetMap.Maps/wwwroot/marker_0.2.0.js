import { findMapWrapper } from './maps_0.2.0.js';

console.log("osm marker_0.2.0.js loaded");

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
    let i = markerWrappers.findIndex(me => me.markerid = markerId);
    let wrapper = markerWrappers.splice(i, 1);
    wrapper[0].log("removing from map");
    wrapper[0].marker.remove();
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
    wrapper.marker.setLatLng([position.latitude, position.longitude]);
}

export function SetMarkerOpacity(markerId, opacity) {
    let wrapper = findMarkerWrapper(markerId);
    wrapper.log(`setting opacity ${opacity}`);
    wrapper.marker.setOpacity(opacity);
}

export function SetMarkerImageIcon(markerId, icon) {
    console.log("SetMarkerImageIcon");
}

export function SetMarkerHtmlIcon(markerId, icon) {
    console.log("SetMarkerHtmlIcon: " + markerId);
    console.log(icon);

    let wrapper = findMarkerWrapper(markerId);
    let i = L.divIcon({
        html: icon.html,
        className: icon.className,
    });

    if (!icon.size.isZero) {
        console.log("icon size not zero");
        i.options.iconSize = [icon.size.width, icon.size.height]
    }

    wrapper.marker.setIcon(i);
}

function setIcon(marker, iconOptions) {
    console.log(iconOptions);

    let i = marker.getIcon();
    console.log(i);

    switch (iconOptions.discriminator) {
        case "HtmlIcon":
            i = createDivIcon(iconOptions);
            break;
        case "ImageIcon":
            i = createPngIcon(iconOptions);
            break;
    }

    console.log(i);
    marker.setIcon(i);
}

function findMarkerWrapper(markerId) {
    let i = markerWrappers.findIndex(me => me.markerId === markerId);
    return i === -1
        ? null
        : markerWrappers[i];
}

function createMarkerWrapper(markerId, marker, map, netRef, enableLogging) {

    let wrapper = {
        markerId: markerId,
        parentMap: map,
        marker: marker, // marker instance
        ref: netRef,    // net object reference

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
            console.log("[Marker " + markerId + "]:");
            console.log(m)
        }
    };

    //-- mouse events
    marker.on("click", (e) => {
        wrapper.invokeRef("OnMouseClick", { latitude: e.latlng.lat, longitude: e.latlng.lng })
    });

    marker.on("dblclick", (e) => {
        wrapper.invokeRef("OnMouseDoubleClick", { latitude: e.latlng.lat, longitude: e.latlng.lng })
    });

    marker.on("mousedown", (e) => {
        wrapper.invokeRef("OnMouseDown", { latitude: e.latlng.lat, longitude: e.latlng.lng })
    });

    marker.on("mouseup", (e) => {
        wrapper.invokeRef("OnMouseUp", { latitude: e.latlng.lat, longitude: e.latlng.lng })
    });

    marker.on("mouseover", (e) => {
        wrapper.invokeRef("OnMouseEnter", { latitude: e.latlng.lat, longitude: e.latlng.lng })
    });

    marker.on("mouseout", (e) => {
        wrapper.invokeRef("OnMouseLeave", { latitude: e.latlng.lat, longitude: e.latlng.lng })
    });
    //------------------------

    marker.on("movestart", (e) => {
        let position = marker.getLatLng();
        wrapper.invokeRef("OnPositionStartChange", { latitude: position.lat, longitude: position.lng });
    });

    marker.on("move", (e) => {
        wrapper.invokeRef("OnPositionChanging", { latitude: e.latlng.lat, longitude: e.latlng.lng });
    });

    marker.on("moveend", (e) => {
        let position = marker.getLatLng();
        wrapper.invokeRef("OnPositionChanged", { latitude: position.lat, longitude: position.lng });
    });

    //marker.on("dragstart"), (e) => {
    //    // console.log(e);
    //    // let position = marker.getLatLng();
    //    // wrapper.invokeRef("OnDragStart", { latitude: position.lat, longitude: position.lng });
    //}

    //marker.on("drag"), (e) => {
    //    console.log(e);
    //    // let position = marker.getLatLng();
    //    // wrapper.invokeRef("OnDragStart", { latitude: position.lat, longitude: position.lng });
    //}

    //marker.on("dragend"), (e) => {
    //    console.log(e);
    //    let position = marker.getLatLng();
    //    // wrapper.invokeRef("OnDragStart", { latitude: position.lat, longitude: position.lng });
    //}

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