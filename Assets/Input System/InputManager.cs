using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static PlayerInputActions playerInputActions;

    LogManager logManager;
    private void Awake()
    {
        playerInputActions?.Dispose();

        playerInputActions = new PlayerInputActions();
        playerInputActions.Enable();

        logManager = LogManager.Instance;

        playerInputActions.General.OpenLogMenu.performed += OpenLogMenu_performed;
    }

    private void OpenLogMenu_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) => logManager.ToggleConsole();
}
