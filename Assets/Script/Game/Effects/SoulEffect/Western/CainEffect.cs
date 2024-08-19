using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CainEffect : TargetingEffect
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
        Predicate<ChessPiece> condition = (ChessPiece piece) => (piece != soul.InfusedPiece) && (piece.soul != null);
        EffectTarget effectTarget = new EffectTarget(TargetType.Piece, targetPieceRestriction, false, true, condition);
        targetTypes.Add(effectTarget);
    }

    public override void EffectAction(PlayerController player)
    {
        foreach (var target in targets)
        {
            soul.AD = (target as ChessPiece).soul.cost * 7;
            (target as ChessPiece).RemoveSoul();
        }
    }
}
