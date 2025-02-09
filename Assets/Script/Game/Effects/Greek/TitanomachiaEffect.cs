using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TitanomachiaEffect : Effect
{
    [SerializeField] private int repeat;

    public override void EffectAction(PlayerController player)
    {
        ChessPiece kingPiece = null;

        foreach (var chessPiece in GameBoard.instance.gameData.pieceObjects)
        {
            if (chessPiece.pieceType == ChessPiece.PieceType.King)
            {
                if (chessPiece.pieceColor == player.playerColor)
                    kingPiece = chessPiece;
            }
        }

        if (kingPiece == null)
            Debug.Log("Titanomachia: No King Piece");

        GameBoard.instance.chessBoard.KillByTitanomachiaEffect(effectPrefab, kingPiece, repeat);
    }
}
