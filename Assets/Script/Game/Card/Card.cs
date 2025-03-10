using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun.Demo.Cockpit;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Card : TargetableObject, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IEndDragHandler
{
    public static int discardCost = 2;
    [HideInInspector]
    public bool isMine { get => owner.playerColor == GameBoard.instance.playerColor; }
    public PlayerData owner;
    public int handIndex = -1;

    abstract protected int CardID { get; }
    public int GetCardID { get => CardID; }

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
    public int originalCost { get; private set; }
    public Sprite illustration;
    public Sprite back;
    public Reigon reigon;
    public Rarity rarity;
    [Multiline]
    public string description;

    public Effect EffectOnCardUsed;

    public List<Keyword.Type> cardKeywords = new List<Keyword.Type>();

    public bool isFlipped { get; private set; }

    [HideInInspector] public bool isInSelection;

    private bool isDragging;

    protected virtual void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        cardObject = GetComponent<CardObject>();

        cardObject.cardNameText.text = cardName;
        cardObject.costText.text = cost.ToString();
        cardObject.illustration.sprite = illustration;
        cardObject.descriptionText.text = description;
        cardObject.backSpriteRenderer.sprite = back;

        originalCost = cost;
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        if (!GameBoard.instance.myController.isUsingCard && !isFlipped && !isInSelection && !isDragging)
            GameBoard.instance.ShowCard(this);
    }
    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        if (!GameBoard.instance.myController.isUsingCard && !isFlipped && !isInSelection)
        {
            GameBoard.instance.HideCard();
        }
    }

    /*// 카드 클릭 사용 방식
    private void OnMouseDown()
    {
        if (!isMine) return;
        
        if (!GameBoard.instance.myController.isUsingCard && !isFlipped && !isInSelection)
        {
            if (!TryUse())
            {
                //카드 원위치
                GameBoard.instance.gameData.myPlayerData.UpdateHandPosition();
            }
        }
    }*/

    // 카드 드래그 사용 방식
    public void OnDrag(PointerEventData eventData)
    {
        if (!isMine) return;

        if (!GameBoard.instance.myController.isUsingCard && !isFlipped && !isInSelection)
        {
            Vector3 tmpPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(tmpPos.x, tmpPos.y, 0);

            GameBoard.instance.myHand.color = new Color(1, 1, 1, 0.6f);
            //GameBoard.instance.trashCan.gameObject.SetActive(true);

            isDragging = true;
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isMine) return;

        GameBoard.instance.myHand.color = Color.clear;
        GameBoard.instance.trashCan.gameObject.SetActive(false);

        if (!GameBoard.instance.myController.isUsingCard && !isFlipped && !isInSelection)
        {
            if (GameBoard.instance.isCardDiscarded(transform.position))
            {
                if (!TryDiscard())
                {
                    //카드 원위치
                    GameBoard.instance.gameData.myPlayerData.UpdateHandPosition();
                }
            }
            else if (GameBoard.instance.isCardUsed(transform.position))
            {
                if (!TryUse())
                {
                    //카드 원위치
                    GameBoard.instance.gameData.myPlayerData.UpdateOneCardPosition(this);
                }
            }
            else
            {
                GameBoard.instance.gameData.myPlayerData.UpdateOneCardPosition(this);
            }

            isDragging = false;
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

        GameManager.instance.soundManager.PlaySFX("UseCard");

        //코스트 제거는 PlayerController.UseCardEffect에서 수행함 (타겟 지정 후 효과 발동한 다음 코스트 제거)
        return GameBoard.instance.CurrentPlayerController().UseCard(this);
    }
    public virtual bool TryDiscard()
    {
        if (!GameBoard.instance.isActivePlayer)
            return false;
        if (GameBoard.instance.gameData.myPlayerData.soulEssence < discardCost)
            return false;

        GameBoard.instance.myController.DiscardCard(this);
        return true;
    }

    public void FlipFront()
    {
        if (cardObject.ADCircle != null)
        {
            cardObject.ADCircle.SetActive(true);
            cardObject.HPCircle.SetActive(true);
        }
        cardObject.backSpriteRenderer.transform.localScale = new Vector3(0.8f, 0.8f, 1);
        cardObject.backSpriteRenderer.sortingOrder = -1;
        isFlipped = false;

    }
    public void FlipBack()
    {
        if (cardObject.ADCircle != null)
        {
            cardObject.ADCircle.SetActive(false);
            cardObject.HPCircle.SetActive(false);
        }
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


    static public int[] GetCardIDArray(List<Card> cards)
    {
        int[] cardIDArray = new int[cards.Count];
        for (int i = 0; i < cards.Count; i++)
        {
            cardIDArray[i] = cards[i].GetCardID;
        }
        return cardIDArray;
    }

    //Card Dictionary<CardName, CardID>
    public static Dictionary<string, int> cardIdDict = new Dictionary<string, int>(){
        {"오딘의 눈", 0},
        {"프리그", 1},
        {"이미르", 2},
        {"토르", 3},
        {"로키", 4},
        {"티르", 5},
        {"미미르", 6},
        {"헬", 7},
        {"수르트", 8},
        {"라그나로크", 9},
        {"광포한 바르그", 10},
        {"피의 독수리", 11},
        {"오만한 주술사", 12},
        {"바이킹 전사", 13},
        {"약탈선", 14},
        {"제우스", 15},
        {"처형", 16},
        {"하데스", 17},
        {"아레스", 18},
        {"어미 곰", 19},
        {"고독한 대장장이", 20},
        {"포세이돈", 21},
        {"아테나", 22},
        {"히드라", 23},
        {"케르베로스", 24},
        {"메두사의 시선", 25},
        {"헤라클레스", 26},
        {"판도라의 상자", 27},
        {"티타노마키아", 28},
        {"중기갑 보병", 29},
        {"생각뿐인 철학자", 30},
        {"신식-낫전차", 31},
        {"아서왕의 가호", 32},
        {"녹색 기사", 33},
        {"멀린", 34},
        {"데비 존스", 35},
        {"카인", 36},
        {"음험한 마녀", 37},
        {"호수의 여인", 38},
        {"베헤모스", 39},
        {"다윗", 40},
        {"로빈 후드", 41},
        {"아벨", 42},
        {"잭 프로스트", 43},
        {"늙은 방랑기사", 44},
        {"침착한 명사수", 45},
        {"음치 음유시인", 46},
        {"근엄한 경비병", 47},
        {"심해의 바다괴물", 48},
        {"아폴론", 49},
        //{"궁니르", 1000}, //서브 스펠카드
        //{"감반테인", 1001},
        //{"드라우프니르", 1002},
        {"약탈자", 1014},
        {"천둥벼락", 1015},
        //{"메두사의 머리", 1025},
        //{"페가수스", 1026},
        //{"하데스의 투구", 1027}
    };
}
