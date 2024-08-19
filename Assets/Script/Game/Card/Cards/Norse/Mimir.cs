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

        if (GameBoard.instance.myController.playerColor == GameBoard.PlayerColor.White)
            playercolor = GameBoard.instance.gameData.playerWhite;
        else
            playercolor = GameBoard.instance.gameData.playerBlack;
    }

    public override void AddEffect()
    {
        foreach (var card in playercolor.hand)
        {
            card.cost = card.cost - reduction;
        }
        playercolor.OnGetCard += CardCostReduction;
    }

    public override void RemoveEffect()
    {
        foreach (var card in playercolor.hand)
        {
            card.cost += reduction;
        }
        playercolor.OnGetCard -= CardCostReduction;
    }

    public void CardCostReduction(Card card)
    {
        card.cost -= reduction;
    }
}