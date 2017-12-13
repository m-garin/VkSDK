using UnityEngine;
using VkontakteSDK.Core;

namespace UnityEditor.VkontakteSDK
{
    [CustomEditor(typeof(SettingsVkSDK))]
    public class SettingsVkSDKEditor : Editor
    {
        private SettingsVkSDK myScript;

        public override void OnInspectorGUI()
        {

            myScript = (SettingsVkSDK)target;

            EditorGUILayout.HelpBox("For local testing without IFrame authorization", MessageType.Info);

            myScript.appId = EditorGUILayout.IntField("Your application ID", myScript.appId);
            if (myScript.appId == 0)
                EditorGUILayout.HelpBox("Plese enter your vk application ID", MessageType.Warning);

            myScript.viewerId = EditorGUILayout.IntField("User ID", myScript.viewerId);
            if (myScript.viewerId == 0)
                EditorGUILayout.HelpBox("Plese enter your vk user ID", MessageType.Warning);

            myScript.sid = EditorGUILayout.TextField("Session ID (SID)", myScript.sid);
            if (string.IsNullOrEmpty(myScript.sid))
                EditorGUILayout.HelpBox("Plese enter your vk session ID", MessageType.Warning);

            myScript.secret = EditorGUILayout.TextField("Secret key", myScript.secret);
            if (string.IsNullOrEmpty(myScript.secret))
                EditorGUILayout.HelpBox("Plese enter your vk secret key", MessageType.Warning);

            if (GUI.changed)
            {
                EditorUtility.SetDirty(myScript);
            }
        }
    }
}