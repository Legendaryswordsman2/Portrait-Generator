using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetupMessage : MonoBehaviour
{
    [SerializeField] TMP_Text text;

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
}
