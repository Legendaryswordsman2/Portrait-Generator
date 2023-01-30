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
    private void Awake()
    {
        Instance = this;

        LogBaseInfo("Unity version: " + Application.unityVersion);

        LogBaseInfo("OS: " + SystemInfo.operatingSystem + " (" + SystemInfo.operatingSystemFamily + ")");

        LogBaseInfo("GPU: " + SystemInfo.graphicsDeviceName + " (Running " + SystemInfo.graphicsDeviceType + ")");

        LogBaseInfo("CPU: " + SystemInfo.processorType + " (" + SystemInfo.processorCount + " X " + SystemInfo.processorFrequency + " Mhz)");

        LogBaseInfo("RAM: " + SystemInfo.systemMemorySize + " MB");

        LogBaseInfo("Current Directory: " + Directory.GetCurrentDirectory());
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
        TMP_Text log = Instantiate(logPrefab, logMenuContents.transform).GetComponent<TMP_Text>();

        log.text = message;
    }

    public void LogMessage(string logMessage)
    {
        TMP_Text log = Instantiate(logPrefab, logMenuContents.transform).GetComponent<TMP_Text>();

        log.text = "[" + DateTime.Now + "] [Log] " + logMessage;
    }

    public void LogWarning(string warningMessage)
    {
        TMP_Text log = Instantiate(logPrefab, logMenuContents.transform).GetComponent<TMP_Text>();

        log.text = "[Warning]:" + warningMessage;

        log.color = Color.yellow;
    }

    public void LogError(string errorMessage)
    {
        errorText.text = errorMessage;

        errorMessageMenu.SetActive(true);

        TMP_Text log = Instantiate(logPrefab, logMenuContents.transform).GetComponent<TMP_Text>();

        log.text = "[Error]:" + errorMessage;

        log.color = Color.red;
    }

    void ToggleLogMenu()
    {
        logMenu.SetActive(!logMenu.activeSelf);
    }
}
