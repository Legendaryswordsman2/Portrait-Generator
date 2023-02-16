using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class PGManager : MonoBehaviour
{
    public PortraitPiece[] portraitPieces;

    public event EventHandler OnDropdownChanged;

    [Space]

    [SerializeField] SetupMessage setupMessage;

    PortraitPieceGrabber ppg;

    public static bool finishedSetup { get; private set; } = false;

    private async void Awake()
    {
        ppg = GetComponent<PortraitPieceGrabber>();

        await UniTask.WaitUntil(() => ppg.finishedSetup == true);

        foreach (PortraitPiece portraitPiece in portraitPieces)
        {
            SetPortraitPartDropdown(portraitPiece);
        }

        //SetPortraitPartDropdown(portraitPieces[0]);

        //SetPortraitPartDropdown(hair);

        //OnSkinDropdownChanged(0);

        List<Action<int>> dropdownChangedMethods = new()
        {
            OnSkinDropdownChanged,
            OnHairstyleDropdownChanged,
            OnEyesDropdownChanged,
            OnAccessoriesDropdownChanged
        };

        for (int i = 0; i < portraitPieces.Length; i++)
        {
            if (portraitPieces[i].defaultDropdownIndex > portraitPieces[i].sprites.Count - 1)
            {
                Debug.Log("(" + portraitPieces[i].name + ") Default dropdown index was outside of range of sprites, setting index to sprite count");
                portraitPieces[i].defaultDropdownIndex = portraitPieces[i].sprites.Count - 1;
            }

            portraitPieces[i].dropdown.value = portraitPieces[i].defaultDropdownIndex;
            dropdownChangedMethods[i](portraitPieces[i].defaultDropdownIndex);
            portraitPieces[i].dropdown.RefreshShownValue();

        }

        //OnHairstyleDropdownChanged(0);
        //portraitPieces[1].dropdown.RefreshShownValue();

        //OnEyesDropdownChanged(0);
        //portraitPieces[2].dropdown.RefreshShownValue();

        //OnAccessoriesDropdownChanged(0);
        //portraitPieces[3].dropdown.RefreshShownValue();

        finishedSetup = true;

    }

    public void SetFirstTimeSetupMessage(bool isActive)
    {
        setupMessage.SetSetupMessage(isActive);
    }

    public void AddPortraitPiece(Sprite portraitPiece, PortraitPieceType type)
    {
        switch (type)
        {
            case PortraitPieceType.Skin:
                portraitPieces[0].sprites.Add(portraitPiece);
                break;
            case PortraitPieceType.Hairstyle:
                portraitPieces[1].sprites.Add(portraitPiece);
                break;
            case PortraitPieceType.Eyes:
                portraitPieces[2].sprites.Add(portraitPiece);
                break;
            case PortraitPieceType.Accessory:
                portraitPieces[3].sprites.Add(portraitPiece);
                break;
        }
    }

    void SetPortraitPartDropdown(PortraitPiece portraitPiece)
    {
        portraitPiece.dropdown.ClearOptions();

        if (portraitPiece.includeNAOption)
        {
            portraitPiece.dropdown.options.Add(new TMP_Dropdown.OptionData() { text = portraitPiece.name + " N/A" });
        }

        for (int i = 0; i < portraitPiece.sprites.Count; i++)
        {
            portraitPiece.dropdown.options.Add(new TMP_Dropdown.OptionData() { text = portraitPiece.sprites[i].name });
        }

        portraitPiece.dropdown.value = 0;
    }

    public void OnSkinDropdownChanged(int index)
    {
        DropdownChanged(0, index);
    }

    public void OnHairstyleDropdownChanged(int index)
    {
        DropdownChanged(1, index);
    }

    public void OnEyesDropdownChanged(int index)
    {
        DropdownChanged(2, index);
    }

    public void OnAccessoriesDropdownChanged(int index)
    {
        DropdownChanged(3, index);
    }

    void DropdownChanged(int portraitPieceIndex, int dropdownIndex)
    {
        if (portraitPieces[portraitPieceIndex].includeNAOption)
        {
            if (dropdownIndex == 0)
            {
                portraitPieces[portraitPieceIndex].imageComponent.enabled = false;
                portraitPieces[portraitPieceIndex].activeSprite = null;
                portraitPieces[portraitPieceIndex].activeSpriteIndex = -1;
            }
            else
            {
                portraitPieces[portraitPieceIndex].imageComponent.enabled = true;
                portraitPieces[portraitPieceIndex].imageComponent.sprite = portraitPieces[portraitPieceIndex].sprites[dropdownIndex - 1];
                portraitPieces[portraitPieceIndex].activeSprite = portraitPieces[portraitPieceIndex].sprites[dropdownIndex - 1];
                portraitPieces[portraitPieceIndex].activeSpriteIndex = dropdownIndex - 1;
            }
        }
        else
        {
            portraitPieces[portraitPieceIndex].imageComponent.enabled = true;
            portraitPieces[portraitPieceIndex].imageComponent.sprite = portraitPieces[portraitPieceIndex].sprites[dropdownIndex];
            portraitPieces[portraitPieceIndex].activeSprite = portraitPieces[portraitPieceIndex].sprites[dropdownIndex];
            portraitPieces[portraitPieceIndex].activeSpriteIndex = dropdownIndex;
        }

        portraitPieces[3].InvokeOnActivePortraitPieceChanged();
        OnDropdownChanged?.Invoke(this, null);
    }

    [Serializable]
    public class PortraitPiece
    {
        public string name;
        public TMP_Dropdown dropdown;
        [Min(0)]
        public int defaultDropdownIndex;
        public bool includeNAOption = false;
        public bool NaOptionSelectedDefault = true;
        [ReadOnlyInspector] public Sprite activeSprite;
        [ReadOnlyInspector] public int activeSpriteIndex;
        public List<Sprite> sprites;
        public Image imageComponent;

        public event EventHandler OnActivePortraitPieceChanged;

        public void InvokeOnActivePortraitPieceChanged()
        {
            OnActivePortraitPieceChanged?.Invoke(this, null);
        }
    }
}
