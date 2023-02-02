using UnityEngine;
using TMPro;
using System.IO;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Cysharp.Threading.Tasks.Triggers;
using Cysharp.Threading.Tasks;

public class LogManager : MonoBehaviour
{
    public static LogManager Instance;

    [Header("Settings")]
    [SerializeField, Range(1, 1000)] int logCap = 250;
    [SerializeField] float fontSize = 25;

    [Tooltip("Clears all logs when the current scene changes")]
    [SerializeField] bool clearLogsOnSceneChange;

    [Space]


    [SerializeField, ReadOnlyInspector] List<LogData> queuedLogs;
    [SerializeField, ReadOnlyInspector] List<Log> logs = new();

    [SerializeField] GameObject logConsole;
    LogConsole logConsoleComponent;
    [SerializeField] GameObject logMenuContents;
    [SerializeField] GameObject logPrefab;
    [SerializeField] ScrollRect logMenuScrollRect;
    public Transform SliderBottomPOS;
    [SerializeField] GameObject bottomPrefab;

    [Space]

    [SerializeField] SetupMessage setupMessage;
    [SerializeField] GameObject errorMessageMenu;
    [SerializeField] TMP_Text errorText;

    int logIndex;
    bool mouseDown = false;

    [SerializeField] bool isAtBottom;

    public Transform bottomListTransform;

    [SerializeField] Image imageTest;

    private void Awake()
    {
        Instance = this;

        logConsoleComponent = logConsole.GetComponent<LogConsole>();

        logConsoleComponent.OnEnabled += OnLogConsoleEnabled;

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

        bottomListTransform = Instantiate(bottomPrefab, logMenuContents.transform).transform;

        Application.logMessageReceivedThreaded += OnLogMessageReceived;
    }

    private void OnDisable()
    {
        Application.logMessageReceivedThreaded -= OnLogMessageReceived;
    }


    private void OnLogMessageReceived(string condition, string stackTrace, LogType type)
    {
        if (!logConsole.activeSelf)
        {
            if (queuedLogs.Count >= logCap)
                queuedLogs.RemoveAt(0);
            queuedLogs.Add(new LogData(condition, stackTrace, type));
            return;
        }

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

    private void LogBaseInfo(string message)
    {
        Log log = Instantiate(logPrefab, logMenuContents.transform).GetComponent<Log>();

        log.SetupBaseInfoLog(message);
    }

    private void Update()
    {
        //if(SliderBottomPOS.position.y <= bottomListTransform.position.y)
        //{
        //    isAtBottom = true;
        //    imageTest.color = Color.green;
        //}
        //else
        //{
        //    isAtBottom = false;
        //    imageTest.color = Color.red;
        //}
    }
    public void Log(string logMessage, string logDetails = "")
    {
        if (SliderBottomPOS.position.y <= bottomListTransform.position.y)
        {
            isAtBottom = true;
            imageTest.color = Color.green;
        }
        else
        {
            isAtBottom = false;
            imageTest.color = Color.red;
        }

        bool logsfull = CheckLogCap();

        if (logsfull)
            logs[^1].SetupLog(logMessage, logDetails, LogType.Log, this, SliderBottomPOS.position.y);
        else
        {
            logs[logIndex].SetupLog(logMessage, logDetails, LogType.Log, this, SliderBottomPOS.position.y);
            logIndex++;

            if (isAtBottom)
                GoToBottom();
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

    public async void GoToBottom()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(logConsole.GetComponent<RectTransform>());
        await UniTask.WaitForEndOfFrame(this);
        logMenuScrollRect.ScrollToBottom();
        //else
        //    Debug.Log("Mouse is currently down");
    }
    bool CheckLogCap()
    {
        if (logIndex >= logCap)
        {
            logs[0].transform.SetSiblingIndex(logs.Count + 5);

            Log log = logs[0];

            logs.Remove(log);
            logs.Add(log);

            return true;
        }
        else
        {
            //if(logMenuScrollRect.verticalNormalizedPosition)
            //Debug.Log(logMenuScrollRect.verticalNormalizedPosition);
            return false;
        }

    }

    void ToggleLogMenu()
    {
        if (!logConsole.activeSelf) logMenuScrollRect.ScrollToBottom();
        logConsole.SetActive(!logConsole.activeSelf);
    }
    private void OnLogConsoleEnabled(object sender, EventArgs e)
    {
        for (int i = 0; i < queuedLogs.Count; i++)
        {
            switch (queuedLogs[i].type)
            {
                case LogType.Error:
                    LogError(queuedLogs[i].condition, queuedLogs[i].stackTrace);
                    break;
                case LogType.Warning:
                    LogWarning(queuedLogs[i].condition, queuedLogs[i].stackTrace);
                    break;
                case LogType.Log:
                    Log(queuedLogs[i].condition, queuedLogs[i].stackTrace);
                    break;
                case LogType.Exception:
                    LogException(new Exception(queuedLogs[i].condition), queuedLogs[i].stackTrace);
                    break;
            }
        }

        queuedLogs.Clear();
    }

    //public void OnPointerDown(PointerEventData eventData)
    //{
    //    mouseDown = true;
    //}

    //public void OnPointerUp(PointerEventData eventData)
    //{
    //    mouseDown = false;
    //}

    [Serializable]
    public class LogData
    {
        public string condition;
        public string stackTrace;
        public LogType type;

        public LogData(string condition, string stackTrace, LogType type)
        {
            this.condition = condition;
            this.stackTrace = stackTrace;
            this.type = type;
        }
    }
}
