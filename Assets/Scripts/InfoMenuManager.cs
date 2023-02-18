using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoMenuManager : MonoBehaviour
{
    public void OpenInfoMenu()
    {
        UIManager.OpenMenu(gameObject);
    }
}
