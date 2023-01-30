using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

enum PortraitPieceType { Skin, Hairstyle}
public class PortraitPieceGrabber : MonoBehaviour
{
    //[SerializeField] Image imageComponent;

    PortraitPieceDatabase ppd;
    private async void Awake()
    {
        LogManager.Instance.SetFirstTimeSetupMessage(true);

        ppd = GetComponent<PortraitPieceDatabase>();

        var tasks = new List<Task>();

        tasks.Add(GetSkinsForDisplay());
        tasks.Add(GetHairstylesForDisplay());

        // Wait for all portrait pieces to be added
        await Task.WhenAll(tasks);

        LogManager.Instance.SetFirstTimeSetupMessage(false);
    }

    async Task GetSkinsForDisplay()
    {

        string filepath = Directory.GetCurrentDirectory() + "/Portrait Pieces/Portrait_Generator - 16x16/Skins";

        if (!CheckDirectory(filepath)) return;

        DirectoryInfo d = new DirectoryInfo(filepath);

        foreach (var file in d.GetFiles("*.png"))
        {
            Debug.Log(file.Name);
            // file.FullName is the full path to the file
            await GetImage(file.FullName, file.Name, PortraitPieceType.Skin);
        }
    }

    async Task GetHairstylesForDisplay()
    {
        string filepath = Directory.GetCurrentDirectory() + "/Portrait Pieces/Portrait_Generator - 16x16/Hairstyles";

        if (!CheckDirectory(filepath)) return;

        DirectoryInfo d = new DirectoryInfo(filepath);

        foreach (var file in d.GetFiles("*.png"))
        {
            Debug.Log(file.Name);
            // file.FullName is the full path to the file
            await GetImage(file.FullName, file.Name, PortraitPieceType.Hairstyle);
        }
    }

    async Task GetImage(string filepath, string fileName, PortraitPieceType type)
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(filepath))
        {
            await uwr.SendWebRequest();

            if (uwr.result != UnityWebRequest.Result.Success)
            {
                LogManager.Instance.LogError(uwr.error);
                Debug.Log(uwr.error);
            }
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

    bool CheckDirectory(string path)
    {
        if (Directory.Exists(path))
        {
            return true;
        }
        else
        {
            LogManager.Instance.LogError("Error: file path does not exist");
            return false;
        }
    }
}
