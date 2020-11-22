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
    return new google.maps.Map(
        element,
        {
            center: { lat: options.latitude, lng: options.longitude },
            zoom: options.zoom,
        }
    );
}
