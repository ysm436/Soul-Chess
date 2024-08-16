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
        OnInfuse += SoulEffect;
    }

    public void SoulEffect(ChessPiece chessPiece)
    {
        List<ChessPiece> enemyPieceList = GameBoard.instance.gameData.pieceObjects.Where(piece => piece.pieceColor != GameBoard.instance.myController.playerColor).ToList();

        foreach (ChessPiece piece in enemyPieceList)
        {
            piece.SetKeyword(Keyword.Type.Stun);
            piece.buff.AddBuffByDescription(this.cardName, Buff.BuffType.Description, "잭 프로스트: 기절 부여", false);
            GameBoard.instance.CurrentPlayerController().OnMyTurnStart += RemoveEffect;
        }
    }

    public override void AddEffect()
    {
        
    }
    public override void RemoveEffect()
    {
        List<ChessPiece> enemyPieceList = GameBoard.instance.gameData.pieceObjects.Where(piece => piece.pieceColor != GameBoard.instance.CurrentPlayerController().playerColor).ToList();

        foreach (ChessPiece piece in enemyPieceList)
        {
            piece.buff.TryRemoveSpecificBuff(this.cardName, Buff.BuffType.Description);
        }
        GameBoard.instance.CurrentPlayerController().OnMyTurnStart -= RemoveEffect;
    }
}
