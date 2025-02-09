using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceEffectIcon : MonoBehaviour
{
    [HideInInspector]
    public ChessPiece piece;

    [SerializeField]
    List<Sprite> sprites;
    SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject[] effectObjects;


    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetIconSprite()
    {
        foreach (Keyword.Type i in Enum.GetValues(typeof(Keyword.Type)))
        {
            if (/* (i != Keyword.Type.Defense) &&  */piece.GetKeyword(i) > 0)
            {
                effectObjects[(int)i].GetComponent<SpriteRenderer>().sprite = sprites[(int)i];
            }
        }
        spriteRenderer.sprite = null;
    }

    public void AttackedEffect()
    {
        spriteRenderer.sprite = sprites[5];
        Invoke("DestroyIcon", 0.2f);
    }

    public void MoveRestrictionEffect()
    {
        spriteRenderer.sprite = sprites[6];
    }

    public void DestroyIcon()
    {
        CancelInvoke("DestroyIcon");
        Destroy(gameObject);
    }

    public void RemoveIcon()
    {
        spriteRenderer.sprite = null;
    }
}
