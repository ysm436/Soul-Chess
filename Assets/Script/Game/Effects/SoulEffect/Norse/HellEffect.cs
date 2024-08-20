using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class HellEffect : TargetingEffect
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
        EffectTarget effectTarget = new EffectTarget(TargetType.Piece, targetPieceRestriction , true, true, condition);
        targetTypes.Add(effectTarget);
    }

    public override void EffectAction()
    {
        foreach (var target in targets)
        {
            gameObject.GetComponent<Hell>().targetSoul = Instantiate((target as ChessPiece).soul);
            gameObject.GetComponent<Hell>().targetSoul.gameObject.SetActive(false);
            (target as ChessPiece).RemoveSoul();
            gameObject.GetComponent<Hell>().AddEffect();
        }
    }
}
