using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VkontakteSDK.Core
{
    public sealed class VKApiResponse
    {
        public VKApiResponse(Dictionary<string, object> _JSONResponseLocal)
        {
            this.JSONResponseLocal = _JSONResponseLocal;
        }

		public Dictionary<string, object> JSONResponse {
            get
            {
                Dictionary<string, object> finalResponse = new Dictionary<string, object>();
                var tmpResponse = JSONResponseLocal["response"];
                if (tmpResponse is List<object>)
                {
                    finalResponse = (tmpResponse as List<object>).Select((s, i) => new { Key = i.ToString(), Value = s }).ToDictionary(v => v.Key, v => v.Value);
                }
                else if (tmpResponse is Dictionary<string, object>)
                {
                    finalResponse = tmpResponse as Dictionary<string, object>;
                }

                return finalResponse;
            }
        }

        private Dictionary<string, object> JSONResponseLocal;

        public bool IsError()
        {
            bool isError = false;
            object error;
            if (JSONResponseLocal.TryGetValue("error", out error))
            {
                isError = true;
                DisplayError("VK API error code: " + (error as Dictionary<string, object>)["error_code"].ToString() + "\n" + (error as Dictionary<string, object>)["error_msg"].ToString());
            }
            return isError;
        }

        private void DisplayError(string _errorText)
        {
            Debug.LogError(_errorText);
        }
    }
}