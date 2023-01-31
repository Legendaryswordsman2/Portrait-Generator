using UnityEngine;
using TMPro;
using System.IO;
using System;

public class LogManager : MonoBehaviour
{
    public static LogManager Instance;

    [SerializeField] SetupMessage setupMessage;

    [Space]

    [SerializeField] GameObject errorMessageMenu;
    [SerializeField] TMP_Text errorText;

    [SerializeField] GameObject logMenu;
    [SerializeField] GameObject logMenuContents;

    [SerializeField] GameObject logPrefab;

    [Space]

    [SerializeField] GameObject detailsView;
    [SerializeField] TMP_Text detailsViewText;
    private void Awake()
    {
        Instance = this;

        Application.logMessageReceived += Application_logMessageReceived;

        LogBaseInfo("Unity version: " + Application.unityVersion);

        LogBaseInfo("OS: " + SystemInfo.operatingSystem + " (" + SystemInfo.operatingSystemFamily + ")");

        LogBaseInfo("GPU: " + SystemInfo.graphicsDeviceName + " (Running " + SystemInfo.graphicsDeviceType + ")");

        LogBaseInfo("CPU: " + SystemInfo.processorType + " (" + SystemInfo.processorCount + " X " + SystemInfo.processorFrequency + " Mhz)");

        LogBaseInfo("RAM: " + SystemInfo.systemMemorySize + " MB");

        LogBaseInfo("Current Directory: " + Directory.GetCurrentDirectory());

    }

    private void Application_logMessageReceived(string condition, string stackTrace, LogType type)
    {
        //Debug.Log(type + "rfjhti");
        //Debug.Log(condition + "ijoergijohr");
        //Debug.Log(stackTrace + "errreh");
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
        Log log = Instantiate(logPrefab, logMenuContents.transform).GetComponent<Log>();

        log.SetupLog(logMessage, logDetails, LogType.Log, this);
    }

    public void LogWarning(string warningMessage, string warningDetails = "")
    {
        Log log = Instantiate(logPrefab, logMenuContents.transform).GetComponent<Log>();

        log.SetupLog(warningMessage, warningDetails, LogType.Warning, this);
    }

    public void LogError(string errorMessage, string errorDetails = "")
    {
        //errorText.text = errorMessage;

        //errorMessageMenu.SetActive(true);

        Log log = Instantiate(logPrefab, logMenuContents.transform).GetComponent<Log>();

        log.SetupLog(errorMessage, errorDetails, LogType.Error, this);

        //log.text = "[" + DateTime.Now + "] [Error]:" + errorMessage;

        //log.color = Color.red;
    }

    public void LogException(Exception exception, string errorDetails = "")
    {
        Log log = Instantiate(logPrefab, logMenuContents.transform).GetComponent<Log>();

        log.SetupLog(exception.ToString(), errorDetails, LogType.Exception, this);
    }

    void ToggleLogMenu()
    {
        logMenu.SetActive(!logMenu.activeSelf);
    }

    public void SetDetailsView(string text)
    {
        detailsView.SetActive(true);
        detailsViewText.text = text;
    }
}
