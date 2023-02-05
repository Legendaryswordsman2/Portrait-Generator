using UnityEngine;
using TMPro;
using System.IO;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

public class LogManager : MonoBehaviour
{
    public static LogManager Instance;

    [field: SerializeField] public BetterLoggingSettingsSO Settings { get; private set; }

    [Space]

    List<LogData> totalLogs = new();
    [SerializeField, ReadOnlyInspector] List<LogData> queuedLogs;
    [SerializeField, ReadOnlyInspector] List<Log> logs = new();

    [Space]

    [SerializeField] GameObject logConsole;
    [SerializeField] Scrollbar logConsoleScrollbar;
    LogConsole logConsoleComponent;
    [SerializeField] GameObject logConsoleContents;
    [SerializeField] ScrollRect logConsoleScrollRect;
    [SerializeField] Transform SliderBottomPOS;
    [SerializeField] GameObject logPrefab;
    [SerializeField] GameObject bottomOfListprefab;

    [Space]

    [SerializeField] SetupMessage setupMessage;
    [SerializeField] GameObject errorMessageMenu;
    [SerializeField] TMP_Text errorText;

    int logIndex;

    Transform bottomListTransform;

    void Awake()
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

        for (int i = 0; i < Settings.logCap; i++)
        {
            Log log = Instantiate(logPrefab, logConsoleContents.transform).GetComponent<Log>();

            log.Init(Settings.fontSize);

            logs.Add(log);
        }

        bottomListTransform = Instantiate(bottomOfListprefab, logConsoleContents.transform).transform;

        Application.logMessageReceivedThreaded += OnLogMessageReceived;
    }

    void Start()
    {
        InputManager.playerInputActions.General.OpenLogMenu.performed += OpenLogMenu_performed;
        SceneManager.sceneLoaded += OnNewSceneLoaded;
    }

    void OnDisable()
    {
        Application.logMessageReceivedThreaded -= OnLogMessageReceived;
    }

    void OnLogMessageReceived(string _condition, string stackTrace, LogType type)
    {
        string condition = "[" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "] [" + type + "] " + _condition;

        if (totalLogs.Count >= Settings.logCap)
            totalLogs.RemoveAt(0);
        totalLogs.Add(new LogData(condition, stackTrace, type));

        UnityMainThread.wkr.AddJob(() =>
        {
            // Will run on main thread
            if (!logConsole.activeSelf)
            {
                if (queuedLogs.Count >= Settings.logCap)
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
                    LogException(condition, stackTrace);
                    break;
            }
        });

    }

    void OpenLogMenu_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        ToggleLogMenu();
    }

    public void SetFirstTimeSetupMessage(bool isActive)
    {
        setupMessage.SetSetupMessage(isActive);
    }

    void LogBaseInfo(string message)
    {
        Log log = Instantiate(logPrefab, logConsoleContents.transform).GetComponent<Log>();

        log.SetupBaseInfoLog(message);

        totalLogs.Add(new LogData(message, "", LogType.Log));
    }
    void Log(string logMessage, string logDetails = "")
    {
        bool isAtBottom;

        if (SliderBottomPOS.position.y <= bottomListTransform.position.y)
            isAtBottom = true;
        else
            isAtBottom = false;

        bool logsfull = CheckLogCap();

        if (logsfull)
            logs[^1].SetupLog(logMessage, logDetails, LogType.Log);
        else
        {
            logs[logIndex].SetupLog(logMessage, logDetails, LogType.Log);
            logIndex++;

            if (isAtBottom)
                GoToBottom();
        }
    }
    void LogWarning(string warningMessage, string warningDetails = "")
    {
        bool isAtBottom;

        if (SliderBottomPOS.position.y <= bottomListTransform.position.y)
            isAtBottom = true;
        else
            isAtBottom = false;

        bool logsfull = CheckLogCap();

        if (logsfull)
            logs[^1].SetupLog(warningMessage, warningDetails, LogType.Warning);
        else
        {
            logs[logIndex].SetupLog(warningMessage, warningDetails, LogType.Warning);
            logIndex++;

            if (isAtBottom)
                GoToBottom();
        }
    }
    void LogError(string errorMessage, string errorDetails = "")
    {
        bool isAtBottom;

        if (SliderBottomPOS.position.y <= bottomListTransform.position.y)
            isAtBottom = true;
        else
            isAtBottom = false;

        bool logsfull = CheckLogCap();

        if (logsfull)
            logs[^1].SetupLog(errorMessage, errorDetails, LogType.Error);
        else
        {
            logs[logIndex].SetupLog(errorMessage, errorDetails, LogType.Error);
            logIndex++;

            if (isAtBottom)
                GoToBottom();
        }
    }
    void LogException(string exception, string exceptionDetails = "")
    {
        bool isAtBottom;

        if (SliderBottomPOS.position.y <= bottomListTransform.position.y)
            isAtBottom = true;
        else
            isAtBottom = false;

        bool logsfull = CheckLogCap();

        if (logsfull)
            logs[^1].SetupLog(exception, exceptionDetails, LogType.Exception);
        else
        {
            logs[logIndex].SetupLog(exception, exceptionDetails, LogType.Exception);
            logIndex++;

            if (isAtBottom)
                GoToBottom();
        }
    }

    async void GoToBottom()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(logConsole.GetComponent<RectTransform>());
        await UniTask.WaitForEndOfFrame(this);
        logConsoleScrollRect.ScrollToBottom();
    }
    bool CheckLogCap()
    {
        if (logIndex >= Settings.logCap)
        {
            logs[0].transform.SetSiblingIndex(logs.Count + 5);

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
        logConsole.SetActive(!logConsole.activeSelf);
    }

    void OnLogConsoleEnabled(object sender, EventArgs e)
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
                    LogException(queuedLogs[i].condition, queuedLogs[i].stackTrace);
                    break;
            }
        }

        queuedLogs.Clear();

        logConsoleScrollRect.ScrollToBottom();
    }

    void OnNewSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (Settings.clearConsoleOnSceneChange)
            ClearConsole();
    }

    public void ClearConsole()
    {
        queuedLogs.Clear();

        foreach (var log in logs)
        {
            log.gameObject.SetActive(false);
        }
    }

    void OnApplicationQuit()
    {
        if (Settings.logCap <= 0) return;

        string logsFolder = Application.persistentDataPath + "/logs";

        if(!Directory.Exists(logsFolder))
            Directory.CreateDirectory(logsFolder);

        DirectoryInfo d = new DirectoryInfo(logsFolder);

        List<FileInfo> logFiles = new();
        foreach (var logFile in d.GetFiles("*.txt"))
        {
                logFiles.Add(logFile);
        }

        
        while (logFiles.Count >= Settings.logFileCap)
        {
            logFiles[0].Delete();
            logFiles.RemoveAt(0);
        }

        TextWriter tw = new StreamWriter(Application.persistentDataPath + "/logs/log" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".txt");

        tw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + " / " + DateTime.Now);

        foreach (LogData log in totalLogs)
        {
            tw.WriteLine(log.condition + "\n" + log.stackTrace);
        }

        tw.Close();
    }

    [Serializable]
    class LogData
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
