using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behemoth : SoulCard
{
    protected override int CardID => Card.cardIdDict["베헤모스"];
    private int buffedHP;
    private int buffedAD;

    protected override void Awake()
    {
        base.Awake();
        OnInfuse += SoulEffect;
    }

    public void SoulEffect(ChessPiece chessPiece)
    {
        buffedHP = 0;
        buffedAD = 0;

        GameBoard.instance.myController.OnMyTurnEnd += SoulEffect2;

        chessPiece.OnSoulRemoved += RemoveEffect;
    }

    public void SoulEffect2()
    {
        InfusedPiece.maxHP += 10;
        InfusedPiece.AD += 10;

        buffedHP += 10;
        buffedAD += 10;
    }

    public override void AddEffect()
    {
        InfusedPiece.maxHP += buffedHP;
        InfusedPiece.AD += buffedAD;

        GameBoard.instance.myController.OnMyTurnEnd += SoulEffect2;
    }

    public override void RemoveEffect()
    {
        InfusedPiece.maxHP -= buffedHP;
        InfusedPiece.AD -= buffedAD;

        GameBoard.instance.myController.OnMyTurnEnd -= SoulEffect2;
    }
}
