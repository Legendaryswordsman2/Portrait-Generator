using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SavePortraitManager : MonoBehaviour
{
    [SerializeField] TMP_InputField fileNameInputField;
    [SerializeField] TMP_Dropdown sizeDropdown;
    [SerializeField] Button saveButton;

    [Space]

    [SerializeField] PortraitPieceMerger ppMerger;
    [SerializeField] GameObject savePortraitMenus;
    [SerializeField] GameObject creatingPortraitOverlay;

    Sprite finalSprite;

    PortraitSize size;

    public void OpenSavePortraitMenu()
    {
        fileNameInputField.interactable = true;
        sizeDropdown.interactable = true;
        saveButton.interactable = true;

        UIManager.OpenMenu(savePortraitMenus);

        gameObject.SetActive(true);
    }

    public async void SavePortrait()
    {
        fileNameInputField.interactable = false;
        sizeDropdown.interactable = false;
        saveButton.interactable = false;

        creatingPortraitOverlay.SetActive(true);

        switch (sizeDropdown.value)
        {
            case 0:
                size = PortraitSize.Sixteen;
                break;
            case 1:
                size = PortraitSize.Thirtytwo;
                break;
            case 2:
                size = PortraitSize.Fortyeight;
                break;
        }
        finalSprite = await ppMerger.CombinePortraitPieces(size);

        SavePortraitToFile();
    }

    void SavePortraitToFile()
    {
        byte[] bytes = finalSprite.texture.EncodeToPNG();

        if (!Directory.Exists(Directory.GetCurrentDirectory() + "/Saved Portraits"))
            Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/Saved Portraits");

        if (fileNameInputField.text == "")
            fileNameInputField.text = "Unnamed Portrait";

        File.WriteAllBytes(Directory.GetCurrentDirectory() + "/Saved Portraits/" + fileNameInputField.text + ".png", bytes);
    }
}
