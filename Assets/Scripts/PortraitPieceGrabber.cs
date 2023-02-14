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
using System.Runtime.CompilerServices;
using System.Drawing;

public enum PortraitPieceType { Skin, Hairstyle, Eyes, Accessory}
public class PortraitPieceGrabber : MonoBehaviour
{
    [SerializeField] PGManager pgManager;

    [Space]

    [SerializeField] ImageSize sixteenXSixsteenImageSize;
    [SerializeField] ImageSize thirtyTwoXThirtyTwoImageSize;
    [SerializeField] ImageSize fortyEightXFortyEightImageSize;

    //CancellationTokenSource _tokenSource = null;

    public bool finishedSetup { get; private set; } = false;

    List<Task> tasks;
    private async void Awake()
    {
        pgManager.SetFirstTimeSetupMessage(true);

        tasks = new List<Task>
        {
            GetPortraitPiecesForDisplay(PortraitPieceType.Skin),
            GetPortraitPiecesForDisplay(PortraitPieceType.Hairstyle),
            GetPortraitPiecesForDisplay(PortraitPieceType.Eyes),
            GetPortraitPiecesForDisplay(PortraitPieceType.Accessory)
        };

        // Wait for all portrait pieces to be added
        await Task.WhenAll(tasks);

        pgManager.SetFirstTimeSetupMessage(false);

        finishedSetup = true;

        //LogManager.Instance.Log("Finished loading portrait pieces");
        Debug.Log("Finished loading portrait pieces");
    }

    async Task GetPortraitPiecesForDisplay(PortraitPieceType type)
    {
        string filepath = "";
        switch (type)
        {
            case PortraitPieceType.Skin:
                filepath = Directory.GetCurrentDirectory() + "/Portrait Pieces/Portrait_Generator - 16x16/Skins";
                break;
            case PortraitPieceType.Hairstyle:
                filepath = Directory.GetCurrentDirectory() + "/Portrait Pieces/Portrait_Generator - 16x16/Hairstyles";
                break;
            case PortraitPieceType.Eyes:
                filepath = Directory.GetCurrentDirectory() + "/Portrait Pieces/Portrait_Generator - 16x16/Eyes";
                break;
            case PortraitPieceType.Accessory:
                filepath = Directory.GetCurrentDirectory() + "/Portrait Pieces/Portrait_Generator - 16x16/Accessories";
                break;
        }

        if (!CheckDirectory(filepath)) return;

        DirectoryInfo d = new DirectoryInfo(filepath);

        foreach (var file in d.GetFiles("*.png"))
        {
            // file.FullName is the full path to the file

            Sprite sprite = await GetImage(file.FullName, Path.GetFileNameWithoutExtension(file.Name), PortraitSize.Sixteen);
            if (sprite == null) continue;
            pgManager.AddPortraitPiece(sprite, type);
        }
    }

    public async Task<Sprite> GetImage(string filepath, string fileName, PortraitSize size)
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(filepath))
        {
            await uwr.SendWebRequest();

            if (uwr.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(uwr.error);
                return null;
            }
            else
            {
                // Get downloaded asset bundle
                Texture2D texture = DownloadHandlerTexture.GetContent(uwr);

                switch (size)
                {
                    case PortraitSize.Sixteen:
                        if (texture.width != sixteenXSixsteenImageSize.width || texture.height != sixteenXSixsteenImageSize.height)
                        {
                            Debug.LogWarning("IncorrectSizeException: Tried to get image of size 16x16 (Image name: " + fileName + ") but it's size is incorrect (" + texture.width + " | " + texture.height + ")");
                            return null;
                        }
                        break;
                    case PortraitSize.Thirtytwo:
                        if (texture.width != thirtyTwoXThirtyTwoImageSize.width || texture.height != thirtyTwoXThirtyTwoImageSize.height)
                        {
                            Debug.LogWarning("IncorrectSizeException: Tried to get image of size 32x32 (Image name: " + fileName + ") but it's size is incorrect");
                            return null;
                        }
                        break;
                    case PortraitSize.Fortyeight:
                        if (texture.width != fortyEightXFortyEightImageSize.width || texture.height != fortyEightXFortyEightImageSize.height)
                        {
                            Debug.LogWarning("IncorrectSizeException: Tried to get image of size 48x48 (Image name: " + fileName + ") but it's size is incorrect");
                            return null;
                        }
                        break;
                }

                Sprite sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 16f);

                sprite.name = fileName;
                sprite.texture.name = fileName;
                sprite.texture.filterMode = FilterMode.Point;

                return sprite;
            }
        }
    }

    public async Task<Texture2D> GetImageAsTexture2D(string filepath, string fileName, PortraitSize size)
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(filepath))
        {
            await uwr.SendWebRequest();

            if (uwr.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(uwr.error);
                return null;
            }
            else
            {
                // Get downloaded asset bundle
                Texture2D texture = DownloadHandlerTexture.GetContent(uwr);

                switch (size)
                {
                    case PortraitSize.Sixteen:
                        if (texture.width != sixteenXSixsteenImageSize.width || texture.height != sixteenXSixsteenImageSize.height)
                        {
                            Debug.LogWarning("IncorrectSizeException: Tried to get image of size 16x16 (Image name: " + fileName + ") but it's size is incorrect (" + texture.width + " | " + texture.height + ")");
                            return null;
                        }
                        break;
                    case PortraitSize.Thirtytwo:
                        if (texture.width != thirtyTwoXThirtyTwoImageSize.width || texture.height != thirtyTwoXThirtyTwoImageSize.height)
                        {
                            Debug.LogWarning("IncorrectSizeException: Tried to get image of size 32x32 (Image name: " + fileName + ") but it's size is incorrect");
                            return null;
                        }
                        break;
                    case PortraitSize.Fortyeight:
                        if (texture.width != fortyEightXFortyEightImageSize.width || texture.height != fortyEightXFortyEightImageSize.height)
                        {
                            Debug.LogWarning("IncorrectSizeException: Tried to get image of size 48x48 (Image name: " + fileName + ") but it's size is incorrect");
                            return null;
                        }
                        break;
                }

                texture.name = fileName;
                texture.filterMode = FilterMode.Point;

                //Debug.Log(texture.name);

                return texture;
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
            //LogManager.Instance.LogError("File path does not exist");
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

    [Serializable]
    class ImageSize
    {
        public int width, height;
    }
}
