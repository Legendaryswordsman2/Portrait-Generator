using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpSubmenuManager : MonoBehaviour
{
    [SerializeField] GameObject[] pages;

    int index;

    public void OpenHelpSubmenu()
    {
        pages[index].SetActive(false);
        pages[0].SetActive(true);

        UIManager.OpenSubMenu(gameObject);
    }

    private void OnEnable()
    {
        InputManager.playerInputActions.HelpMenu.NextPage.performed += NextPage_performed;
        InputManager.playerInputActions.HelpMenu.PreviousPage.performed += PreviousPage_performed;
    }


    private void OnDisable()
    {
        InputManager.playerInputActions.HelpMenu.NextPage.performed -= NextPage_performed;
        InputManager.playerInputActions.HelpMenu.PreviousPage.performed -= PreviousPage_performed;
    }

    private void NextPage_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) => NextPage();
    private void PreviousPage_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) => PrevPage();
    public void NextPage()
    {
        if (index == pages.Length - 1) return;

        pages[index].SetActive(false);
        pages[index + 1].SetActive(true);

        index++;
    }

    public void PrevPage()
    {
        if (index == 0) return;

        pages[index].SetActive(false);
        pages[index - 1].SetActive(true);

        index--;
    }
    public void JoinDiscord()
    {
        Application.OpenURL("https://discord.com/invite/2wB3RuAESb");
    }
}
