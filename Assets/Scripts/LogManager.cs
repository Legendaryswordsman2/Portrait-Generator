using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.Build;
using UnityEngine;

public class LogManager : MonoBehaviour
{
    public static LogManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    [SerializeField] GameObject firstTimeSetupMessage;
    public void SetFirstTimeSetupMessage(bool isActive)
    {
        firstTimeSetupMessage.SetActive(isActive);
    }
}
