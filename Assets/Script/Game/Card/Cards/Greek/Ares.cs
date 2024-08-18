using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ares : SoulCard
{
    protected override int CardID => Card.cardIdDict["아레스"];

    private int IncreaseAmountAD = 15;
    private int IncreaseAmountHP = 15;

    protected override void Awake()
    {
        base.Awake();
    }

    public void IncreaseStat(ChessPiece chessPiece)
    {
        InfusedPiece.AD += IncreaseAmountAD;
        InfusedPiece.maxHP += IncreaseAmountHP;

        InfusedPiece.buff.AddBuffByValue(this.cardName, Buff.BuffType.AD, IncreaseAmountAD, true);
        InfusedPiece.buff.AddBuffByValue(this.cardName, Buff.BuffType.HP, IncreaseAmountHP, true);
    }

    public override void AddEffect()
    {
        List<ChessPiece> pieceList = GameBoard.instance.gameData.pieceObjects;
        foreach (ChessPiece piece in pieceList)
        {
            piece.OnKilled += IncreaseStat;
        }
    }

    public override void RemoveEffect()
    {
        List<ChessPiece> pieceList = GameBoard.instance.gameData.pieceObjects;
        foreach (ChessPiece piece in pieceList)
        {
            piece.OnKilled -= IncreaseStat;
        }
    }
}
