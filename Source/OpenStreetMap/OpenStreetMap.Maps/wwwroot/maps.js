var mapWrappers = [];
var markerWrappers = [];

//--Maps-----------------------------------------
export function InitializeMapOnElement(mapId, options, hostElement, netRef) {
    let wrapper = findMapWrapper(mapId);
    if (wrapper !== null) {
        wrapper.ref = netObjRef;
        return;
    }

    let map = createMapOnElement(options, hostElement);

    wrapper = createMapWrapper(mapId, map, netRef);

    mapWrappers.push(wrapper);
}

export function Remove(mapId) {
    let i = mapWrappers.findIndex(me => me.mapId == mapId);
    mapWrappers.splice(i, 1);
}

export function PanTo(mapId, center) {
    let wrapper = findMapWrapper(mapId);
    wrapper.map.panTo([center.latitude, center.longitude]);
}

export function ZoomTo(mapId, zoom) {
    let wrapper = findMapWrapper(mapId);
    wrapper.map.setZoom(zoom);
}

function findMapWrapper(mapId) {
    let i = mapWrappers.findIndex(me => me.mapId == mapId);
    return i === -1
        ? null
        : mapWrappers[i];
}

function createMapOnElement(options, hostElement) {
    let map = L.map(hostElement)
        .setView([options.center.latitude, options.center.longitude], options.zoom);

    L.tileLayer('https://{s}.tile.osm.org/{z}/{x}/{y}.png', {
        attribution: '&copy; <a href="https://osm.org/copyright">OpenStreetMap</a> contributors'
    }).addTo(map);

    return map;
}

//-----------------------------------------------

//--Markers--------------------------------------

export function CreateMarker(markerId, options, mapId, netRef) {
    let mapWrapper = findMapWrapper(mapId);

    let marker = L.marker([options.position.latitude, options.position.longitude]);
    mapWrapper.map.addLayer(marker);

    if (options.draggable) {
        marker.dragging.enable();
    }

    let markerWrapper = createMarkerWrapper(markerId, marker, mapWrapper.map, netRef);
    markerWrappers.push(markerWrapper);
}

export function SetMarkerDraggable(markerId, draggable) {
    let wrapper = findMarkerWrapper(markerId);
    if (draggable) {
        console.log("enable dragging");
        wrapper.marker.dragging.enable();
    } else {
        console.log("disable dragging");
        wrapper.marker.dragging.disable();
    }
    
}

export function SetMarkerPosition(markerId, position) {
    let wrapper = findMarkerWrapper(markerId);

    wrapper.marker.setLatLng([position.latitude, position.longitude]);
}

function findMarkerWrapper(markerId) {
    let i = markerWrappers.findIndex(me => me.markerid = markerId);
    return i === -1
        ? null
        : markerWrappers[i];
}

//-----------------------------------------------

function createMapWrapper(mapId, map, netRef) {
    let wrapper = {
        mapId: mapId,
        map: map,    // map instance
        ref: netRef, // net object reference

        addMarker: function () {
            return 1;
        }
    };

    map.on("moveend", () => {
        let center = map.getCenter();
        wrapper.ref.invokeMethodAsync("OnCenterChanged", { latitude: center.lat, longitude: center.lng });
    });

    map.on("zoomend", () => {
        let zoom = map.getZoom();
        wrapper.ref.invokeMethodAsync("OnZoomChanged", zoom);
    });

    return wrapper;
}

function createMarkerWrapper(markerId, marker, map, netRef) {

    let wrapper = {
        markerId: markerId,
        parentMap: map,
        marker: marker, // marker instance
        ref: netRef,    // net object reference
    };

    marker.on("moveend", () => {
        let position = marker.getLatLng();
        wrapper.ref.invokeMethodAsync("OnPositionChanged", { latitude: position.lat, longitude: position.lng });
    });

    return wrapper;
}