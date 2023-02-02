using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogConsole : MonoBehaviour
{
    public event EventHandler OnEnabled;

    private void OnEnable()
    {
        OnEnabled?.Invoke(this, EventArgs.Empty);
    }
}
