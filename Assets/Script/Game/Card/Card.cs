using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Card : TargetableObject, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IEndDragHandler
{
    abstract protected int CardID { get; }

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

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        if (!GameBoard.instance.myController.isUsingCard && !isFlipped && !isInSelection)
            GameBoard.instance.ShowCard(this);
    }
    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        if (!GameBoard.instance.myController.isUsingCard && !isFlipped && !isInSelection)
        {
            GameBoard.instance.HideCard();
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (!GameBoard.instance.myController.isUsingCard && !isFlipped && !isInSelection)
        {
            Vector3 tmpPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(tmpPos.x, tmpPos.y, 0);
        }
    }
    public void OnEndDrag(PointerEventData eventData)
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
        return GameBoard.instance.CurrentPlayerController().UseCard(this);
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
        {"우트가르다 로키", 12},
        {"처형", 16},
        {"아레스", 18},
        {"어미 곰", 19},
        {"포세이돈", 21},
        {"아테나", 22},
        {"케르베로스", 24},
        {"판도라의 상자", 27},
        {"중기갑 보병", 29},
        {"데비 존스", 35},
        {"카인", 36},
        {"모르건 르 페이", 37},
        {"호수의 여인", 38},
        {"베헤모스", 39},
        {"다윗", 40},
        {"아벨", 42},
        {"잭 프로스트", 43},
        {"돈키호테", 44},
        {"음치 음유시인", 46},
        {"근엄한 경비병", 47},
        {"크라켄", 48}
    };
}
