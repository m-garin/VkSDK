using UnityEditor;
using UnityEngine;
using VkontakteSDK.Core;

namespace VkontakteSDK.Editor 
{
	public class MenuVkSDK
	{
		private static SettingsVkSDK instance;

		private const string AssetName = "SettingsVkSDK";

		private static SettingsVkSDK Instance
		{
			get
			{
				if (instance == null)
				{
					instance = Resources.Load(AssetName) as SettingsVkSDK;
					if (instance == null)
					{
						// If not found, autocreate the asset object.
						instance = ScriptableObject.CreateInstance<SettingsVkSDK>();
						AssetDatabase.CreateAsset(instance, "Assets/VkontakteSDK/Resources/" + AssetName + ".asset");
					}
				}

				return instance;
			}
		}

		[MenuItem("VkSDK/Edit Settings")]
		static void EditAssetVkSDK()
		{
			Selection.activeObject = Instance;
		}

		[MenuItem("VkSDK/Create A New VK App")]
		static void CreateNewVKApp()
		{
			Application.OpenURL("https://vk.com/editapp?act=create");
		}

		[MenuItem("VkSDK/VK API Documentation")]
		static void OpenVKAPIPage()
		{
			Application.OpenURL("https://vk.com/dev/methods");
		}

		[MenuItem("VkSDK/Client API Documentation")]
		static void OpenClientAPIPage()
		{
			Application.OpenURL("https://vk.com/dev/clientapi");
		}
	}
}