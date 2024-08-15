using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSquareOutline : MonoBehaviour
{
    public enum TargetableStates
    {
        negative,
        movable,
        positive,
        none
    };
    public List<Sprite> sprites;
    SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    public void changeOutline(TargetableStates states)
    {
        spriteRenderer.sprite = sprites[(int)states];
    }
}
