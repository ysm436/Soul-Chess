using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ExecutionEffect : TargetingEffect
{
    public int basicAD;
    public int insteadAD;
    public int standardHP;

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
        foreach (var target in targets)
        {
            if ((target as ChessPiece).GetHP <= standardHP)
            {
                (target as ChessPiece).SpellAttacked(insteadAD);
            }
            else
            {
                (target as ChessPiece).SpellAttacked(basicAD);
            }
        }
    }
}
