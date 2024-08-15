using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class Card : TargetableObject
{
    private CardObject cardObject;

    protected Predicate<ChessPiece> targetCondition = null;

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

    [HideInInspector] public bool isInSelection;

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
        if (!GameBoard.instance.myController.isUsingCard && !isFlipped && !isInSelection)
            GameBoard.instance.ShowCard(this);
    }
    private void OnMouseExit()
    {
        if (!GameBoard.instance.myController.isUsingCard && !isFlipped && !isInSelection)
            GameBoard.instance.HideCard();
    }
    private void OnMouseDrag()
    {
        if (!GameBoard.instance.myController.isUsingCard && !isFlipped && !isInSelection)
        {
            Vector3 tmpPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(tmpPos.x, tmpPos.y, 0);
        }
    }
    private void OnMouseUp()
    {
        if (!GameBoard.instance.myController.isUsingCard && !isFlipped && !isInSelection)
        {
            if (transform.position.y > 0)
            {
                if (!TryUse())
                {
                    //카드 원위치
                    GameBoard.instance.gameData.playerWhite.UpdateHandPosition();
                }
                else
                {
                    GameBoard.instance.gameData.playerWhite.TryRemoveCardInHand(this);
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
        if (!GameBoard.instance.isActivePlayer)
            return false;
        if (GameBoard.instance.CurrentPlayerData().soulEssence < cost)
            return false;

        //코스트 제거는 PlayerController.UseCardEffect에서 수행함 (타겟 지정 후 효과 발동한 다음 코스트 제거)
        GameBoard.instance.CurrentPlayerController().UseCard(this, targetCondition);
        return true;
    }

    public void FlipFront()
    {
        cardObject.backSpriteRenderer.sortingOrder = -1;
        isFlipped = false;

    }
    public void FlipBack()
    {
        cardObject.backSpriteRenderer.sortingOrder = 1; //뒷면이 어떤 경우에도 완전히 카드 덮도록 정렬 순서 조정
        isFlipped = true;
    }

    //public abstract void Use();
}
