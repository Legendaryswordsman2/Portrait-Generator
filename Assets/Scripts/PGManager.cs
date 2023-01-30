using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class PGManager : MonoBehaviour
{
    public PortraitPiece skin;
    public PortraitPiece hair;

    PortraitPieceGrabber ppg;

    private async void Awake()
    {
        ppg = GetComponent<PortraitPieceGrabber>();

        await UniTask.WaitUntil(() => ppg.finishedSetup == true);

        SetPortraitPartDropdown(skin);

        SetPortraitPartDropdown(hair);

        OnSkinChanged(0);
        skin.dropdown.RefreshShownValue();

        OnHairstyleChanged(0);
        hair.dropdown.RefreshShownValue();
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

    public void OnSkinChanged(int index)
    {
        skin.imageComponent.sprite = skin.sprites[index];
        skin.activeSpriteIndex = index;
    }

    public void OnHairstyleChanged(int index)
    {
        hair.imageComponent.sprite = hair.sprites[index];
        hair.activeSpriteIndex = index;
    }

    [System.Serializable]
  public class PortraitPiece
    {
        public string name;
        public TMP_Dropdown dropdown;
        [ReadOnlyInspector] public int activeSpriteIndex;
        public List<Sprite> sprites;
        public Image imageComponent;
    }
}
