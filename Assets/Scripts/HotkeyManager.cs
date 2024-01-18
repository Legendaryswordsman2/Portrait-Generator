using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotkeyManager : MonoBehaviour
{
    [SerializeField] RandomizeButtonManager randomizeButtonManager;
    [SerializeField] SavePortraitManager savePortraitManager;
    [SerializeField] InfoMenuManager infoMenuManager;

    PortraitPieceGrabber ppg;

    private async void Start()
    {
        ppg = GetComponent<PortraitPieceGrabber>();

        await UniTask.WaitUntil(() => ppg.FinishedSetup == true);

        InputManager.playerInputActions.General.RandomizePortrait.performed += RandomizePortrait_performed;
        InputManager.playerInputActions.General.SavePopup.performed += SavePopup_performed;
        InputManager.playerInputActions.General.InfoPopup.performed += InfoPopup_performed;
    }


    private void RandomizePortrait_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if(UIManager.ActiveMenu == null)
        randomizeButtonManager.RandomizePortrait();
    }

    private void SavePopup_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if(UIManager.ActiveMenu == null)
            savePortraitManager.OpenSavePortraitMenu();
    }

    private void InfoPopup_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (UIManager.ActiveMenu == null)
            infoMenuManager.OpenInfoMenu();
        else
            UIManager.CloseMenu();
    }

    private void OnDestroy()
    {
        InputManager.playerInputActions.General.RandomizePortrait.performed -= RandomizePortrait_performed;
        InputManager.playerInputActions.General.SavePopup.performed -= SavePopup_performed;
        InputManager.playerInputActions.General.InfoPopup.performed -= InfoPopup_performed;
    }
}