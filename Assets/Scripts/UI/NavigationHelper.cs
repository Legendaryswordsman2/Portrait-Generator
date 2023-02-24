using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NavigationHelper : MonoBehaviour
{
    [SerializeField] EventSystem eventSystem;

    static NavigationHelper instance;

    private void Awake()
    {
        instance = this;
    }

    public static void DeselectSelectedUIObject()
    {
        instance.DeselectSelectedUIObject_Private();
    }

    async void DeselectSelectedUIObject_Private()
    {
        await UniTask.WaitForEndOfFrame(this);
        eventSystem.SetSelectedGameObject(null);
    }
}
