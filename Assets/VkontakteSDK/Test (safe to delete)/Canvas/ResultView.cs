using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultView : MonoBehaviour {

    private static ResultView instance = null;

    [SerializeField]
    private Text resultText = null;

    void Awake()
    {
        instance = this;
    }

    static public void Output(string _text)
    {
        instance.resultText.text += ">> " + _text + "\n";
    }
}
