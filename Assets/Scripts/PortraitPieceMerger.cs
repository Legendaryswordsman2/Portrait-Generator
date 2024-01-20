using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public enum PortraitSize { Sixteen, Thirtytwo, Fortyeight }
public class PortraitPieceMerger : MonoBehaviour
{
    [SerializeField, ReadOnlyInspector] List<Texture2D> portraitPiecesToBeCombined;

    PGManager pgm;
    PortraitPieceGrabber ppg;

    private async void Awake()
    {
        pgm= GetComponent<PGManager>();

        ppg = GetComponent<PortraitPieceGrabber>();

        await UniTask.WaitUntil(() => PGManager.FinishedSetup == true);
    }
    public async Task<Sprite> CombinePortraitPieces(PortraitSize size)
    {
        portraitPiecesToBeCombined.Clear();

        #region Filepath
        string filepath;

        Path.Combine(Directory.GetCurrentDirectory(), "Portrait Pieces", "Portrait_Generator - 16x16");

        switch (size)
        {
            case PortraitSize.Sixteen:
                filepath = Path.Combine(Directory.GetCurrentDirectory(), "Portrait Pieces", "Portrait_Generator - 16x16");
                break;
            case PortraitSize.Thirtytwo:
                filepath = Path.Combine(Directory.GetCurrentDirectory(), "Portrait Pieces", "Portrait_Generator - 32x32");
                break;
            case PortraitSize.Fortyeight:
                filepath = Path.Combine(Directory.GetCurrentDirectory(), "Portrait Pieces", "Portrait_Generator - 48x48");
                break;
            default:
                Debug.LogError("File size provided is not a known size");
                return null;
        }
        #endregion

        #region Get Portrait Pieces to be Combined
        if (size == PortraitSize.Sixteen)
        {
            for (int i = 0; i < pgm.portraitPieces.Length; i++)
            {
                if (pgm.portraitPieces[i].activeSprite != null)
                    portraitPiecesToBeCombined.Add(pgm.portraitPieces[i].activeSprite.texture);
            }
        }
        else
        {
            for (int i = 0; i < pgm.portraitPieces.Length; i++)
            {
                if (pgm.portraitPieces[i].activeSprite == null) continue;
                string _filepath = filepath;
                switch (i)
                {
                    case 0:
                        _filepath = Path.Combine(filepath, "Skins");
                        break;
                    case 1:
                        _filepath = Path.Combine(filepath, "Hairstyles");
                        break;
                    case 2:
                        _filepath = Path.Combine(filepath, "Eyes");
                        break;
                    case 3:
                        _filepath = Path.Combine(filepath, "Accessories");
                        break;
                }

                string fileUrl = new Uri(Path.Combine(_filepath, pgm.portraitPieces[i].activeSprite.name + ".png")).AbsoluteUri;
                Texture2D texture2D = await ppg.GetImageAsTexture2D(fileUrl, pgm.portraitPieces[i].activeSprite.name, size);

                if (texture2D == null) return null;
                portraitPiecesToBeCombined.Add(texture2D);
            }
        }
        #endregion

        if (portraitPiecesToBeCombined.Count == 0)
        {
            Debug.LogWarning("No portrait pieces to be combined");
            //SetupManager.Instance.DisplayError(ErrorType.MissingPortraitPiecesFolder);
            return null;
        }
        else if (portraitPiecesToBeCombined.Count == 1)
            return ConvertTextureToSprite(portraitPiecesToBeCombined[0]);

        Texture2D finalTexture = portraitPiecesToBeCombined[0];

        for (int i = 1; i < portraitPiecesToBeCombined.Count; i++)
        {
            finalTexture = CombineTextures(finalTexture, portraitPiecesToBeCombined[i]);
        }

        return ConvertTextureToSprite(finalTexture);
    }

    Texture2D CombineTextures(Texture2D _texture1, Texture2D texture2)
    {
        Texture2D texture1 = Instantiate(_texture1);

        int startX = 0;
        int startY = 0;

        for (int x = startX; x < texture1.width; x++)
        {

            for (int y = startY; y < texture1.height; y++)
            {
                Color s1Color = texture1.GetPixel(x, y);
                Color s2Color = texture2.GetPixel(x - startX, y - startY);

                Color final_color = Color.Lerp(s1Color, s2Color, s2Color.a / 1.0f);

                texture1.SetPixel(x, y, final_color);
            }
        }
        texture1.Apply();

        return texture1;

        //Sprite combinedSprite = Sprite.Create(texture1, new Rect(0.0f, 0.0f, texture1.width, texture1.height), new Vector2(0.5f, 0.5f), 100.0f);

        //return combinedSprite;
    }

    public Sprite OverrideTexture(Sprite textureToOverride, Sprite TextureToOverrideWith)
    {
        int startX = 0;
        int startY = 0;

        for (int x = startX; x < textureToOverride.texture.width; x++)
        {

            for (int y = startY; y < textureToOverride.texture.height; y++)
            {
                Color s2Color = TextureToOverrideWith.texture.GetPixel(x - startX, y - startY);

                textureToOverride.texture.SetPixel(x, y, s2Color);
            }
        }
        textureToOverride.texture.Apply();

        Sprite combinedSprite = Sprite.Create(textureToOverride.texture, new Rect(0.0f, 0.0f, textureToOverride.texture.width, textureToOverride.texture.height), new Vector2(0.5f, 0.5f), 100.0f);

        return combinedSprite;
    }

    Sprite ConvertTextureToSprite(Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
    }
}
