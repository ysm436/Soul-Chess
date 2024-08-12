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
        GameBoard.instance.whiteController.OnMyTurnEnd += () => SoulEffect2(InfusedPiece);
    }

    public void SoulEffect2(ChessPiece piece)
    {
        HP += 10;
        AD += 10;
    }
}
