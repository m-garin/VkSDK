using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOT;
using VkontakteSDK.JSON;

namespace VkontakteSDK.Core.WebGL
{
    public class WebGLSDK : BaseSDK
	{
        #region Callbacks ID controller 
        private static Dictionary <int, Action<string>> callbacks = new Dictionary <int, Action<string>>();
		private static int lastCallbackId = 0;

        private static void InvokeCallback(int _callbackId, string _data)
        {
            callbacks[_callbackId].Invoke(_data);
            callbacks.Remove(_callbackId);
        }

        private static int AddCallback(Action<string> _Callback)
        {
            int callbaclId = ++lastCallbackId;
            callbacks.Add(callbaclId, _Callback);
            return callbaclId;
        }
        #endregion

        private delegate void IntPtrCallback (int _callbackId, System.IntPtr _pointer);
        #region WebGL Interface
        [DllImport("__Internal")]
		private static extern void InitWebGL(int _callbackId, IntPtrCallback _Callback);

		[DllImport("__Internal")]
		private static extern void CallWebGL(int _callbackId, string _method, string _parameters, IntPtrCallback _Callback);

        [DllImport("__Internal")]
        private static extern void CallClientAPIWebGL(string _method, string _action, string _parameters);

		[DllImport("__Internal")]
		private static extern void AddCallbackWebGL(int _callbackId, string _eventName, IntPtrCallback _Callback);

		[DllImport("__Internal")]
		private static extern void RemoveCallbackWebGL(int _callbackId, string _eventName, IntPtrCallback _Callback);
        #endregion

        #region WebGL Static Callbacks
        [MonoPInvokeCallback(typeof(IntPtrCallback))]
		public static void InitWebGLCallback (int _callbackId, System.IntPtr _ptr) {

            string response = Marshal.PtrToStringAuto(_ptr);
            Dictionary<string, object> tmpJson = Json.Decode (response) as Dictionary<string, object>;
            //appVars init
			appVars = new AppVars (Convert.ToInt32(tmpJson["api_id"]), 
										 Convert.ToInt32(tmpJson["viewer_id"]), 
										 Convert.ToString(tmpJson["sid"]), Convert.ToString(tmpJson["secret"]));
            InvokeCallback(Convert.ToInt32(_callbackId), response);
        }

        [MonoPInvokeCallback(typeof(IntPtrCallback))]
        public static void CallWebGLCallback(int _callbackId, System.IntPtr _ptr)
        {
            InvokeCallback(_callbackId, Marshal.PtrToStringAuto(_ptr));
        }

		[MonoPInvokeCallback(typeof(IntPtrCallback))]
		public static void AddCallbackWebGLCallback(int _callbackId, System.IntPtr _ptr)
		{
			InvokeCallback(_callbackId, Marshal.PtrToStringAuto(_ptr));
		}
        #endregion

		/// <summary>
		/// Initializes connection to a parent window to start external calls.
		/// </summary>
		/// <param name="_Callback">If initialization is successful, this function is called</param>
        public override void Init (Action<string> _Callback)
		{
			InitWebGL(AddCallback(_Callback), InitWebGLCallback);
		}

        public override void Call (string _method, Dictionary<string, string> _parameters, Action<string> _Callback)
        {
            CallWebGL(AddCallback(_Callback), _method, Json.Encode(_parameters), CallWebGLCallback);
        }

        public override void CallClientAPI(string _method, string _action = null, Dictionary<string, string> _parameters = null)
        {
            CallClientAPIWebGL(_method, _action, Json.Encode(_parameters));
        }

		public override void AddCallback(string _eventName, Action<string> _Callback = null)
		{
			AddCallbackWebGL(AddCallback(_Callback), _eventName, AddCallbackWebGLCallback);
		}
	}
}

