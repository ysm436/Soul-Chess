using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behemoth : SoulCard
{
    protected override int CardID => Card.cardIdDict["베헤모스"];

    protected override void Awake()
    {
        base.Awake();
        OnInfuse += SoulEffect;
    }

    public void SoulEffect(ChessPiece chessPiece)
    {
        GameBoard.instance.myController.OnMyTurnEnd += SoulEffect2;

        chessPiece.OnSoulRemoved += RemoveEffect;
    }

    public void SoulEffect2()
    {
        InfusedPiece.maxHP += 10;
        InfusedPiece.AD += 10;

        InfusedPiece.buff.AddBuffByValue(cardName, Buff.BuffType.HP, 10, true);
        InfusedPiece.buff.AddBuffByValue(cardName, Buff.BuffType.AD, 10, true);
    }

    public override void AddEffect()
    {
        GameBoard.instance.myController.OnMyTurnEnd += SoulEffect2;
    }

    public override void RemoveEffect()
    {
        GameBoard.instance.myController.OnMyTurnEnd -= SoulEffect2;
    }
}
