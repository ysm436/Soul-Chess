using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JackFrost : SoulCard
{
    protected override int CardID => Card.cardIdDict["잭 프로스트"];

    protected override void Awake()
    {
        base.Awake();
        //OnInfuse += SoulEffect;
    }

    public override void AddEffect()
    {
        
    }
    public override void RemoveEffect()
    {
        List<ChessPiece> enemyPieceList = GameBoard.instance.gameData.pieceObjects.Where(piece => piece.pieceColor != GameBoard.instance.CurrentPlayerController().playerColor).ToList();

        foreach (ChessPiece piece in enemyPieceList)
        {
            piece.buff.TryRemoveSpecificBuff(this.cardName, Buff.BuffType.Stun);
        }
        GameBoard.instance.CurrentPlayerController().OnMyTurnStart -= RemoveEffect;
    }
}
