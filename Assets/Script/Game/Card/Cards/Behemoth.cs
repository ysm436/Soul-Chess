using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behemoth : SoulCard
{
    protected override void Awake()
    {
        base.Awake();
        gameObject.GetComponent<SoulCard>().OnInfuse += SoulEffect;
    }

    public void SoulEffect(ChessPiece chessPiece)
    {
        GameManager.instance.whiteController.OnMyTurnEnd += () => SoulEffect2(InfusedPiece);
    }

    public void SoulEffect2(ChessPiece piece)
    {
        piece.maxHP += 10;
        piece.AD += 10;
        piece.soul.HP += 10;
        piece.soul.AD += 10;
    }
}
