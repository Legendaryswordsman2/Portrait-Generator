using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.Build;
using UnityEngine;
using TMPro;

public class LogManager : MonoBehaviour
{
    public static LogManager Instance;

    [SerializeField] GameObject firstTimeSetupMessageMenu;

    [Space]

    [SerializeField] GameObject errorMessageMenu;
    [SerializeField] TMP_Text errorText;
    private void Awake()
    {
        Instance = this;
    }

    public void SetFirstTimeSetupMessage(bool isActive)
    {
        firstTimeSetupMessageMenu.SetActive(isActive);
    }

    public void LogError(string errorMessage)
    {
        errorText.text = errorMessage;

        errorMessageMenu.SetActive(true);
    }
}
