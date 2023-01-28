var mapWrappers = [];

console.log("osm maps_0.5.0.js loaded");

//--Maps-----------------------------------------
export function InitializeMapOnElement(mapId, options, hostElement, netRef) {
    let wrapper = findMapWrapper(mapId);
    if (wrapper !== null) {
        wrapper.ref = netObjRef;
        return;
    }

    let map = createMapOnElement(options, hostElement);

    wrapper = createMapWrapper(mapId, map, netRef, options.traceJs);
    wrapper.log("initialized");

    mapWrappers.push(wrapper);
}

export function Remove(mapId) {
    let i = mapWrappers.findIndex(me => me.mapId == mapId);
    let wrapper = mapWrappers.splice(i, 1);
    wrapper[0].map.remove();
    wrapper[0].log("removed");
}

export function PanTo(mapId, center) {
    let wrapper = findMapWrapper(mapId);
    wrapper.log(`panTo ${center.latitude} ${center.longitude}`);
    wrapper.map.panTo([center.latitude, center.longitude]);
}

export function ZoomTo(mapId, zoom) {
    let wrapper = findMapWrapper(mapId);
    wrapper.log(`zoomTo ${zoom}`);
    wrapper.map.setZoom(zoom);
}

export function SetCenter(mapId, center) {
    let wrapper = findMapWrapper(mapId);
    wrapper.log(`set center ${center.latitude} ${center.longitude}`);
    wrapper.map.setView([center.latitude, center.longitude]);
}

export function GetCenter(mapId) {
    let wrapper = findMapWrapper(mapId);
    let center = wrapper.map.getCenter();
    wrapper.log(`get center ${center.lat} ${center.lng}`);
    return { latitude: center.lat, longitude: center.lng };
;
}

export function FitBounds(mapId, bounds, padding, zoom) {
    let wrapper = findMapWrapper(mapId);
    wrapper.log(`fitBounds SW: ${bounds.southWest.latitude} ${bounds.southWest.longitude} NE: ${bounds.northEast.latitude} ${bounds.northEast.longitude}`);
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

export function GetBounds(mapId) {
    let wrapper = findMapWrapper(mapId);
    let bounds = wrapper.map.getBounds();
    wrapper.log(`getBounds sw: ${bounds.getSouth()} ${bounds.getWest()}, ne: ${bounds.getNorth()} ${bounds.getEast()}`);

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

export function findMapWrapper(mapId) {
    let i = mapWrappers.findIndex(me => me.mapId == mapId);
    return i === -1
        ? null
        : mapWrappers[i];
}

function createMapOnElement(options, hostElement) {
    let map = L.map(hostElement,
        {
            center: [options.center.latitude, options.center.longitude],
            zoom: options.zoom,
            dragging: options.draggable
        });

    L.tileLayer('https://{s}.tile.osm.org/{z}/{x}/{y}.png', {
        attribution: '&copy; <a href="https://osm.org/copyright">OpenStreetMap</a> contributors'
    }).addTo(map);

    return map;
}

function createMapWrapper(mapId, map, netRef, enableLogging) {
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
        },

        log: function (m) {
            if (!enableLogging) {
                return;
            }
            console.log(`[Map ${mapId}]`);
            console.log(m);
        }
    };


    //-- mouse events
    map.on("click", (e) => {
        wrapper.invokeRef("OnMouseClick", { latitude: e.latlng.lat, longitude: e.latlng.lng })
    });

    map.on("dblclick", (e) => {
        wrapper.invokeRef("OnMouseDoubleClick", { latitude: e.latlng.lat, longitude: e.latlng.lng })
    });

    map.on("mousedown", (e) => {
        wrapper.invokeRef("OnMouseDown", { latitude: e.latlng.lat, longitude: e.latlng.lng })
    });

    map.on("mouseup", (e) => {
        wrapper.invokeRef("OnMouseUp", { latitude: e.latlng.lat, longitude: e.latlng.lng })
    });

    map.on("mouseover", (e) => {
        wrapper.invokeRef("OnMouseEnter", { latitude: e.latlng.lat, longitude: e.latlng.lng })
    });

    map.on("mousemove", (e) => {
        wrapper.invokeRef("OnMouseMove", { latitude: e.latlng.lat, longitude: e.latlng.lng })
    });

    map.on("mouseout", (e) => {
        wrapper.invokeRef("OnMouseLeave", { latitude: e.latlng.lat, longitude: e.latlng.lng })
    });
    //------------------------

    //--map events------------
    map.on("resize", (e) => {
        wrapper.invokeRef("OnResized", e.newSize);
    });

    map.on("move", () => {
        let center = map.getCenter();
        wrapper.invokeRef("OnCenterChanging", { latitude: center.lat, longitude: center.lng })
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
