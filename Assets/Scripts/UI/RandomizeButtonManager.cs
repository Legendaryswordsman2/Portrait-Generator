using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomizeButtonManager : MonoBehaviour
{
    [SerializeField] PGManager pgManager;

    public void RandomizePortrait()
    {
        pgManager.RandomizePortrait();

        LeanTween.cancel(gameObject);
        transform.rotation = Quaternion.identity;
        LeanTween.rotateAround(gameObject, new Vector3(0, 0, 360), 360, 0.4f).setEaseInOutBack();
    }
}
