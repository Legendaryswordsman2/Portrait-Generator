using Cysharp.Threading.Tasks;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIObjectHelper : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] bool deselectOnPressed = true;

    [Space]

    [SerializeField] TMP_Text text;
    [SerializeField] Image dropdownArrow;

    Selectable uiObject;

    Color defaultTextColor;

    [field: SerializeField, ReadOnlyInspector] public bool selected { get; private set; } = false;
    private void Awake()
    {
        uiObject = GetComponent<Selectable>();

        if (text != null)
            defaultTextColor = text.color;

        if (uiObject.interactable)
            WaitUntilDropdownDisabled();
        else
            WaitUntilDropdownEnabled();
    }

    public void OnSelect(BaseEventData eventData)
    {
        selected = true;
        if (deselectOnPressed)
            NavigationHelper.DeselectSelectedUIObject();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        //Debug.Log(dropdown.IsExpanded);
        if (uiObject is TMP_Dropdown dropdown)
        {
            if (dropdown == null || !dropdown.IsExpanded)
                selected = false;
        }
        else
            selected = false;
    }

    async void WaitUntilDropdownEnabled()
    {
        await UniTask.WaitUntil(() => uiObject.interactable == true);

        if (text == null) return;

        text.color = defaultTextColor;
        if(dropdownArrow != null) dropdownArrow.color = Color.white;
        WaitUntilDropdownDisabled();
    }

    async void WaitUntilDropdownDisabled()
    {
        await UniTask.WaitUntil(() => uiObject.interactable == false);

        if (text == null) return;

        Color textDisabledColor = defaultTextColor;
        Color dropdownDisabledColor = Color.white;
        textDisabledColor.a = uiObject.colors.disabledColor.a;
        dropdownDisabledColor.a = uiObject.colors.disabledColor.a;
        text.color = textDisabledColor;
        if (dropdownArrow != null) dropdownArrow.color = dropdownDisabledColor;

        WaitUntilDropdownEnabled();
    }
}
