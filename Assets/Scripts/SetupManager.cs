using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ErrorType { MissingPortraitPiecesFolder}
public class SetupManager : MonoBehaviour
{
    public static SetupManager Instance;
    [SerializeField] GameObject[] errorMenus;

    private void Awake()
    {
        Instance = this;
    }
    public void DisplayError(ErrorType errorType)
    {
        switch (errorType)
        {
            case ErrorType.MissingPortraitPiecesFolder:
                errorMenus[0].SetActive(true);
                Debug.LogError("Portrait Pieces Folder Missing");
                break;
        }
    }
}
