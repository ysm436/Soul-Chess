using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ExitGames.Client.Photon.StructWrapping;
using Photon.Realtime;
using UnityEngine;

public class Merlin : SoulCard
{
    protected override int CardID => Card.cardIdDict["멀린"];
    PlayerData objectData;
    public int reductionCost = 1;
    Dictionary<Card, int> cardCostDict = new Dictionary<Card, int>();

    protected override void Awake()
    {
        base.Awake();
    }

    public override void AddEffect()
    {
        if (InfusedPiece.pieceColor == GameBoard.PlayerColor.White)
            objectData = GameBoard.instance.gameData.playerWhite;
        else
            objectData = GameBoard.instance.gameData.playerBlack;

        foreach (var card in objectData.hand)
        {
            if (card is SpellCard)
            {
                if (card.cost < reductionCost)
                {
                    cardCostDict.TryAdd(card, card.cost);
                }
                else
                {
                    card.cost -= reductionCost;
                    cardCostDict.TryAdd(card, reductionCost);
                } 
            }
        }

        objectData.OnGetCard += CardCostReduction;
        InfusedPiece.OnSoulRemoved += RemoveEffect;
        InfusedPiece.buff.AddBuffByDescription(cardName, Buff.BuffType.Description, "멀린: 이 영혼이 부여된 기물이 살아있는 동안 내 마법의 비용이 "+ reductionCost + " 감소합니다.", true);
    }

    public override void RemoveEffect()
    {
        foreach (var card in objectData.hand)
        {
            if (card is not SpellCard)
            {
                continue;
            }

            if (cardCostDict.ContainsKey(card))
            {
                card.cost += cardCostDict[card];
            }
            else
            {
                card.cost += reductionCost;
            }
        }
        cardCostDict.Clear();
        objectData.OnGetCard -= CardCostReduction;
        InfusedPiece.buff.TryRemoveSpecificBuff(cardName, Buff.BuffType.Description);
    }

    public void CardCostReduction(Card card)
    {
        if (card is SpellCard)
        {
            if (card.cost < reductionCost)
            {
                cardCostDict.TryAdd(card, card.cost);
            }
            else
            {
                card.cost -= reductionCost;
                cardCostDict.TryAdd(card, reductionCost);
            } 
        }
    }
}
