using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CommandLineController : MonoBehaviour {

    private static CommandLineController instance = null;

    [SerializeField]
    private InputField commandField = null;
    [SerializeField]
    private Button commandSendButton = null;
    [SerializeField]
    private Toggle clientAPIToggle = null;

    static public event Action<string, Dictionary<string, string>> CallEvent;
	static public event Action<string, string, Dictionary<string, string>> CallClientAPIEvent;

    void Awake()
    {
        instance = this;
    }

    void Start () {
        this.commandSendButton.onClick.AddListener(ParseStringToVK);

#if UNITY_WEBGL && !UNITY_EDITOR
        clientAPIToggle.interactable = true;
#endif
    }

    private void ParseStringToVK()
    {
        try
        {
            string queryStr = instance.commandField.text;
            string[] tmpArray = queryStr.Split('?');
            string method = tmpArray[0];
			Dictionary<string, string> parameters = null;

			//find array after ?
            if (tmpArray.Length > 1 && tmpArray[1].Length > 0)
            {
				//find array after &
				tmpArray = tmpArray[1].Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
				if (tmpArray[0].Contains('=')) 
				{
				    parameters = tmpArray.ToDictionary(x => x.Split('=')[0], x => x.Split('=')[1]);
				}
				else
				{
					string action = tmpArray[0];

					if (instance.clientAPIToggle.isOn)
						CallClientAPIEvent.Invoke(method, action, null);
				}	
            }

			if (instance.clientAPIToggle.isOn)
				CallClientAPIEvent.Invoke(method, null, parameters);
			else
				CallEvent.Invoke(method, parameters);
        }
		catch (Exception e){
			Debug.LogError (e);
		}
	}
}
