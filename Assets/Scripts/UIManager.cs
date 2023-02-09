using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UIManager
{
  public static GameObject ActiveMenu { get; private set; }

    public static bool OpenMenu(GameObject menuToOpen)
    {
        if (ActiveMenu != null) return false;

        ActiveMenu= menuToOpen;

        ActiveMenu.SetActive(true);

        return true;
    }

    public static bool CloseMenu(GameObject menuToClose)
    {
        if(menuToClose == null)
        {
            Debug.LogError("Can't close an overlay that is null");

            return false;
        }

        if(menuToClose != ActiveMenu)
        {
            Debug.LogWarning($"The overlay '{menuToClose}' you're trying to close is already closed or has not been opened using the game manager");
            return false;
        }

        ActiveMenu.SetActive(false);

        return true;
    }

    public static bool ForceCloseOverlay()
    {
        if(ActiveMenu == null)
        {
            Debug.Log("Can't force close menu, no actie menu set");
            return false;
        }

        ActiveMenu.SetActive(false);

        ActiveMenu = null;

        return true;
    }
}
