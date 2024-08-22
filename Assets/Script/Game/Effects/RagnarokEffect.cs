using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagnarokEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        List<ChessPiece> pieceList = GameBoard.instance.gameData.pieceObjects;

        for (int i = pieceList.Count - 1; i >= 0; i--)
        {
            if (pieceList[i].pieceType == ChessPiece.PieceType.Pawn)
            {
                pieceList[i].Kill();
            }
        }
    }
}
