using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
 public static InputManager Instance { get; private set; }

    public static PlayerInputActions playerInputActions;

    private void Awake()
    {
        Instance = this;

        playerInputActions?.Dispose();

        playerInputActions = new PlayerInputActions();
        playerInputActions.General.Enable();
    }
}
