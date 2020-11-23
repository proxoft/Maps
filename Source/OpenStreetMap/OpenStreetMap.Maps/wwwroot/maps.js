var mapElements = [];

export function InitializeMap(elementId, options, netObjRef) {
    let mapElement = findMap(elementId);
    if (mapElement !== null) {
        mapElement.ref = netObjRef;
        return;
    }

    let map = createMap(elementId, options);

    mapElement = {
        elementId: elementId,
        map: map,
        ref: netObjRef
    };

    addListeners(mapElement);

    mapElements.push(mapElement);
}

function findMap(elementId) {
    let i = mapElements.findIndex(me => me.elementId == elementId);
    return i === -1
        ? null
        : mapElements[i];
}

function createMap(elementId, options) {
    let map = L.map(elementId)
        .setView([options.center.latitude, options.center.longitude], options.zoom);

    L.tileLayer('https://{s}.tile.osm.org/{z}/{x}/{y}.png', {
        attribution: '&copy; <a href="https://osm.org/copyright">OpenStreetMap</a> contributors'
    }).addTo(map);

    return map;
}

function addListeners(mapEl) {
    let map = mapEl.map;

    mapEl.map.on("moveend", () => {
        let center = map.getCenter();
        mapEl.ref.invokeMethodAsync("OnCenterChanged", { latitude: center.lat, longitude: center.lng });
    });
}