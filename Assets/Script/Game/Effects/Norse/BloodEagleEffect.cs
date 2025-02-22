using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BloodEagleEffect : TargetingEffect
{
    ChessPiece.PieceType targetPieceRestriction =
        ChessPiece.PieceType.Pawn |
        ChessPiece.PieceType.Knight |
        ChessPiece.PieceType.Bishop |
        ChessPiece.PieceType.Rook |
        ChessPiece.PieceType.Quene |
        ChessPiece.PieceType.King;

    SoulCard soul = null;

    void Awake()
    {
        if (soul == null) soul = gameObject.GetComponent<SoulCard>();
        Predicate<ChessPiece> condition = (ChessPiece piece) => piece.soul != null;
        EffectTarget effectTarget = new EffectTarget(TargetType.Piece, targetPieceRestriction, true, false, condition);
        targetTypes.Add(effectTarget);
    }

    public override void EffectAction(PlayerController player)
    {
        ChessPiece kingPiece = null;

        foreach (var chessPiece in GameBoard.instance.gameData.pieceObjects)
        {
            if (chessPiece.pieceType == ChessPiece.PieceType.King && chessPiece.pieceColor == player.playerColor)
                kingPiece = chessPiece;
        }

        if (kingPiece == null)
            Debug.Log("BloodEagle: No King Piece");

        foreach (var target in targets)
        {
            GameBoard.instance.chessBoard.KillByCardEffect(effectPrefab, kingPiece, target as ChessPiece);
        }
    }
}
