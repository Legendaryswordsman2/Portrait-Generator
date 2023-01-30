using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class PortraitPieceMerger : MonoBehaviour
{
    [SerializeField, ReadOnlyInspector] List<Texture2D> portraitPieces;

    PGManager pgm;
    PortraitPieceGrabber ppg;

    private async void Awake()
    {
        pgm= GetComponent<PGManager>();

        ppg = GetComponent<PortraitPieceGrabber>();

        await UniTask.WaitUntil(() => pgm.finishedSetup == true);
    }
    public async Task<Sprite> CombinePortraitPieces()
    {
        portraitPieces.Clear();

        string filepath = Directory.GetCurrentDirectory() + "/Portrait Pieces/Portrait_Generator - 16x16/";

        //Debug.Log(filepath + pgm.skin.activeSprite.name + ".png");

        Sprite skinSprite = await ppg.GetImage(filepath + "Skins/" + pgm.skin.activeSprite.name + ".png", pgm.skin.activeSprite.name);

        skinSprite.texture.name = skinSprite.name;

        Sprite hairSprite = await ppg.GetImage(filepath + "Hairstyles/" + pgm.hair.activeSprite.name + ".png", pgm.hair.activeSprite.name);

        hairSprite.texture.name = hairSprite.name;

        portraitPieces.Add(skinSprite.texture);

        portraitPieces.Add(hairSprite.texture);

        Texture2D combinedTexture = CombineTextures(portraitPieces[0], portraitPieces[1]);

        return ConvertTextureToSprite(combinedTexture);
    }

    public Texture2D CombineTextures(Texture2D _texture1, Texture2D texture2)
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
