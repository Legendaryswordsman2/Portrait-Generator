using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class DropdownHelper : MonoBehaviour
{
    [SerializeField] TMP_Dropdown dropdown;

    [SerializeField] EventSystem eventSystem;

    private void Awake()
    {
        dropdown.onValueChanged.AddListener(Test);
    }

    void Test(int index)
    {
        Debug.Log("Test: " + index);
        eventSystem.SetSelectedGameObject(null);
    }
}
