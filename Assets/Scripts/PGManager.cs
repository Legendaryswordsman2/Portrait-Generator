using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class PGManager : MonoBehaviour
{
    public PortraitPiece skin;
    public PortraitPiece hair;

    public event EventHandler OnSkinChanged;

    public event EventHandler OnHairstyleChanged;

    public event EventHandler OnDropdownChanged;

    [Space]

    [SerializeField] SetupMessage setupMessage;

    PortraitPieceGrabber ppg;

    public static bool finishedSetup { get; private set; } = false;

    int index;

    private async void Awake()
    {
        ppg = GetComponent<PortraitPieceGrabber>();

        await UniTask.WaitUntil(() => ppg.finishedSetup == true);

        SetPortraitPartDropdown(skin);

        SetPortraitPartDropdown(hair);

        OnSkinDropdownChanged(0);
        skin.dropdown.RefreshShownValue();

        OnHairstyleDropdownChanged(0);
        hair.dropdown.RefreshShownValue();

        finishedSetup = true;

    }

    //private void Start()
    //{
    //    //Debug.Log("Testing");
    //    //Debug.LogWarning("Warnign Test");
    //    //Debug.LogError("Error Tust");
    //    //Debug.LogException(new Exception("Excetion"));
    //    Timer();
    //}

    //async void Timer()
    //{
    //    //await UniTask.Delay(1000);

    //    //Debug.LogError("Testing: " + index);

    //    //index++;

    //    //Timer();
    //}

    private void Update()
    {
        //Debug.LogWarning("Serious Warning");
        //Debug.Log("Testing: " + index);

        //index++;
        //Debug.LogWarning("This is a bad warning");

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
                skin.sprites.Add(portraitPiece);
                break;
            case PortraitPieceType.Hairstyle:
                hair.sprites.Add(portraitPiece);
                break;
        }
    }

    void SetPortraitPartDropdown(PortraitPiece portraitPiece)
    {
        portraitPiece.dropdown.ClearOptions();

        for (int i = 0; i < portraitPiece.sprites.Count; i++)
        {
            portraitPiece.dropdown.options.Add(new TMP_Dropdown.OptionData() { text = portraitPiece.sprites[i].name });
        }

        portraitPiece.dropdown.value = 0;
    }

    public void OnSkinDropdownChanged(int index)
    {
        skin.imageComponent.sprite = skin.sprites[index];
        skin.activeSprite = skin.sprites[index];
        skin.activeSpriteIndex = index;

        OnSkinChanged?.Invoke(this, null);

        OnDropdownChanged?.Invoke(this, null);
    }

    public void OnHairstyleDropdownChanged(int index)
    {
        hair.imageComponent.sprite = hair.sprites[index];
        hair.activeSprite = hair.sprites[index];
        hair.activeSpriteIndex = index;

        if (!finishedSetup) return;

        OnHairstyleChanged?.Invoke(this, null);

        OnDropdownChanged?.Invoke(this, null);
    }

    [System.Serializable]
  public class PortraitPiece
    {
        public string name;
        public TMP_Dropdown dropdown;
        [ReadOnlyInspector] public Sprite activeSprite;
        [ReadOnlyInspector] public int activeSpriteIndex;
        public List<Sprite> sprites;
        public Image imageComponent;
    }
}
