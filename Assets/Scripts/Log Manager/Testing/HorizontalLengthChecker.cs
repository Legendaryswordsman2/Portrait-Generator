using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalLengthChecker : MonoBehaviour
{
    [field: SerializeField] public RectTransform RectTransform { get; set; }

    private void Start()
    {
        RectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        RectTransform.anchorMax = new Vector2(0.5f, 0.5f);
    }
}
