var mapElements = [];

export function InitializeMap(elementId, options, netObjRef) {
    let mapElement = findMap(elementId);
    if (mapElement !== null) {
        mapElement.ref = netObjRef;
        google.maps.event.trigger(mapElement.map, 'resize');
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
    let element = document.getElementById(elementId);
    let map = new google.maps.Map(
        element,
        {
            center: { lat: options.center.latitude, lng: options.center.longitude },
            zoom: options.zoom,
        }
    );

    return map;
}

function addListeners(mapEl) {
    let map = mapEl.map;

    map.addListener("center_changed", () => {
        let center = map.getCenter();
        mapEl.ref.invokeMethodAsync("OnCenterChanged", { latitude: center.lat(), longitude: center.lng() });
    });
}
