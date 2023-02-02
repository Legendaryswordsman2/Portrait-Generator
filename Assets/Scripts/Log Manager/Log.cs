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

    [Space]

    [SerializeField] TMP_Text text;
    [SerializeField] ContentSizeFitter contentSizeFitter; 

    bool isBaseInfo = false;

    bool isExpanded = false;

    public void Init(float fontSize)
    {
        text.fontSize = fontSize;

        //gameObject.SetActive(false);
    }
    public void SetupLog(string _logMessage, string _logDetails, LogType type)
    {
        logMessage = _logMessage;
        logDetails = _logDetails;

        LogData = "[" + DateTime.Now + "] [" + type + "] ";

        text.text = LogData + logMessage;

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

        contentSizeFitter.SetLayoutVertical();

        gameObject.SetActive(true);
    }

    public void SetupBaseInfoLog(string _logMessage)
    {
        text.text = _logMessage;

        isBaseInfo = true;

        //contentSizeFitter.SetLayoutVertical();

        gameObject.SetActive(true);
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
        contentSizeFitter.SetLayoutVertical();
    }
}
