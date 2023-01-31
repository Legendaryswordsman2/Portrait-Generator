using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Log : MonoBehaviour, IPointerClickHandler
{
    [SerializeField, ReadOnlyInspector] string logMessage;
    [SerializeField, ReadOnlyInspector] string logDetails;

    [SerializeField, ReadOnlyInspector] string LogData;

    LogManager logManager;

    [Space]

    [SerializeField] TMP_Text text;
    [SerializeField] ContentSizeFitter contentSizeFitter; 

    bool isBaseInfo = false;

    bool isExpanded = false;
    public void SetupLog(string _logMessage, string _logDetails, LogType type, float fontSize, LogManager lm)
    {
        logMessage = _logMessage;
        logDetails = _logDetails;

        logManager = lm;

        LogData = "[" + DateTime.Now + "] [" + type + "] ";

        text.text = LogData + logMessage;

        text.fontSize = fontSize;
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

    public void SetupBaseInfoLog(string _logMessage, float fontSize)
    {
        text.text = _logMessage;

        text.fontSize = fontSize;

        isBaseInfo = true;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (isBaseInfo) return;

        if (!isExpanded)
        {
            text.text = LogData + logMessage + "\n" + logDetails;
            isExpanded = true;
        }
        else
        {
            text.text = LogData + logMessage;
            isExpanded = false;
        }

        //text.ForceMeshUpdate();
        //text.

        contentSizeFitter.SetLayoutVertical();
    }
}
