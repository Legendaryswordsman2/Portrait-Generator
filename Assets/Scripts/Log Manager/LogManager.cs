using UnityEngine;
using TMPro;

public class LogManager : MonoBehaviour
{
    public static LogManager Instance;

    [SerializeField] GameObject firstTimeSetupMessageMenu;
    [SerializeField] SetupMessage setupMessage;

    [Space]

    [SerializeField] GameObject errorMessageMenu;
    [SerializeField] TMP_Text errorText;
    private void Awake()
    {
        Instance = this;
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
}
