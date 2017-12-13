using System;
using System.Collections.Generic;
using UnityEngine;

namespace VkontakteSDK.Core
{
    public abstract class BaseSDK
    {
        public static AppVars appVars;
        protected const string apiLink = "https://api.vk.com/api.php";
        protected const string apiVersion = "5.0";

        public abstract void Init(Action<string> _Callback);
        public abstract void Call(string _method, Dictionary<string, string> _parameters, Action<string> _Callback);
        //public abstract void CallClientAPI(string _method, string _action = null, Dictionary<string, string> _parameters = null);
        //public abstract void Call(string _method, Dictionary<string, string> _parameters, Action<VKApiResponse> _Callback);

		public virtual void CallClientAPI(string _method, string _action = null, Dictionary<string, string> _parameters = null)
		{
			Debug.LogError("This platform doesn't support method CallClientAPI");
		}

		public virtual void AddCallback(string _eventName, Action<string> _Callback = null)
		{
			Debug.LogError("This platform doesn't support method AddCallback");
		}

        protected void ParseAPIResponse(Dictionary<string, object> _data, Action<Dictionary<string, object>> _Callback)
        {
            VKApiResponse response = new VKApiResponse(_data);
            if (!response.IsError())
            {
                if (_Callback != null)
                {
                    _Callback(response.JSONResponse);
                }
                else
                {
                    Debug.Log(">>> API _Callback is NULL!");
                }
            }
        }
    }
}
