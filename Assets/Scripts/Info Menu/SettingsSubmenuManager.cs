using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsSubmenuManager : MonoBehaviour
{
    [SerializeField] PGManager pgManager;

    public void OpenSettingsSubmenu()
    {
        UIManager.OpenSubMenu(gameObject);
    }
}
