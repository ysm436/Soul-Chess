using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagnarokEffect : Effect
{
    public override void EffectAction()
    {
        List<ChessPiece> pieceList =  GameManager.instance.gameData.pieceObjects;

        for (int i = pieceList.Count - 1; i >= 0; i--)
        {
            if (pieceList[i].pieceType != ChessPiece.PieceType.King && pieceList[i].pieceType != ChessPiece.PieceType.Quene)
            {
                GameManager.instance.KillPiece(pieceList[i]);
            }
        }
    }
}
