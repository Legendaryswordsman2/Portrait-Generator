using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortraitPieceMerger : MonoBehaviour
{
    [SerializeField, ReadOnlyInspector] List<Texture2D> portraitPieces;

    PGManager pgm;

    private void Awake()
    {
        pgm= GetComponent<PGManager>();
    }
    public void CombinePortraitPieces()
    {

    }
}
