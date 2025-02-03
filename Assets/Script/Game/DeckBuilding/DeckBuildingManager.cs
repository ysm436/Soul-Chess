using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class DeckBuildingManager : MonoBehaviour
{
    public DeckManager deckManager;

    public List<GameObject> cardDisplayList = new List<GameObject>();
    public List<int> cardIndexList = new List<int>();

    // [SerializeField] private List<Sprite> iconSprites;
    [SerializeField] private int quantitySetting;

    public Transform cardDisplayArea;
    public Transform displayStorageArea;        // 디스플레이 카드가 덱에 들어가 남아있는 카드의 개수가 0개가 되었을 때 저장되는 창고
    public Transform trashCan;              // 더 이상 쓰지 않는 object를 넣어두는 쓰레기통
    
    public GameObject displayPrefab;
    /* public GameObject oddDisplayPrefab;
    public GameObject evenDisplayPrefab; */

    private void Awake()
    {
        SetDisplayCard();
    }

    // 카드들을 화면에 나타냅니다.
    private void SetDisplayCard()
    {
        List<Tuple<GameObject, Card>> OrderedCards = new List<Tuple<GameObject, Card>>();
        
        for (int i = 0; i < GameManager.instance.AllCards.Length; i++)
        {
            GameObject obj = GameManager.instance.AllCards[i];
            if (obj)
            {
                Card objCard = obj.GetComponent<Card>();
                OrderedCards.Add(Tuple.Create(obj, objCard));
            }
        }

        OrderedCards = OrderedCards
            .OrderBy(entry => entry.Item2.cost)
            .ThenBy(entry => entry.Item1.GetComponent<SoulCard>() != null ? 0 : 1)
            .ThenByDescending(entry => entry.Item2.rarity)
            .ThenBy(entry => entry.Item2.reigon)
            .ToList();

        for (int i = 0; i < OrderedCards.Count; i++)
        {
            GameObject card = OrderedCards[i].Item1;
            Card cardInfo = OrderedCards[i].Item2;
            int quantity;
            if (!(OrderedCards[i].Item2.rarity == Card.Rarity.Common))
                quantity = 1;
            else
                quantity = 2;

            GameObject cardDisplayObj = Instantiate(displayPrefab, cardDisplayArea);
            DisplayInfo displayInfo = cardDisplayObj.GetComponent<DisplayInfo>();

            cardDisplayObj.name = card.name + "Display";
            displayInfo.cardDisplayIndex = i;
            displayInfo.cardIndex = cardInfo.GetCardID;
            displayInfo.CardName = cardInfo.cardName;
            displayInfo.Cost = cardInfo.cost;
            displayInfo.description = cardInfo.description;
            displayInfo.reigon = cardInfo.reigon;
            displayInfo.Rarity = cardInfo.rarity;
            displayInfo.illustrate.sprite = cardInfo.illustration;
            displayInfo.Quantity = quantity;

            if (cardInfo is SoulCard)
            {
                displayInfo.cardType = Card.Type.Soul;
                displayInfo.HP = (cardInfo as SoulCard).HP;
                displayInfo.AD = (cardInfo as SoulCard).AD;
            }
            else
            {
                displayInfo.cardType = Card.Type.Spell;
            }

            displayInfo.chessPieceType = ChessPiece.PieceType.None;
            cardDisplayList.Add(cardDisplayObj);
            cardIndexList.Add(displayInfo.cardIndex);
        }
    }

    //디스플레이될 카드들을 재설정 합니다.
    public void ReloadDisplayCard()
    {
        foreach (var displayCard in cardDisplayList)
        {
            DisplayInfo targetDisplay = displayCard.GetComponent<DisplayInfo>();

            if (targetDisplay.Rarity == Card.Rarity.Common)
            {
                targetDisplay.Quantity = 2;
            }
            else
            {
                targetDisplay.Quantity = 1;
            }
        }
    }

    /* private Sprite ChessPieceDisplay(ChessPiece.PieceType piece)
    {
        if (piece.HasFlag(ChessPiece.PieceType.King))
        {
            return iconSprites[0];
        }
        else if (piece.HasFlag(ChessPiece.PieceType.Quene))
        {
            return iconSprites[1];
        }
        else if (piece.HasFlag(ChessPiece.PieceType.Rook))
        {
            return iconSprites[2];
        }
        else if (piece.HasFlag(ChessPiece.PieceType.Bishop))
        {
            return iconSprites[3];
        }
        else if (piece.HasFlag(ChessPiece.PieceType.Knight))
        {
            return iconSprites[4];
        }
        else
        {
            return iconSprites[5];
        }
    } */
}