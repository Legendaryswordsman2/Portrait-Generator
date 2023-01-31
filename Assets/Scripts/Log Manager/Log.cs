using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Log : MonoBehaviour
{
    [SerializeField, ReadOnlyInspector] string logMessage;
    [SerializeField, ReadOnlyInspector] string logDetails;

    [Space]

    [SerializeField] TMP_Text text;


    public void SetupLog(string _logMessage, string _logDetails, LogType type)
    {
        logMessage = _logMessage;
        logDetails = _logDetails;

        text.text = "[" + DateTime.Now + "] [" + type + "] " + logMessage;

        switch (type)
        {
            case LogType.Error:
                text.color = Color.red;
                break;
            case LogType.Warning:
                text.color = Color.yellow;
                break;
            case LogType.Log:
                break;
            case LogType.Exception:
                text.color = Color.red;
                break;
            default:
                break;
        }
    }
}
