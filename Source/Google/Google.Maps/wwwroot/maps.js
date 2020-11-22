export function InitializeMap(elementId, options, mapRef) {
    let element = document.getElementById(elementId);

    let map = new google.maps.Map(element, {
        center: { lat: options.latitude, lng: options.longitude },
        zoom: options.zoom,
    });
}