using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace VkontakteSDK.Core.HTTP
{
    public class UnityWebRequestWrapper : MonoBehaviour
    {
		private bool cacheBusting = true;//if you want to be sure exactly which queries are not cached

        public void Call(string _serverLink, Dictionary<string, string> _parameters, Callback _CallbackSuccess)
        {
            StartCoroutine(Upload(_serverLink, _parameters, _CallbackSuccess));
        }

        public delegate void Callback(string _response);

        protected IEnumerator Upload(string _serverLink, Dictionary<string, string> _parameters, Callback _CallbackSuccess)
        {
            UnityWebRequest www;

            if (_parameters == null || _parameters.Count == 0)
            {
                //Debug.Log("GET");

                if (this.cacheBusting)
                {
                    _serverLink += "?t=" + (System.Int32)(System.DateTime.UtcNow.Subtract(new System.DateTime(1970, 1, 1))).TotalSeconds; // + UnixTime for break query cache 
                }
                www = UnityWebRequest.Get(_serverLink);
            }
            else
            {
                //Debug.Log("POST");
                WWWForm form = new WWWForm();
                foreach (var pair in _parameters)
                {
                    form.AddField(pair.Key, pair.Value);
                }

                www = UnityWebRequest.Post(_serverLink, form);
            }

            try
            {
                yield return www.SendWebRequest();

                if (www.isNetworkError)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    _CallbackSuccess(www.downloadHandler.text);
                }
            }
            finally
            {
                www.Dispose();
            }
        }
    }
}
