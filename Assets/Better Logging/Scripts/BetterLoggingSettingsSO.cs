using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Better Logging Settings")]
public class BetterLoggingSettingsSO : ScriptableObject
{
    [Header("Settings")]
    [Tooltip("The max amount of logs that can be displayed at a given time, once the limit has been reached logs will begin getting replaced, the higher the number of logs the laggier the game")]
    [Range(1, 1000)] public int logCap = 250;
    [Tooltip("The font size of each log in the console")]
    [Min(1)] public float fontSize = 25;
    [Tooltip("The max amount of log files that can be generated, once this limit has been reached older logs will start being overwritten"), Min(0)]
    public int logFileCap = 5;

    [Tooltip("Clears all logs when the current scene changes")]
    public bool clearConsoleOnSceneChange = true;
}
