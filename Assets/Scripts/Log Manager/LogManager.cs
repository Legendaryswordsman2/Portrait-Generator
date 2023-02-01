using UnityEngine;
using TMPro;
using System.IO;
using System;
using System.Collections.Generic;

public class LogManager : MonoBehaviour
{
    public static LogManager Instance;

    [Header("Settings")]
    [SerializeField] int logCap = 250;
    [SerializeField] float fontSize = 25;

    [Tooltip("Clears all logs when the current scene changes")]
    [SerializeField] bool clearLogsOnSceneChange;

    [Space]


    [SerializeField] List<Log> logs = new();

    [SerializeField] GameObject logMenu;
    [SerializeField] GameObject logMenuContents;

    [SerializeField] GameObject logPrefab;

    [Space]

    [SerializeField] SetupMessage setupMessage;
    [SerializeField] GameObject errorMessageMenu;
    [SerializeField] TMP_Text errorText;

    int logIndex;

    private void Awake()
    {
        Instance = this;

        Application.logMessageReceivedThreaded += Application_logMessageReceived;

        LogBaseInfo("Unity version: " + Application.unityVersion);

        LogBaseInfo("OS: " + SystemInfo.operatingSystem + " (" + SystemInfo.operatingSystemFamily + ")");

        LogBaseInfo("GPU: " + SystemInfo.graphicsDeviceName + " (Running " + SystemInfo.graphicsDeviceType + ")");

        LogBaseInfo("CPU: " + SystemInfo.processorType + " (" + SystemInfo.processorCount + " X " + SystemInfo.processorFrequency + " Mhz)");

        LogBaseInfo("RAM: " + SystemInfo.systemMemorySize + " MB");

        LogBaseInfo("Current Directory: " + Directory.GetCurrentDirectory());

        for (int i = 0; i < logCap; i++)
        {
            Log log = Instantiate(logPrefab, logMenuContents.transform).GetComponent<Log>();

            log.Init(fontSize);

            logs.Add(log);
        }
    }

    private void Application_logMessageReceived(string condition, string stackTrace, LogType type)
    {
        switch (type)
        {
            case LogType.Error:
                LogError(condition, stackTrace);
                break;
            case LogType.Warning:
                LogWarning(condition, stackTrace);
                break;
            case LogType.Log:
                Log(condition, stackTrace);
                break;
            case LogType.Exception:
                LogException(new Exception(condition), stackTrace);
                break;
        }
    }

    private void Start()
    {
        InputManager.playerInputActions.General.OpenLogMenu.performed += OpenLogMenu_performed;
    }

    private void OpenLogMenu_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        ToggleLogMenu();
    }

    public void SetFirstTimeSetupMessage(bool isActive)
    {
        setupMessage.SetSetupMessage(isActive);
    }

    void LogBaseInfo(string message)
    {
        Log log = Instantiate(logPrefab, logMenuContents.transform).GetComponent<Log>();

        log.SetupBaseInfoLog(message);
    }

    public void Log(string logMessage, string logDetails = "")
    {
        bool logsfull = CheckLogCap();

        if (logsfull)
            logs[^1].SetupLog(logMessage, logDetails, LogType.Log);
        else
        {
            logs[logIndex].SetupLog(logMessage, logDetails, LogType.Log);

            logIndex++;
        }
    }

    public void LogWarning(string warningMessage, string warningDetails = "")
    {
        //Log log = Instantiate(logPrefab, logMenuContents.transform).GetComponent<Log>();

        //CheckLogCap();

        //log.SetupLog(warningMessage, warningDetails, LogType.Warning, fontSize, this);
    }

    public void LogError(string errorMessage, string errorDetails = "")
    {
        //Log log = Instantiate(logPrefab, logMenuContents.transform).GetComponent<Log>();

        //CheckLogCap();

        //log.SetupLog(errorMessage, errorDetails, LogType.Error, fontSize, this);
    }

    public void LogException(Exception exception, string errorDetails = "")
    {
        //Log log = Instantiate(logPrefab, logMenuContents.transform).GetComponent<Log>();

        //CheckLogCap();

        //log.SetupLog(exception.ToString(), errorDetails, LogType.Exception, fontSize, this);
    }

    bool CheckLogCap()
    {
        if (logIndex >= logCap)
        {
            logs[0].transform.SetSiblingIndex(logs.Count + 7);

            Log log = logs[0];

            logs.Remove(log);
            logs.Add(log);

            return true;
        } 
        else
            return false;

    }

    void ToggleLogMenu()
    {
        logMenu.SetActive(!logMenu.activeSelf);
    }
}
