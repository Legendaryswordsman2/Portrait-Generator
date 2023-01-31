using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Log : MonoBehaviour, IPointerClickHandler
{
    [SerializeField, ReadOnlyInspector] string logMessage;
    [SerializeField, ReadOnlyInspector] string logDetails;

    [Space]

    [SerializeField] TMP_Text text;

    bool isBaseInfo = false;
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

    public void SetupBaseInfoLog(string _logMessage)
    {
        text.text = _logMessage;

        isBaseInfo = true;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (isBaseInfo) return;

        Debug.Log(logDetails);
    }
}
