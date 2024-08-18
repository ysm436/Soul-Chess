using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UtgardaLokiEffect : Effect
{
    public override void EffectAction()
    {
        List<ChessPiece> enemyPieceList = GameBoard.instance.gameData.pieceObjects.Where(piece => piece.pieceColor != GameBoard.instance.myController.playerColor).ToList();

        foreach (ChessPiece piece in enemyPieceList)
        {
            piece.SetKeyword(Keyword.Type.Silence);
            piece.buff.AddBuffByKeyword(gameObject.GetComponent<SoulCard>().cardName, Buff.BuffType.Silence);
        }
    }
}
