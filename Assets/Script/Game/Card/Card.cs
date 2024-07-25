using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Card : TargetableObject
{
    private CardObject cardObject;

    [Header("CardData")]
    public string cardName;
    public int cost;
    public Sprite illustration;
    [Multiline]
    public string description;
    public Effect effect;

    protected virtual void Awake()
    {
        cardObject = GetComponent<CardObject>();

        cardObject.cardNameText.text = cardName;
        cardObject.costText.text = cost.ToString();
        cardObject.spriteRenderer.sprite = illustration;
        cardObject.descriptionText.text = description;
    }
    private void OnMouseEnter()
    {
        if (!GameManager.instance.whiteController.isUsingCard)
            GameManager.instance.ShowCard(this);
    }
    private void OnMouseExit()
    {
        if (!GameManager.instance.whiteController.isUsingCard)
            GameManager.instance.HideCard();
    }
    private void OnMouseDrag()
    {
        if (!GameManager.instance.whiteController.isUsingCard)
        {
            Vector3 tmpPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(tmpPos.x, tmpPos.y, 0);
        }
    }
    private void OnMouseUp()
    {
        if (!GameManager.instance.whiteController.isUsingCard)
        {
            if (transform.position.y > 0)
                GameManager.instance.whiteController.UseCard(this);
        }
        else
        {
            //이 카드가 효과의 대상으로 선택되었을 때 코드

        }
    }
    public void Destroy()
    {
        Destroy(gameObject);
    }

    //public abstract void Use();
}
