using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun.Demo.Cockpit;
using UnityEngine;

public class DavidEffect : TargetingEffect
{
    [SerializeField] private int standardAD = 7;

    ChessPiece.PieceType targetPieceRestriction =
        ChessPiece.PieceType.Pawn |
        ChessPiece.PieceType.Knight |
        ChessPiece.PieceType.Bishop |
        ChessPiece.PieceType.Rook |
        ChessPiece.PieceType.Quene |
        ChessPiece.PieceType.King;

    [SerializeField] private ChessPiece.PieceType additionalBuffPieceType;

    void Awake()
    {
        Predicate<ChessPiece> condition = (ChessPiece piece) =>
        {
            if (gameObject.GetComponent<David>().InfusedPiece.pieceType == additionalBuffPieceType)
            {
                return true;
            }
            else
            {
                return piece.AD >= standardAD;
            }
        };
        
        EffectTarget effectTarget = new EffectTarget(TargetType.Piece, targetPieceRestriction, true, false, condition);
        targetTypes.Add(effectTarget);
    }

    public override void EffectAction(PlayerController player)
    {
        foreach (var target in targets)
        {
            (target as ChessPiece).GetComponent<Animator>().SetTrigger("killedTrigger");
            (target as ChessPiece).MakeAttackedEffect();
            (target as ChessPiece).Kill();
        }
    }
}
