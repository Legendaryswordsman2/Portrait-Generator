using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public static class UIManager
{
    public static GameObject ActiveMenu { get; private set; }

    public static List<GameObject> ActiveSubMenus { get; private set; } = new List<GameObject>();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void Awake() => InputManager.playerInputActions.General.Back.performed += Back_performed;

    private static void Back_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (SavePortraitManager.SavingPortrait == true) return;

        if (ActiveMenu == null)
            Application.Quit();
        else
            CloseMenu();
    }

    public static bool OpenMenu(GameObject menuToOpen)
    {
        if (ActiveMenu != null) return false;

        ActiveMenu = menuToOpen;

        ActiveMenu.SetActive(true);

        return true;
    }

    public static bool CloseMenu()
    {
        //if(menuToClose == null)
        //{
        //    Debug.LogError("Can't close an overlay that is null");

        //    return false;
        //}

        //if(menuToClose != ActiveMenu)
        //{
        //    Debug.LogWarning($"The overlay '{menuToClose}' you're trying to close is already closed or has not been opened using the game manager");
        //    return false;
        //}

        if(ActiveMenu == null) return false;

        Debug.Log(ActiveSubMenus.Count);
        if (ActiveSubMenus.Count == 0)
        {
            Debug.Log("No active sub menus, closing menu");
            ActiveMenu.SetActive(false);
            ActiveMenu = null;
            return true;
        }
        else
        {
            Debug.Log("Closing sub menu");
            ActiveSubMenus[^1].SetActive(false);
            ActiveSubMenus.RemoveAt(ActiveSubMenus.Count - 1);

            if (ActiveSubMenus.Count > 0)
                ActiveSubMenus[^1].SetActive(true);
            //else
            //    ActiveMenu.SetActive(true);

            return true;
        }

        //if(ActiveSubMenu != null && ActiveSubMenu.activeSelf)
        //{
        //    ActiveSubMenu.SetActive(false);
        //    ActiveSubMenu = null;
        //    ActiveMenu.SetActive(true);
        //    return true;
        //}
        //else
        //{
        //    ActiveMenu.SetActive(false);
        //    ActiveMenu = null;
        //    return true;
        //}

    }

    public static bool OpenSubMenu(GameObject submenuToOpen)
    {
        if(ActiveMenu == null)
        {
            Debug.LogError("Can't open a submenu when a menu isn't open");
            return false;
        }

        //ActiveMenu.SetActive(false);
        if(ActiveSubMenus.Count > 0)
        ActiveSubMenus[^1].SetActive(false);
        ActiveSubMenus.Add(submenuToOpen);
        submenuToOpen.SetActive(true);

        return true;
    }

    public static bool SwitchActiveMenu(GameObject oldMenu, GameObject newMenu)
    {
        if(ActiveMenu != oldMenu)
        {
            Debug.Log("Cannot switch active menu, supplied active menu is inccorect");
            return false;
        }

        ActiveMenu.SetActive(false);

        ActiveMenu = newMenu;

        ActiveMenu.SetActive(true);

        return true;
    }

    public static bool ForceCloseMenu()
    {
        if(ActiveMenu == null)
        {
            Debug.Log("Can't force close menu, no actie menu set");
            return false;
        }

        ActiveMenu.SetActive(false);
        if(ActiveSubMenus.Count > 0)
        {
            ActiveSubMenus[^1].SetActive(false);
            ActiveSubMenus.Clear();
        }

        ActiveMenu = null;

        return true;
    }
}
