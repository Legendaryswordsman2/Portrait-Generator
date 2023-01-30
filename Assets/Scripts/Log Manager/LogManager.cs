using UnityEngine;
using TMPro;

public class LogManager : MonoBehaviour
{
    public static LogManager Instance;

    [SerializeField] SetupMessage setupMessage;

    [Space]

    [SerializeField] GameObject errorMessageMenu;
    [SerializeField] TMP_Text errorText;

    [SerializeField] GameObject logMenu;
    private void Awake()
    {
        Instance = this;
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

    public void LogError(string errorMessage)
    {
        errorText.text = errorMessage;

        errorMessageMenu.SetActive(true);
    }

    void ToggleLogMenu()
    {
        logMenu.SetActive(!logMenu.activeSelf);
    }
}
