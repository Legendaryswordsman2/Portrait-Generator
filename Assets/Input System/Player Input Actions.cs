//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.4.4
//     from Assets/Input System/Player Input Actions.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerInputActions : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Player Input Actions"",
    ""maps"": [
        {
            ""name"": ""General"",
            ""id"": ""042f867d-a93d-498e-b0f4-5f778769f2cf"",
            ""actions"": [
                {
                    ""name"": ""Open Log Menu"",
                    ""type"": ""Button"",
                    ""id"": ""d460b451-7dc0-4b1b-8c81-be5a847626a5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Test"",
                    ""type"": ""Button"",
                    ""id"": ""071a5369-9ec7-4353-8f74-c3a20a55b590"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""2649dd62-0759-4181-a688-c34898fd4d5c"",
                    ""path"": ""<Keyboard>/f1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Open Log Menu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c3e0d71a-cf39-4cb1-bce9-1a301f73249c"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Test"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard & Mouse"",
            ""bindingGroup"": ""Keyboard & Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // General
        m_General = asset.FindActionMap("General", throwIfNotFound: true);
        m_General_OpenLogMenu = m_General.FindAction("Open Log Menu", throwIfNotFound: true);
        m_General_Test = m_General.FindAction("Test", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // General
    private readonly InputActionMap m_General;
    private IGeneralActions m_GeneralActionsCallbackInterface;
    private readonly InputAction m_General_OpenLogMenu;
    private readonly InputAction m_General_Test;
    public struct GeneralActions
    {
        private @PlayerInputActions m_Wrapper;
        public GeneralActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @OpenLogMenu => m_Wrapper.m_General_OpenLogMenu;
        public InputAction @Test => m_Wrapper.m_General_Test;
        public InputActionMap Get() { return m_Wrapper.m_General; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GeneralActions set) { return set.Get(); }
        public void SetCallbacks(IGeneralActions instance)
        {
            if (m_Wrapper.m_GeneralActionsCallbackInterface != null)
            {
                @OpenLogMenu.started -= m_Wrapper.m_GeneralActionsCallbackInterface.OnOpenLogMenu;
                @OpenLogMenu.performed -= m_Wrapper.m_GeneralActionsCallbackInterface.OnOpenLogMenu;
                @OpenLogMenu.canceled -= m_Wrapper.m_GeneralActionsCallbackInterface.OnOpenLogMenu;
                @Test.started -= m_Wrapper.m_GeneralActionsCallbackInterface.OnTest;
                @Test.performed -= m_Wrapper.m_GeneralActionsCallbackInterface.OnTest;
                @Test.canceled -= m_Wrapper.m_GeneralActionsCallbackInterface.OnTest;
            }
            m_Wrapper.m_GeneralActionsCallbackInterface = instance;
            if (instance != null)
            {
                @OpenLogMenu.started += instance.OnOpenLogMenu;
                @OpenLogMenu.performed += instance.OnOpenLogMenu;
                @OpenLogMenu.canceled += instance.OnOpenLogMenu;
                @Test.started += instance.OnTest;
                @Test.performed += instance.OnTest;
                @Test.canceled += instance.OnTest;
            }
        }
    }
    public GeneralActions @General => new GeneralActions(this);
    private int m_KeyboardMouseSchemeIndex = -1;
    public InputControlScheme KeyboardMouseScheme
    {
        get
        {
            if (m_KeyboardMouseSchemeIndex == -1) m_KeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard & Mouse");
            return asset.controlSchemes[m_KeyboardMouseSchemeIndex];
        }
    }
    public interface IGeneralActions
    {
        void OnOpenLogMenu(InputAction.CallbackContext context);
        void OnTest(InputAction.CallbackContext context);
    }
}
