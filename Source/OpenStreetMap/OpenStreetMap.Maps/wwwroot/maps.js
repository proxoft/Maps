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
    let wrapper = mapWrappers.splice(i, 1);
    wrapper[0].map.remove();
}

export function PanTo(mapId, center) {
    let wrapper = findMapWrapper(mapId);
    wrapper.map.panTo([center.latitude, center.longitude]);
}

export function ZoomTo(mapId, zoom) {
    let wrapper = findMapWrapper(mapId);
    wrapper.map.setZoom(zoom);
}

export function SetCenter(mapId, center) {
    let wrapper = findMapWrapper(mapId);
    wrapper.map.setView([center.latitude, center.longitude]);
}

export function FitBounds(mapId, bounds, padding, zoom) {
    let wrapper = findMapWrapper(mapId);
    wrapper.map.fitBounds(
        [
            [bounds.southWest.latitude, bounds.southWest.longitude],
            [bounds.northEast.latitude, bounds.northEast.longitude]
        ],
        [
            [padding.top, padding.left]
            [padding.bottom, padding.right]
        ],
        zoom || null);
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

    let marker = L.marker([options.position.latitude, options.position.longitude], { opacity: options.opacity.value });
    mapWrapper.map.addLayer(marker);

    if (options.draggable) {
        marker.dragging.enable();
    }

    let markerWrapper = createMarkerWrapper(markerId, marker, mapWrapper.map, netRef);
    markerWrappers.push(markerWrapper);
}

export function RemoveMarker(markerId) {
    let i = markerWrappers.findIndex(me => me.markerid = markerId);
    let wrapper = mapWrappers.splice(i, 1);
    wrapper.marker.remove();
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

export function SetMarkerOpacity(markerId, opacity) {
    let wrapper = findMarkerWrapper(markerId);

    wrapper.marker.setOpacity(opacity);
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
        },

        invokeRef: function (...args) {
            try {
                wrapper.ref.invokeMethodAsync(...args);
            }
            catch(e)
            {
                console.log(e);
            }
        }
    };


    //-- mouse events
    map.on("click", (e) => {
        wrapper.invokeRef("OnMouseClick", { latitude: e.latlng.lat, longitude: e.latlng.lat })
    });

    map.on("dblclick", (e) => {
        wrapper.invokeRef("OnMouseDoubleClick", { latitude: e.latlng.lat, longitude: e.latlng.lat })
    });

    map.on("mousedown", (e) => {
        wrapper.invokeRef("OnMouseDown", { latitude: e.latlng.lat, longitude: e.latlng.lat })
    });

    map.on("mouseup", (e) => {
        wrapper.invokeRef("OnMouseUp", { latitude: e.latlng.lat, longitude: e.latlng.lat })
    });

    map.on("mouseover", (e) => {
        wrapper.invokeRef("OnMouseEnter", { latitude: e.latlng.lat, longitude: e.latlng.lat })
    });

    map.on("mousemove", (e) => {
        wrapper.invokeRef("OnMouseMove", { latitude: e.latlng.lat, longitude: e.latlng.lat })
    });

    map.on("mouseout", (e) => {
        wrapper.invokeRef("OnMouseLeave", { latitude: e.latlng.lat, longitude: e.latlng.lat })
    });
    //------------------------

    //--map events------------
    map.on("resize", (e) => {
        wrapper.invokeRef("OnResized", e.newSize);
    });

    map.on("move", () => {
        let center = map.getCenter();
        wrapper.invokeRef("OnCenterChanging", { latitude: center.lat, longitude: center.lng } )
    });

    map.on("moveend", () => {
        let center = map.getCenter();
        wrapper.invokeRef("OnCenterChanged", { latitude: center.lat, longitude: center.lng });
    });

    map.on("zoom", () => {
        let zoom = map.getZoom();
        wrapper.invokeRef("OnZoomChanging", zoom);
    });

    map.on("zoomend", () => {
        let zoom = map.getZoom();
        wrapper.invokeRef("OnZoomChanged", zoom);
    });
    //------------------------

    return wrapper;
}

function createMarkerWrapper(markerId, marker, map, netRef) {

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
        }
    };

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