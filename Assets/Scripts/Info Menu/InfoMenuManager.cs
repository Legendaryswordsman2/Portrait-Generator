using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoMenuManager : MonoBehaviour
{
    [SerializeField] GameObject baseInfoMenu;
    public void OpenInfoMenu()
    {
        UIManager.OpenMenu(gameObject);
        UIManager.OpenSubMenu(baseInfoMenu);
    }

    public void OpenGithubPage()
    {
        Application.OpenURL("https://github.com/Legendaryswordsman2/Portrait-Generator");
    }

    public void OpenMyOtherWorkPage()
    {
        Application.OpenURL("https://linktr.ee/legendaryswordsman2");
    }
}
