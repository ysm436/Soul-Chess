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
    public Sprite back;
    [Multiline]
    public string description;

    public Effect EffectOnCardUsed;

    public bool isFlipped { get; private set; }

    protected virtual void Awake()
    {
        cardObject = GetComponent<CardObject>();

        cardObject.cardNameText.text = cardName;
        cardObject.costText.text = cost.ToString();
        cardObject.spriteRenderer.sprite = illustration;
        cardObject.descriptionText.text = description;
        cardObject.backSpriteRenderer.sprite = back;
    }
    private void OnMouseEnter()
    {
        if (!GameManager.instance.whiteController.isUsingCard && !isFlipped)
            GameManager.instance.ShowCard(this);
    }
    private void OnMouseExit()
    {
        if (!GameManager.instance.whiteController.isUsingCard && !isFlipped)
            GameManager.instance.HideCard();
    }
    private void OnMouseDrag()
    {
        if (!GameManager.instance.whiteController.isUsingCard && !isFlipped)
        {
            Vector3 tmpPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(tmpPos.x, tmpPos.y, 0);
        }
    }
    private void OnMouseUp()
    {
        if (!GameManager.instance.whiteController.isUsingCard && !isFlipped)
        {
            if (transform.position.y > 0)
            {
                if (!TryUse())
                {
                    //카드 원위치
                    GameManager.instance.gameData.playerWhite.UpdateHandPosition();
                }
                else
                {
                    GameManager.instance.gameData.playerWhite.TryRemoveCardInHand(this);
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

    public void FlipFront()
    {
        cardObject.backSpriteRenderer.sortingOrder = -1;
        isFlipped = false;

    }
    public void FlipBack()
    {
        cardObject.backSpriteRenderer.sortingOrder = 0;
        isFlipped = true;
    }

    //public abstract void Use();
}
