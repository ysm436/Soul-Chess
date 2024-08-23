using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Mimir : SoulCard
{
    protected override int CardID => Card.cardIdDict["미미르"];
    private PlayerData playercolor;
    private int reduction = 1;

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
            card.cost = card.cost - reduction;
        }
        playercolor.OnGetCard += CardCostReduction;
        InfusedPiece.OnSoulRemoved += RemoveEffect;
    }

    public override void RemoveEffect()
    {
        foreach (var card in playercolor.hand)
        {
            card.cost += reduction;
        }
        playercolor.OnGetCard -= CardCostReduction;
        InfusedPiece.OnSoulRemoved -= RemoveEffect;
    }

    public void CardCostReduction(Card card)
    {
        card.cost -= reduction;
        if (card.cost < 0)
        {
            card.cost = 0;
        }
    }
}