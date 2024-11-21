var mapWrappers = [];

console.log("osm maps_0.10.0.js loaded");

//--Maps-----------------------------------------
export function InitializeMapOnElement(mapId, options, hostElement, netRef) {
    if (options.traceJs) {
        console.log(`InitializeMapOnElement >> mapId ${mapId}, options ${JSON.stringify(options)}`)
    }

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
    if (i < 0)
    {
        console.log(`no map found for mapId ${mapId}`);
    }

    let wrapper = mapWrappers.splice(i, 1);
    wrapper[0].log("remove >>");
    wrapper[0].disconnect();
    wrapper[0].map.remove();
    wrapper[0].log("removed");
}

export function PanTo(mapId, center) {
    let wrapper = findMapWrapper(mapId);
    wrapper.log(`panTo >> center ${JSON.stringify(center)}`);

    wrapper.map.panTo([center.latitude, center.longitude]);
}

export function ZoomTo(mapId, zoom) {
    let wrapper = findMapWrapper(mapId);
    wrapper.log(`zoomTo >> zoom ${zoom}`);

    wrapper.map.setZoom(zoom);
}

export function SetCenter(mapId, center) {
    let wrapper = findMapWrapper(mapId);
    wrapper.log(`setCenter >> center ${JSON.stringify(center)}`);

    wrapper.map.setView([center.latitude, center.longitude]);
}

export function GetCenter(mapId) {
    let wrapper = findMapWrapper(mapId);
    wrapper.log("getCenter >>");

    let center = wrapper.map.getCenter();
    wrapper.log(`getCenter >>>> ${JSON.stringify(center)}`);
    return { latitude: center.lat, longitude: center.lng };
}

export function FitBounds(mapId, bounds, padding, zoom) {
    let wrapper = findMapWrapper(mapId);
    wrapper.log(`fitBounds >> bounds: ${JSON.stringify(bounds)}, padding: ${JSON.stringify(padding)}, zoom: ${zoom}`);

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
    wrapper.log("getBounds >>");

    let bounds = wrapper.map.getBounds();
    wrapper.log(`getBounds >>>> ${JSON.stringify(bounds)}`);

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
        refId: netRef._id,

        invokeRef: function (...args) {
            try {
                wrapper.log(`invoking ${args[0]}`);
                wrapper.ref.invokeMethodAsync(...args);
            }
            catch(e)
            {
                console.log("error in map wrapper");
                console.log(e);
            }
        },

        disconnect: function () {
            wrapper.log("diconnecting map events");

            map.off("click", wrapper._onMouseClick);
            map.off("dblclick", wrapper._onMouseDblClick);
            map.off("mousedown", wrapper._onMouseDown);
            map.off("mouseup", wrapper._onMouseUp);
            map.off("mouseover", wrapper._onMouseOver);
            map.off("mousemove", wrapper._onMouseMove);
            map.off("mouseout", wrapper._onMouseOut);

            map.off("resize", wrapper._onResize);
            map.off("move", wrapper._onMove);
            map.off("moveend", wrapper._onMoveEnd);
            map.off("zoom", wrapper._onZoom);
            map.off("zoomend", wrapper._onZoomEnd);

            wrapper.log("diconnected map events");
        },

        log: function (m) {
            if (!enableLogging) {
                return;
            }

            console.log(`[Map ${wrapper.mapId}:${wrapper.refId}]: ${m}`);
        },

        _onMouseClick: function (e) {
            wrapper.invokeRef("OnMouseClick", { latitude: e.latlng.lat, longitude: e.latlng.lng });
        },

        _onMouseDblClick: function (e) {
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

        _onMouseMove: function (e) {
            wrapper.invokeRef("OnMouseMove", { latitude: e.latlng.lat, longitude: e.latlng.lng });
        },

        _onMouseOut: function (e) {
            wrapper.invokeRef("OnMouseLeave", { latitude: e.latlng.lat, longitude: e.latlng.lng });
        },

        _onResize: function (e) {
            wrapper.invokeRef("OnResized", e.newSize);
        },

        _onMove: function (e) {
            wrapper.invokeRef("OnCenterChanging", e.newSize);
        },

        _onMoveEnd: function (e) {
            let center = e.target.getCenter();
            wrapper.invokeRef("OnCenterChanged", { latitude: center.lat, longitude: center.lng });
        },

        _onZoom: function (e) {
            let zoom = e.target.getZoom();
            wrapper.invokeRef("OnZoomChanging", zoom);
        },

        _onZoomEnd: function (e) {
            let zoom = e.target.getZoom();
            wrapper.invokeRef("OnZoomChanged", zoom);
        }
    };

    //-- mouse events
    map.on("click", wrapper._onMouseClick);
    map.on("dblclick", wrapper._onMouseDblClick);
    map.on("mousedown", wrapper._onMouseDblClick);
    map.on("mouseup", wrapper._onMouseUp);
    map.on("mouseover", wrapper._onMouseOver);
    map.on("mousemove", wrapper._onMouseMove);
    map.on("mouseout", wrapper._onMouseOut);

    //-- map events
    map.on("resize", wrapper._onResize);
    map.on("move", wrapper._onMove);
    map.on("moveend", wrapper._onMoveEnd);
    map.on("zoom", wrapper._onZoom);
    map.on("zoomend", wrapper._onZoomEnd);

    return wrapper;
}