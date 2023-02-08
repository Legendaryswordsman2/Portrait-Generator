using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class SavePortraitManager : MonoBehaviour
{
    [SerializeField] TMP_InputField fileNameInputField;
    [SerializeField] TMP_Dropdown sizeDropdown;

    [Space]

    [SerializeField] PortraitPieceMerger ppMerger;

    Sprite finalSprite;

    PortraitSize size;

    public async void SavePortrait()
    {
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

        if (!Directory.Exists(Directory.GetCurrentDirectory() + "/Saved Characters"))
            Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/Saved Characters");

        if (fileNameInputField.text == "")
            fileNameInputField.text = "Unnamed Character";

        File.WriteAllBytes(Directory.GetCurrentDirectory() + "/Saved Characters/" + fileNameInputField.text + ".png", bytes);
    }
}
