let loadResult = "none";

console.log("osm apiLoader_0.1.2.js loaded");

export function addOpenStreetMapScripts(netObjRef) {
    let myScript = null;
    var element = document.getElementById("proxoftOpenStreetMap");

    if (element) {
        notifyScriptLoadResult(loadResult);
        return;
    }

    let link = document.createElement("link");
    link.rel = "stylesheet"
    link.href = "https://unpkg.com/leaflet@latest/dist/leaflet.css";
    document.body.appendChild(link);

    myScript = document.createElement("script");
    let src = `https://unpkg.com/leaflet@latest/dist/leaflet.js`
    myScript.setAttribute("src", src);
    myScript.setAttribute("id", "proxoftOpenStreetMap");

    document.body.appendChild(myScript);

    myScript.addEventListener("load", scriptLoaded, false);
    myScript.addEventListener("error", scriptLoadFailed, false);

    function scriptLoaded() {
        notifyScriptLoadResult("Loaded");
    }

    function scriptLoadFailed() {
        notifyScriptLoadResult("Failed");
    }

    function notifyScriptLoadResult(result) {

        if (myScript !== null) {
            myScript.removeEventListener("load", scriptLoaded, false);
            myScript.removeEventListener("error", scriptLoadFailed, false);
        }

        loadResult = result;

        netObjRef.invokeMethodAsync("NotifyLoadScriptStatus", loadResult);
    }
}