let loadResult = "none";

console.log("apiLoader_1.0.0.js");

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
    document.getElementsByTagName('head')[0].appendChild(link);

    myScript = document.createElement("script");
    let src = `https://unpkg.com/leaflet@latest/dist/leaflet.js`
    myScript.setAttribute("src", src);
    // myScript.setAttribute("integrity", "sha512-XQoYMqMTK8LvdxXYG3nZ448hOEQiglfqkJs1NOQV44cWnUrBc8PkAOcXy20w0vlaXaVUearIOBhiXZ5V3ynxwA==")
    // myScript.setAttribute("crossorigin", " ");
    myScript.setAttribute("id", "proxoftOpenStreetMap");

    document.body.appendChild(myScript);

    myScript.addEventListener("load", scriptLoaded, false);
    myScript.addEventListener("error", scriptLoadFailed, false);

    function notifyScriptLoadResult(result) {

        if (myScript !== null) {
            myScript.removeEventListener("load", scriptLoaded, false);
            myScript.removeEventListener("error", scriptLoadFailed, false);
        }

        loadResult = result;

        netObjRef.invokeMethodAsync("NotifyLoadScriptStatus", loadResult);
    }

    function scriptLoaded() {
        notifyScriptLoadResult("Loaded");
    }

    function scriptLoadFailed() {
        notifyScriptLoadResult("Failed");
    }
}