using System.Collections.Generic;
using UnityEngine;
using VkontakteSDK;
using VkontakteSDK.JSON;

public class MethodsController : MonoBehaviour {

    // sample script
    void Start () {

        // crossplatform
        VkSDK.Init ((string _response) => {

            ResultView.Output(_response);
            AppVarsPanel.SetAppVars();
        });

        // if you clicked SEND get query
        CommandLineController.CallEvent += (string _method, Dictionary<string, string> _parameters) => {

            // example "users.get"
            VkSDK.Call(_method, _parameters, (string _responseData) =>
				{
                    ResultView.Output(_responseData);
                });
        };

        // if you clicked SEND and Clientt API checkbox is checked
        CommandLineController.CallClientAPIEvent += (string _method, string _action, Dictionary<string, string> _parameters) => {
            Debug.Log("Call client API method: " + _method);

            VkSDK.CallClientAPI(_method, _action, _parameters);
        };


        //EXAMPLE USAGE
        // how to parse response
        VkSDK.Call("users.get", new Dictionary<string, string>() { { "user_ids", "1" } }, (string _responseJson) => {

            Dictionary<string, object> response = Json.Decode(_responseJson) as Dictionary<string, object>;
            List<object> usersList = response["response"] as List<object>;
            string firstUserName = (usersList[0] as Dictionary<string, object>)["first_name"].ToString();
        });

        // this event occurs when purchase is completed successfully (works only WebGL)
        // https://vk.com/dev/payments_dialog
        VkSDK.AddCallback("onOrderSuccess", (string _response) => {

            Debug.Log("Your payment has been successful. Order ID: " + _response);
            ResultView.Output(_response);
        });

        /*
        // Opens a window to invite user's friends into the application
        //VkSDK.CallClientAPI("showInviteBox");

        // opens a window to purchase products
        // https://vk.com/dev/clientapi?f=6.%2BshowOrderBox
        Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "type", "item" },
                { "item", "item1" }
            };
        VkSDK.CallClientAPI("showOrderBox", null, parameters);
        */

    }
}
