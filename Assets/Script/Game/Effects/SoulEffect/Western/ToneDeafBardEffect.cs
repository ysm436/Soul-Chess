using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ToneDeafBardEffect : TargetingEffect
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
        EffectTarget effectTarget = new EffectTarget(TargetType.Piece, targetPieceRestriction, false, true, condition);
        targetTypes.Add(effectTarget);
    }

    public override void EffectAction(PlayerController player)
    {
        int ADchange = -20;
        int maxHPchange = 20;

        foreach (var target in targets)
        {
            (target as ChessPiece).AD -= 20;
            (target as ChessPiece).maxHP += 20;

            (target as ChessPiece).buff.AddBuffByValue(gameObject.GetComponent<SoulCard>().cardName, Buff.BuffType.AD, ADchange, true);
            (target as ChessPiece).buff.AddBuffByValue(gameObject.GetComponent<SoulCard>().cardName, Buff.BuffType.HP, maxHPchange, true);
        }
    }
}
