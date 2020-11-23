var mapElements = [];

export function InitializeMap(elementId, options, netObjRef) {
    let wrapper = findMapWrapper(elementId);
    if (wrapper !== null) {
        wrapper.ref = netObjRef;
        return;
    }

    let map = createMap(elementId, options);

    wrapper = {
        elementId: elementId,
        map: map,
        ref: netObjRef
    };

    addListeners(wrapper);

    mapElements.push(wrapper);
}

export function PanTo(elementId, center) {
    var wrapper = findMapWrapper(elementId);
    wrapper.map.panTo([center.latitude, center.longitude]);
}

function findMapWrapper(elementId) {
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

function addListeners(wrapper) {
    let map = wrapper.map;

    map.on("moveend", () => {
        let center = map.getCenter();
        wrapper.ref.invokeMethodAsync("OnCenterChanged", { latitude: center.lat, longitude: center.lng });
    });
}