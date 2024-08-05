using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class Card : TargetableObject
{
    private CardObject cardObject;

    [Header("CardData")]
    public string cardName;
    public int cost
    {
        set
        {
            _cost = value;
            cardObject.costText.text = value.ToString();
        }
        get { return _cost; }
    }
    [SerializeField]
    private int _cost;
    public Sprite illustration;
    public Reigon reigon;
    public Rarity rarity;
    [Multiline]
    public string description;
    public Effect EffectOnCardUsed;

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
            {
                if (!TryUse())
                {
                    //카드 원위치
                }
            }
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

    public virtual bool TryUse()
    {
        GameManager.instance.whiteController.UseCard(this);

        return true;
    }

    //public abstract void Use();

    public enum Reigon
    {
        Greek,
        Norse,
        Western
    }   

    public enum Rarity
    {
        Common,
        Legendary,
        Mythical
    }
}
