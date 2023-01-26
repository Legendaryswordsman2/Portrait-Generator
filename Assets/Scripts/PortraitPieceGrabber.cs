using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

enum PortraitPieceType { Skin, Hairstyle}
public class PortraitPieceGrabber : MonoBehaviour
{
    //[SerializeField] Image imageComponent;

    PortraitPieceDatabase ppd;
    private async void Awake()
    {
        LogManager.Instance.SetFirstTimeSetupMessage(true);

        ppd = GetComponent<PortraitPieceDatabase>();

        string filepath = Directory.GetCurrentDirectory() + "/Portrait Generator/Portrait_Generator - 16x16/Skins";
        DirectoryInfo d = new DirectoryInfo(filepath);

        var task = new Task[d.GetFiles("*.png").Length];
        foreach (var file in d.GetFiles("*.png"))
        {
            Debug.Log(file.Name);
            // file.FullName is the full path to the file
            //StartCoroutine(GetImage(file.FullName, file.Name, PortraitPieceType.Skin));
            await GetImage(file.FullName, file.Name, PortraitPieceType.Skin);
        }

        LogManager.Instance.SetFirstTimeSetupMessage(false);
    }
    //IEnumerator GetImage(string filepath, string fileName, PortraitPieceType type)
    //{
    //    using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(filepath))
    //    {
    //        yield return uwr.SendWebRequest();

    //        if (uwr.result != UnityWebRequest.Result.Success)
    //            Debug.Log(uwr.error);
    //        else
    //        {
    //            // Get downloaded asset bundle
    //            Texture2D texture = DownloadHandlerTexture.GetContent(uwr);

    //            Sprite sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 16f);

    //            sprite.name = fileName;
    //            sprite.texture.filterMode= FilterMode.Point;

    //            AddPortraitPieceToDatabase(sprite, type);
    //        }
    //    }
    //}

    async Task GetImage(string filepath, string fileName, PortraitPieceType type)
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(filepath))
        {
            await uwr.SendWebRequest();

            if (uwr.result != UnityWebRequest.Result.Success)
                Debug.Log(uwr.error);
            else
            {
                // Get downloaded asset bundle
                Texture2D texture = DownloadHandlerTexture.GetContent(uwr);

                Sprite sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 16f);

                sprite.name = fileName;
                sprite.texture.filterMode = FilterMode.Point;

                AddPortraitPieceToDatabase(sprite, type);
            }
        }
    }

    void AddPortraitPieceToDatabase(Sprite portraitPiece, PortraitPieceType type)
    {
        if(type == PortraitPieceType.Skin)
        {
            ppd.Skins.Add(portraitPiece);
        }
        else
        {
            ppd.Hairstyles.Add(portraitPiece);
        }
    }
}
