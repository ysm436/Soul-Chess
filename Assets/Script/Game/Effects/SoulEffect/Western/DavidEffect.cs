using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun.Demo.Cockpit;
using UnityEngine;

public class DavidEffect : TargetingEffect
{
    [SerializeField] private int standardAD = 70;

    ChessPiece.PieceType targetPieceRestriction =
        ChessPiece.PieceType.Pawn |
        ChessPiece.PieceType.Knight |
        ChessPiece.PieceType.Bishop |
        ChessPiece.PieceType.Rook |
        ChessPiece.PieceType.Quene |
        ChessPiece.PieceType.King;


    void Awake()
    {
        Predicate<ChessPiece> condition = (ChessPiece piece) => piece.AD >= standardAD;
        
        EffectTarget effectTarget = new EffectTarget(TargetType.Piece, targetPieceRestriction, true, false, condition);
        targetTypes.Add(effectTarget);
    }

    public override void EffectAction(PlayerController player)
    {
        foreach (var target in targets)
        {
            (target as ChessPiece).Kill();
        }
    }
}
