using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagnarokEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        List<ChessPiece> pieceList = GameBoard.instance.gameData.pieceObjects;
        ChessPiece kingPiece = null;

        foreach (var chessPiece in pieceList)
        {
            if (chessPiece.pieceType == ChessPiece.PieceType.King && chessPiece.pieceColor == player.playerColor)
                kingPiece = chessPiece;
        }

        if (kingPiece == null)
            Debug.Log("Ragnarok: No King Piece");

        for (int i = pieceList.Count - 1; i >= 0; i--)
        {
            if (pieceList[i].pieceType == ChessPiece.PieceType.Pawn)
            {
                Debug.Log("Ragnarok: Kill Pawn");
                GameBoard.instance.chessBoard.KillByCardEffect(effectPrefab, kingPiece, pieceList[i]);
            }
        }
    }
}
