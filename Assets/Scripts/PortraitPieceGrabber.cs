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
using System.Threading;

public enum PortraitPieceType { Skin, Hairstyle }
public class PortraitPieceGrabber : MonoBehaviour
{
    [SerializeField] PGManager pgManager;

    //CancellationTokenSource _tokenSource = null;

    public bool finishedSetup { get; private set; } = false;

    List<Task> tasks;
    private async void Awake()
    {
        LogManager.Instance.SetFirstTimeSetupMessage(true);

        tasks = new List<Task>
        {
            GetSkinsForDisplay(),
            GetHairstylesForDisplay()
        };

        // Wait for all portrait pieces to be added
        await Task.WhenAll(tasks);

        LogManager.Instance.SetFirstTimeSetupMessage(false);

        finishedSetup = true;
    }

    async Task GetSkinsForDisplay()
    {

        string filepath = Directory.GetCurrentDirectory() + "/Portrait Pieces/Portrait_Generator - 16x16/Skins";

        if (!CheckDirectory(filepath)) return;

        DirectoryInfo d = new DirectoryInfo(filepath);

        Path.GetFileNameWithoutExtension(d.FullName);
        foreach (var file in d.GetFiles("*.png"))
        {
            //Debug.Log(file);
            // file.FullName is the full path to the file
            pgManager.AddPortraitPiece(await GetImage(file.FullName, Path.GetFileNameWithoutExtension(file.Name)), PortraitPieceType.Skin);
        }
    }

    async Task GetHairstylesForDisplay()
    {
        //_tokenSource= new CancellationTokenSource();
        //var token = _tokenSource.Token;

        string filepath = Directory.GetCurrentDirectory() + "/Portrait Pieces/Portrait_Generator - 16x16/Hairstyles";

        if (!CheckDirectory(filepath)) return;

        DirectoryInfo d = new DirectoryInfo(filepath);

        foreach (var file in d.GetFiles("*.png"))
        {
            //Debug.Log(file.Name);
            // file.FullName is the full path to the file
            pgManager.AddPortraitPiece(await GetImage(file.FullName, Path.GetFileNameWithoutExtension(file.Name)), PortraitPieceType.Hairstyle);
        }
    }

    public async Task<Sprite> GetImage(string filepath, string fileName)
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(filepath))
        {
            await uwr.SendWebRequest();

            if (uwr.result != UnityWebRequest.Result.Success)
            {
                LogManager.Instance.LogError(uwr.error);
                Debug.Log(uwr.error);
                return null;
            }
            else
            {
                // Get downloaded asset bundle
                Texture2D texture = DownloadHandlerTexture.GetContent(uwr);

                Sprite sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 16f);

                sprite.name = fileName;
                sprite.texture.filterMode = FilterMode.Point;

                return sprite;
            }
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

    private void OnApplicationQuit()
    {
        if (finishedSetup) return;

        //_tokenSource.Cancel();
        //for (int i = 0; i < tasks.Count; i++)
        //{
        //    tasks[i].can
        //}
    }
}
