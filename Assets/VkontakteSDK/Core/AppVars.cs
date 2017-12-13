using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using VkontakteSDK.JSON;

namespace VkontakteSDK.Core
{
    public class AppVars
    {
		//same as properties vkontakte
		public int AppId { get; set; }
		public int ViewerId { get; set; }
		public string Sid { get; set; }
		public string Secret { get; set; }

		public string Referrer { get; set; }
		//public string authKey;

		/// <param name="_app_id">Your application ID</param>
		/// <param name="_viewer_id">User ID</param>
		/// <param name="_sid">Session ID</param>
		/// <param name="_secret">This parameter must contain the current value of the secret key</param>
		public AppVars (int _app_id, int _viewer_id, string _sid, string _secret, string _referrer = "") 
		{
			AppId = _app_id;
			ViewerId = _viewer_id;
			Sid = _sid;
			Secret = _secret;
			Referrer = _referrer;
		}

        //Generates signature
        public string GetSig(Dictionary<string, string> _requestParams)
        {
            string signature = "";
            List<string> sortedList = new List<string>();
            foreach (var pair in _requestParams)
            {
                sortedList.Add(pair.Key + "=" + pair.Value);
            }
            sortedList.Sort();

            signature += string.Join("", sortedList.ToArray());
            if (this.ViewerId > 0)
            {
                signature = Convert.ToString(this.ViewerId) + signature;
            }
            signature += this.Secret;
            return this.MD5Hash(signature);
        }

        private string MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }

        public string ToVKResponseFormat
        {
            get
            {
                return Json.Encode(new Dictionary<string, string>()
                {
                    { "api_id", AppId.ToString() },
                    { "viewer_id", ViewerId.ToString() },
                    { "sid", Sid },
                    { "secret", Secret },
                    { "referrer", Referrer }
                });
            }
        }
    }
}
