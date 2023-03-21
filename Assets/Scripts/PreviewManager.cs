using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.UI;

public class PreviewManager : MonoBehaviour
{
    [SerializeField] GameObject[] animationPreviews;

    [SerializeField] Transform[] animationPreviewDefaultPositions;

    [SerializeField] GameObject center;
    [SerializeField] GameObject background;
    CanvasGroup backgroundCG;

    HorizontalLayoutGroup horizontalLayoutGroup;

    int currentSelectedIndex;

    bool showcaseActive = false;
    private void Awake()
    {
        horizontalLayoutGroup = GetComponent<HorizontalLayoutGroup>();
        backgroundCG = background.GetComponent<CanvasGroup>();

        //for (int i = 0; i < animationPreviews.Length; i++)
        //{
        //    animationPreviewDefaultPositions.Add(animationPreviews[i].transform.position);
        //}
    }
    public void ShowcasePreview(int previewIndex)
    {
        if(showcaseActive)
        {
            DisableShowcase();
            return;
        }

        showcaseActive = true;
        //horizontalLayoutGroup.enabled = false;

        animationPreviews[previewIndex].transform.SetAsLastSibling();

        LeanTween.move(animationPreviews[previewIndex], center.transform.position, 0.1f);
        LeanTween.scale(animationPreviews[previewIndex], new Vector3(1.5f, 1.5f, 0), 0.1f);
        backgroundCG.alpha = 0;
        background.SetActive(true);
        LeanTween.alphaCanvas(backgroundCG, 1, 0.1f);

        currentSelectedIndex = previewIndex;
    }

    public async void DisableShowcase()
    {
        showcaseActive = false;

        LeanTween.move(animationPreviews[currentSelectedIndex], animationPreviewDefaultPositions[currentSelectedIndex].position, 0.15f);
        LeanTween.scale(animationPreviews[currentSelectedIndex], new Vector3(1, 1, 0), 0.15f);
        LeanTween.alphaCanvas(backgroundCG, 0, 0.15f);

        await UniTask.Delay(150);
        background.SetActive(false);

        animationPreviews[currentSelectedIndex].transform.SetAsFirstSibling();
    }
}
