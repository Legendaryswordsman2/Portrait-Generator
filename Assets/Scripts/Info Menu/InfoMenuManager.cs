using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InfoMenuManager : MonoBehaviour, IUIClosed
{
    [SerializeField] GameObject baseInfoMenu;

    [SerializeField] GameObject background;

    bool canClose = false;
    public void OpenInfoMenu()
    {
        UIManager.OpenMenu(gameObject);
        UIManager.OpenSubMenu(baseInfoMenu);

        LeanTween.cancel(gameObject);
        transform.localScale = Vector2.zero;

        LeanTween.scale(gameObject, Vector2.one, 0.1f);

        canClose = false;
    }

    public void OpenGithubPage()
    {
        Application.OpenURL("https://github.com/Legendaryswordsman2/Portrait-Generator");
    }

    public void OverviewPage()
    {
        Application.OpenURL("https://legendaryswordsman2.itch.io/portrait-generator");
    }

    private void OnEnable()
    {
        background.SetActive(true);

        //UIManager.OnBeforeUIClosed += UIManager_OnBeforeUIClosed;
    }


    private void OnDisable()
    {
        background.SetActive(false);

        //UIManager.OnBeforeUIClosed -= UIManager_OnBeforeUIClosed;
    }

    //private void UIManager_OnBeforeUIClosed(object sender, System.EventArgs e)
    //{
    //    if(canClose) return;

    //    UIManager.CanCloseUI = false;

    //    LeanTween.cancel(gameObject);

    //    LeanTween.scale(gameObject, Vector2.zero, 0.1f).setOnComplete(() =>
    //    {
    //        canClose = true;
    //        UIManager.CanCloseUI = true;
    //        UIManager.CloseMenu();
    //    });
    //}
    public bool OnUIClosed()
    {
        if (canClose) return true;

        LeanTween.cancel(gameObject);

        LeanTween.scale(gameObject, Vector2.zero, 0.1f).setOnComplete(() =>
        {
            canClose = true;
            UIManager.CloseMenu();
        });

        return false;
    }
}
