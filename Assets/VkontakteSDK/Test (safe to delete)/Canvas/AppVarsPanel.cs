using UnityEngine;
using VkontakteSDK.Core;
using UnityEngine.UI;
using System;
using VkontakteSDK;

public class AppVarsPanel : MonoBehaviour {

    private static AppVarsPanel instance = null;

    [SerializeField]
	private InputField appIdField = null;
	[SerializeField]
	private InputField viewerIdField = null;
	[SerializeField]
	private InputField sidField = null;
	[SerializeField]
	private InputField secret = null;

	// Use this for initialization
	void Awake () {
        instance = this;
    }

    public static void SetAppVars()
    {
        if (VkSDK.AppVars.AppId > 0 &&
            VkSDK.AppVars.ViewerId > 0 &&
            VkSDK.AppVars.Sid.Length > 0 &&
            VkSDK.AppVars.Sid.Length > 0)
        {
            instance.appIdField.text = VkSDK.AppVars.AppId.ToString();
            instance.viewerIdField.text = VkSDK.AppVars.ViewerId.ToString();
            instance.sidField.text = VkSDK.AppVars.Sid.ToString();
            instance.secret.text = VkSDK.AppVars.Secret.ToString();
        }
        else
        {
            Debug.LogError("Please enter your VK data in SettingsVkSDK.asset");
        }
    }
}
