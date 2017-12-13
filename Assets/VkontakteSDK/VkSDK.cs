using System;
using System.Collections.Generic;
using UnityEngine;
using VkontakteSDK.Core;
using VkontakteSDK.Core.HTTP;
using VkontakteSDK.Core.WebGL;

namespace VkontakteSDK
{
    sealed public class VkSDK : MonoBehaviour
    {
        private VkSDK() { }

        private static BaseSDK SDK;

        public static AppVars AppVars
        {
            get
            {
            	return BaseSDK.appVars;
            }
            set
            {
                BaseSDK.appVars = value;
            }
        }

        void Awake()
        {          
            DontDestroyOnLoad(this); // class will be available after loading a new scene.
			#region Select platform
				#if UNITY_WEBGL && !UNITY_EDITOR
					SDK = new WebGLSDK();
				#else
					SDK = new HttpSDK(this.gameObject);
				#endif
			#endregion
        }

		/// <summary>Initializes connection to VK API</summary>
		/// <param name="_Callback">If initialization is successful, this function is called</param>
        static public void Init (Action<string> _Callback)
        {
            SDK.Init(_Callback);
        }

		/// <summary>
		/// Makes request to VK API and passes the received data to callback function.
		/// </summary>
		/// <see>https://vk.com/dev/methods</see>
        static public void Call(string _method, Dictionary<string, string> _parameters, Action<string> _Callback)
        {
            SDK.Call(_method, _parameters, _Callback);
        }
			
		/// <summary>
		/// Calls Client API method externally. Client API methods allow to interact with the user interface from the application open dialog boxes, change window size, etc
		/// </summary>
		/// <see>https://vk.com/dev/clientapi</see>
		/// <remarks>Works only with WebGL</remarks>
        static public void CallClientAPI (string _method, string _action = null, Dictionary<string, string> _parameters = null)
        {
            SDK.CallClientAPI(_method, _action, _parameters);
        }

        /// <summary>
        /// Adds callback function as name event handler.
        /// </summary>
        /// <see>https://vk.com/dev/Javascript_SDK</see>
        /// <remarks>Works only with WebGL</remarks>
        static public void AddCallback (string _eventName, Action<string> _Callback = null)
		{
			SDK.AddCallback(_eventName, _Callback);
		}
    }
}