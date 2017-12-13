using System;
using System.Collections.Generic;
using UnityEngine;

namespace VkontakteSDK.Core.HTTP
{
    public class HttpSDK : BaseSDK
    {
        private UnityWebRequestWrapper httpWrapper;

        public HttpSDK (GameObject _parentGameObject)
        {
            GameObject VkSDKlib = new GameObject("HttpWrapper");
            httpWrapper = (VkSDKlib).AddComponent<UnityWebRequestWrapper>();
            VkSDKlib.transform.parent = _parentGameObject.transform;
        }
        
        public override void Init(Action<string> _Callback)
        {
            SettingsVkSDK settings = Resources.Load<SettingsVkSDK>("SettingsVkSDK");
            appVars = new AppVars(settings.appId, settings.viewerId, settings.sid, settings.secret);
            _Callback(appVars.ToVKResponseFormat);
        }

        public override void Call(string _method, Dictionary<string, string> _parameters, Action<string> _Callback = null)
        {
            Dictionary<string, string> requestParams = new Dictionary<string, string>
            {
                { "api_id", appVars.AppId.ToString() },
                { "format", "JSON" },
                { "method", _method },
                { "v", apiVersion }
            };

            if (_parameters != null)
            {
                foreach (var pair in _parameters)
                {
                    requestParams.Add(pair.Key, pair.Value);
                }
            }
            else
            {
                _parameters = new Dictionary<string, string>();
            }

            _parameters.Add("api_id", appVars.AppId.ToString());
            _parameters.Add("format", "JSON");
            _parameters.Add("method", _method);
            _parameters.Add("sid", appVars.Sid);
            _parameters.Add("sig", appVars.GetSig(requestParams));
            _parameters.Add("v", apiVersion);
            httpWrapper.Call(apiLink, _parameters, (string _data) => {
                if (_Callback != null)
                {
                    _Callback.Invoke(_data);
                }
            });
        }
    }
}

