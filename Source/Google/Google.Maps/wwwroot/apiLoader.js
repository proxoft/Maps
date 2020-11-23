let loadResult = "none";

export function addGoogleMapsScripts(key, language, netObjRef) {
    let myScript = null;
    var element = document.getElementById("proxoftGooglemaps");

    if (element) {
        notifyScriptLoadResult(loadResult);
        return;
    }

    myScript = document.createElement("script");
    let src = `https://maps.googleapis.com/maps/api/js?key=${key}&language=${language}`
    myScript.setAttribute("src", src);
    myScript.setAttribute("id", "proxoftGooglemaps");

    document.body.appendChild(myScript);

    myScript.addEventListener("load", scriptLoaded, false);
    myScript.addEventListener("error", scriptLoadFailed, false);

    function notifyScriptLoadResult(result) {

        if (myScript !== null) {
            myScript.removeEventListener("load", scriptLoaded, false);
            myScript.removeEventListener("error", scriptLoadFailed, false);
        }

        loadResult = result;

        netObjRef.invokeMethodAsync('NotifyLoadScriptStatus', loadResult);
    }

    function scriptLoaded() {
        notifyScriptLoadResult("Loaded");
    }

    function scriptLoadFailed() {
        notifyScriptLoadResult("Failed");
    }
}