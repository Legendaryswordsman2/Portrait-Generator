using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetupMessage : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    [SerializeField] TMP_Text detailsText;

    [Space]

    [SerializeField] Slider progressBar;

    string[] textFrames = new string[4];

    int index;

    bool isActive = false;

    Animator anim;

    private void Awake()
    {
        textFrames[0] = text.text;

        textFrames[1] = text.text + ".";

        textFrames[2] = text.text + "..";

        textFrames[3] = text.text + "...";

        anim = GetComponent<Animator>();

        PortraitPieceGrabber.OnNewSpriteLoaded += PortraitPieceGrabber_OnNewSpriteLoaded;
    }

    private void PortraitPieceGrabber_OnNewSpriteLoaded(object sender, PortraitPieceGrabber.OnNewSpriteLoadedEventArgs e)
    {
        detailsText.text = e.Sprite.name + e.Extention;

        progressBar.maxValue = PortraitPieceGrabber.totalSpritesInBatch;
        progressBar.value = PortraitPieceGrabber.loadedSpritesFromBatch;
    }

    public void SetSetupMessage(bool setActive)
    {
        if(setActive)
        {
            gameObject.SetActive(true);
            isActive = true;
            CycleText();
        }
        else
        {
            isActive = false;
            text.text = "";
            detailsText.text = "";
            progressBar.gameObject.SetActive(false);
            anim.SetTrigger("Trigger");
            //gameObject.SetActive(false);
        }
    }

    async void CycleText()
    {
        if (index == 4)
            index = 0;

        text.text = textFrames[index];
        index++;


        await UniTask.Delay(500);

        if (!isActive) return;

        CycleText();
    }

    public void OnSetupBackgroundAnimationFinished()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        PortraitPieceGrabber.OnNewSpriteLoaded -= PortraitPieceGrabber_OnNewSpriteLoaded;
    }
}
