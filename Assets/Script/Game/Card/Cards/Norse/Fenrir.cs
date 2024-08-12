using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Fenrir : SoulCard
{
    protected override int CardID => Card.cardIdDict["펜리르"];

    private int IncreaseAmountAD = 20;
    private int IncreaseAmountHP = 20;

    protected override void Awake()
    {
        base.Awake();
        OnInfuse += (ChessPiece chessPiece) => chessPiece.OnKill += IncreaseStat;
    }

    private void IncreaseStat(ChessPiece chessPiece)
    {
        InfusedPiece.AD += IncreaseAmountAD;
        InfusedPiece.maxHP += IncreaseAmountHP;
    }
}
