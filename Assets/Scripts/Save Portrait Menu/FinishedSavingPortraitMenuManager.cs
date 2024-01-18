using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishedSavingPortraitMenuManager : MonoBehaviour
{
    bool canClose = false;
    private void OnEnable()
    {
        canClose = false;
        UIManager.OnBeforeUIClosed += UIManager_OnBeforeUIClosed;
    }

    private void OnDisable()
    {
        UIManager.OnBeforeUIClosed -= UIManager_OnBeforeUIClosed;
    }

    private void UIManager_OnBeforeUIClosed(object sender, EventArgs e)
    {
        if (canClose) return;

        UIManager.CanCloseUI = false;
        canClose = false;

        LeanTween.cancel(gameObject);

        LeanTween.scale(gameObject, Vector2.zero, 0.1f).setOnComplete(() =>
        {
            UIManager.CanCloseUI = true;
            canClose = true;
            UIManager.CloseMenu();
        });
    }
}