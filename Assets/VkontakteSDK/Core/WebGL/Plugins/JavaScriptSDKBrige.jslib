var JavaScriptSDKBridge =
{
	$ptr: {
		GetFromString : function(_str) {
            var bufferSize = lengthBytesUTF8(_str) + 1;
            var buffer = _malloc(bufferSize);
            stringToUTF8Array(_str, HEAPU8, buffer, bufferSize)
            return buffer;
		}
	},

    InitWebGL: function (_callbackId, _Callback)
	{
	    var vkApiVersion = "5.0";
        console.log("Init Unity VkApi v." + vkApiVersion);

        VK.init(function() {
            console.log("VK init success");

            var socialRefferer = {};
            parseUrlParamsTo(socialRefferer, window.location.search.substring(1));

            var buffer = ptr.GetFromString(JSON.stringify(socialRefferer));
            Runtime.dynCall('vii', _Callback, [_callbackId, buffer]);

        }, function() {
            console.log("VK init failed");
        }, vkApiVersion);

        function parseUrlParamsTo(_target, _paramsString) {

        	if (_paramsString.length > 0) {
        		var paramsData = _paramsString.split('&');
        		for (var i = 0; i < paramsData.length; i++) {
        			var data = paramsData[i].split('=');
        			_target[data[0]] = data[1];
        		}
        	}
        }
    },

    CallWebGL: function (_callbackId, _method, _params, _Callback)
    {
        VK.api(Pointer_stringify(_method), JSON.parse(Pointer_stringify(_params)), function(_data) {

            var strReturn = JSON.stringify( _data);
            console.log("Response: " + strReturn);
            Runtime.dynCall('vii', _Callback, [_callbackId, ptr.GetFromString(strReturn)]);
        });
    },

    CallClientAPIWebGL: function (_method, _action, _params)
    {
        console.log("Call client API method: " + Pointer_stringify(_method) + "; action: " + Pointer_stringify(_action) + "; params: " + Pointer_stringify(_params));

        var actionName = Pointer_stringify(_action);
        if (actionName.length > 0) {
            VK.callMethod(Pointer_stringify(_method), actionName, JSON.parse(Pointer_stringify(_params)));
        } else {
            VK.callMethod(Pointer_stringify(_method), JSON.parse(Pointer_stringify(_params)));
        }
    },

    AddCallbackWebGL: function (_callbackId, _eventName, _Callback)
    {
        VK.addCallback(Pointer_stringify(_eventName), function(_data) {

            var strReturn = JSON.stringify( _data);
            console.log("Response: " + strReturn);
            Runtime.dynCall('vii', _Callback, [_callbackId, ptr.GetFromString(strReturn)]);
        });
    },
};

autoAddDeps(JavaScriptSDKBridge, '$ptr');
mergeInto(LibraryManager.library, JavaScriptSDKBridge);
