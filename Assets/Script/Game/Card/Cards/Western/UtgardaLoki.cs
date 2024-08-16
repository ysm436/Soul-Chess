using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UtgardaLoki : SoulCard
{
    protected override int CardID => cardIdDict["우트가르다 로키"];

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
            piece.SetKeyword(Keyword.Type.Silence);
            piece.buff.AddBuffByDescription(this.cardName, Buff.BuffType.Description, "우트가르다 로키: 침묵 부여", false);
        }
    }

    public override void AddEffect()
    {
        
    }
    public override void RemoveEffect()
    {
        
    }
}
