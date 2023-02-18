using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
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

        List<Action<int>> dropdownChangedMethods = new()
        {
            OnSkinDropdownChanged,
            OnHairstyleDropdownChanged,
            OnEyesDropdownChanged,
            OnAccessoriesDropdownChanged
        };

        for (int i = 0; i < portraitPieces.Length; i++)
        {
            portraitPieces[i].OnDropdownChangedMethod = DropdownChanged;
            portraitPieces[i].index = i;
            if (portraitPieces[i].sprites.Count == 0)
            {
                portraitPieces[i].dropdown.interactable = false;
                portraitPieces[i].imageComponent.enabled = false;
                continue;
            }

            if (portraitPieces[i].includeNAOption && !portraitPieces[i].NAOptionSelectedDefault)
                portraitPieces[i].dropdown.value = 1;

            dropdownChangedMethods[i](portraitPieces[i].dropdown.value);
            portraitPieces[i].dropdown.RefreshShownValue();

        }

        finishedSetup = true;
    }

    public void SetFirstTimeSetupMessage(bool isActive)
    {
        setupMessage.SetSetupMessage(isActive);
    }

    public void RandomizePortrait()
    {
        for (int i = 0; i < portraitPieces.Length; i++)
        {
            portraitPieces[i].Randomize();
        }
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

    void DropdownChanged(int portraitPieceIndex, int dropdownIndex, bool triggerEvent = true)
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

        if (triggerEvent)
        {
            portraitPieces[3].InvokeOnActivePortraitPieceChanged();
            OnDropdownChanged?.Invoke(this, null);
        }
    }

    [Serializable]
    public class PortraitPiece
    {
        public string name;
        public TMP_Dropdown dropdown;
        [Min(0)]
        public bool includeNAOption = false;
        public bool NAOptionSelectedDefault = true;
        [ReadOnlyInspector] public Sprite activeSprite;
        [ReadOnlyInspector] public int activeSpriteIndex;
        public List<Sprite> sprites;
        public Image imageComponent;

        public event EventHandler OnActivePortraitPieceChanged;

        public Action<int, int, bool> OnDropdownChangedMethod;
        public int index;

        public void Randomize()
        {
            if (sprites.Count == 0) return;

            int numb = UnityEngine.Random.Range(0, sprites.Count);

            dropdown.value = numb;
            //dropdown.RefreshShownValue();
            //OnDropdownChangedMethod(index, numb, false);
        }

        public void InvokeOnActivePortraitPieceChanged()
        {
            OnActivePortraitPieceChanged?.Invoke(this, null);
        }
    }
}
