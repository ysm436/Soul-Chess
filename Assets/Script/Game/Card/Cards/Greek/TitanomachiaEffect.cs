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
        List<ChessPiece> targets = GameBoard.instance.gameData.pieceObjects;

        ChessPiece kingPiece = null;

        foreach (var chessPiece in GameBoard.instance.gameData.pieceObjects)
        {
            if (chessPiece.pieceType == ChessPiece.PieceType.King && chessPiece.pieceColor == player.playerColor)
                kingPiece = chessPiece;
        }

        if (kingPiece == null)
            Debug.Log("Titanomachia: No King Piece");

        for (int i = 0; i < repeat; i++)
        {
            int randomIndex = SynchronizedRandom.Range(0, targets.Count);
            ChessPiece target = targets[randomIndex];
            GameBoard.instance.chessBoard.KillByCardEffect(effectPrefab, kingPiece, target);
        }
    }
}
