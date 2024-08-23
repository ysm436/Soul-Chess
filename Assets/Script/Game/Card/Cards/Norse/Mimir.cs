using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Mimir : SoulCard
{
    protected override int CardID => Card.cardIdDict["미미르"];
    private PlayerData playercolor;
    private int reduction = 1;
    Dictionary<Card, int> cardCostDict = new Dictionary<Card, int>();

    protected override void Awake()
    {
        base.Awake();
    }

    public override void AddEffect()
    {
        if (InfusedPiece.pieceColor == GameBoard.PlayerColor.White)
            playercolor = GameBoard.instance.gameData.playerWhite;
        else
            playercolor = GameBoard.instance.gameData.playerBlack;

        foreach (var card in playercolor.hand)
        {
            if (card.cost < reduction)
            {
                cardCostDict.TryAdd(card, card.cost);
            }
            card.cost -= reduction;
        }
        playercolor.OnGetCard += CardCostReduction;
        InfusedPiece.OnSoulRemoved += RemoveEffect;
    }

    public override void RemoveEffect()
    {
        foreach (var card in playercolor.hand)
        {
            if (cardCostDict.ContainsKey(card))
            {
                card.cost += cardCostDict[card];
            }
            else
            {
                card.cost += reduction;
            }
        }
        cardCostDict.Clear();
        playercolor.OnGetCard -= CardCostReduction;
        InfusedPiece.OnSoulRemoved -= RemoveEffect;
    }

    public void CardCostReduction(Card card)
    {
        if (card.cost < reduction)
        {
            cardCostDict.TryAdd(card, card.cost);
        }
        card.cost -= reduction;
        if (card.cost < 0)
        {
            card.cost = 0;
        }
    }
}