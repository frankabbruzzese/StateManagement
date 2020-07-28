(function () {
    window["stateManager"] = {
        "AddUnloadListeners": function (netObject) {
            try {
                window.addEventListener("beforeunload",
                    function (event) {
                        var res = null
                        try {
                            res = netObject.invokeMethod('OnBeforeUnload');
                        }
                        catch (ex) {
                            console.error(ex);
                        }
                        if (res) {
                            event.preventDefault();
                            event.returnValue = res;
                        }
                        else delete event["returnValue"];
                    });
                window.addEventListener("unload",
                    function () {
                        try {
                            netObject.invokeMethod('OnUnload');
                        }
                        catch (ex) {
                            console.error(ex);
                        }

                    });
            } catch (e) {
                console.error(e);
            }
        }
    }
})();