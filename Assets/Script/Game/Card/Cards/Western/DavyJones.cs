using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DavyJones : SoulCard
{
    protected override int CardID => Card.cardIdDict["데비 존스"];

    private int change = 10;

    protected override void Awake()
    {
        base.Awake();
    }

    public void IncreaseStat(ChessPiece chessPiece)
    {
        InfusedPiece.AD += change;
        InfusedPiece.maxHP += change;

        InfusedPiece.buff.AddBuffByValue(cardName, Buff.BuffType.AD, change, true);
        InfusedPiece.buff.AddBuffByValue(cardName, Buff.BuffType.HP, change, true);
    }

    public override void AddEffect()
    {

    }

    public override void RemoveEffect()
    {
        List<ChessPiece> pieceList = GameBoard.instance.gameData.pieceObjects.Where(piece => piece.pieceColor == GameBoard.instance.myController.playerColor).ToList();
        foreach (ChessPiece piece in pieceList)
        {
            piece.OnKilled -= IncreaseStat;
        }
    }
}
