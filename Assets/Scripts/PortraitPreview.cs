using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PortraitPreview : MonoBehaviour
{
    [SerializeField] Sprite previewSpritesheet;

    PGManager pgm;
    PortraitPieceMerger ppm;
    private async void Awake()
    {
        pgm = GetComponent<PGManager>();

        ppm = GetComponent<PortraitPieceMerger>();

        await UniTask.WaitUntil(() => PGManager.FinishedSetup == true);

        Pgm_OnDropdownChanged(this, null);

        pgm.OnDropdownChanged += Pgm_OnDropdownChanged;
    }

    private async void Pgm_OnDropdownChanged(object sender, System.EventArgs e)
    {
        await UniTask.WaitForEndOfFrame(this);

        //Debug.Log("Combining sprites for preview");
        Sprite CombinedSprite = await ppm.CombinePortraitPieces(PortraitSize.Sixteen);

        if(CombinedSprite != null)
        ppm.OverrideTexture(previewSpritesheet, CombinedSprite);
    }
}
