using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenKnightEffect : TargetingEffect
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
        Predicate<ChessPiece> condition = (ChessPiece piece) => piece != soul.InfusedPiece;
        EffectTarget effectTarget = new EffectTarget(TargetType.Piece, targetPieceRestriction , true, true, condition);
        targetTypes.Add(effectTarget);
    }

    public override void EffectAction(PlayerController player)
    {
        foreach (var target in targets)
        {
            (target as ChessPiece).Attack(gameObject.GetComponent<SoulCard>().InfusedPiece);
        }

        gameObject.GetComponent<SoulCard>().InfusedPiece.SetKeyword(Keyword.Type.Taunt);
        gameObject.GetComponent<SoulCard>().InfusedPiece.buff.AddBuffByKeyword(gameObject.GetComponent<SoulCard>().cardName, Buff.BuffType.Taunt);
    }
}
