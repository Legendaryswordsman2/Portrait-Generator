using Cysharp.Threading.Tasks;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIObjectHelper : MonoBehaviour
{
    [SerializeField] TMP_Text text;

    Selectable uiObject;

    Color defaultTextColor;

    private void Awake()
    {
        uiObject = GetComponent<Selectable>();

        defaultTextColor = text.color;

        if(uiObject.interactable)
        WaitUntilDropdownDisabled();
        else
            WaitUntilDropdownEnabled();
    }

    async void WaitUntilDropdownEnabled()
    {
        await UniTask.WaitUntil(() => uiObject.interactable == true);

        text.color = defaultTextColor;

        WaitUntilDropdownDisabled();
    }

    async void WaitUntilDropdownDisabled()
    {
        await UniTask.WaitUntil(() => uiObject.interactable == false);

        Color disabledColor = defaultTextColor;
        disabledColor.a = uiObject.colors.disabledColor.a;
        text.color = disabledColor;

        WaitUntilDropdownEnabled();
    }
}
