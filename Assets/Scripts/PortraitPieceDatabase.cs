using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortraitPieceDatabase : MonoBehaviour
{
    public List<Sprite> Skins;

    public List<Sprite> Hairstyles;

    public void AddSkin(Sprite skin)
    {
        Skins.Add(skin);
    }

    public void AddHairstyle(Sprite hairstyle)
    {
        Hairstyles.Add(hairstyle);
    }
}
