using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Card : TargetableObject
{
    abstract protected int CardID { get; }

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
    public Reigon reigon;
    public Rarity rarity;
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
        if (EventSystem.current.IsPointerOverGameObject() == false)
            if (!GameBoard.instance.myController.isUsingCard && !isFlipped && !isInSelection)
                GameBoard.instance.ShowCard(this);
    }
    private void OnMouseExit()
    {
        if (EventSystem.current.IsPointerOverGameObject() == false)
            if (!GameBoard.instance.myController.isUsingCard && !isFlipped && !isInSelection)
                GameBoard.instance.HideCard();
    }
    private void OnMouseDrag()
    {
        if (EventSystem.current.IsPointerOverGameObject() == false)
            if (!GameBoard.instance.myController.isUsingCard && !isFlipped && !isInSelection)
            {
                Vector3 tmpPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                transform.position = new Vector3(tmpPos.x, tmpPos.y, 0);
            }
    }
    private void OnMouseUp()
    {
        if (EventSystem.current.IsPointerOverGameObject() == false)
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
        cardObject.backSpriteRenderer.transform.localScale = new Vector3(0.8f, 0.8f, 1);
        cardObject.backSpriteRenderer.sortingOrder = -1;
        isFlipped = false;

    }
    public void FlipBack()
    {
        cardObject.backSpriteRenderer.transform.localScale = new Vector3(1, 1, 1);
        cardObject.backSpriteRenderer.sortingOrder = 1; //뒷면이 어떤 경우에도 완전히 카드 덮도록 정렬 순서 조정
        isFlipped = true;
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

    public enum Type
    {
        Soul,
        Spell
    }


    //Card Dictionary<CardName, CardID>
    public static Dictionary<string, int> cardIdDict = new Dictionary<string, int>(){
        {"오딘", 0},
        {"프리그", 1},
        {"토르", 3},
        {"수르트", 8},
        {"라그나로크", 9},
        {"펜리르", 10},
        {"피의 독수리", 11},
        {"처형", 16},
        {"어미 곰", 19},
        {"포세이돈", 21},
        {"페르세우스", 29},
        {"모르건 르 페이", 37},
        {"호수의 여인", 38},
        {"베헤모스", 39},
        {"아벨", 42},
        {"돈키호테", 44},
        {"크라켄", 48}
    };
}
