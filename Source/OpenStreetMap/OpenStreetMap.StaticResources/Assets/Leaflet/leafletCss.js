export function appendOpenStreetMapCss(resourcePath) {
    console.log(`appending to document: ${resourcePath}/leaflet.css`);

    let element = document.getElementById("proxoftOpenStreetMapCss");
    if (element) {
        return;
    }

    let link = document.createElement("link");
    link.id = "proxoftOpenStreetMapCss";
    link.rel = "stylesheet"
    link.href = resourcePath + "/leaflet.css";

    document.body.appendChild(link);
}