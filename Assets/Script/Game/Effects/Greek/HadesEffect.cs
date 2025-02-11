using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HadesEffect : TargetingEffect
{
    ChessPiece.PieceType targetPieceRestriction =
        ChessPiece.PieceType.Pawn |
        ChessPiece.PieceType.Knight |
        ChessPiece.PieceType.Bishop |
        ChessPiece.PieceType.Rook |
        ChessPiece.PieceType.Quene |
        ChessPiece.PieceType.King;

    SoulCard soul = null;

    private void Awake()
    {
        if (soul == null) soul = gameObject.GetComponent<SoulCard>();
        Predicate<ChessPiece> condition = (ChessPiece piece) => piece.soul != null &&  piece.maxHP > piece.GetHP;
        EffectTarget effectTarget = new EffectTarget(TargetType.Piece, targetPieceRestriction, true, false, condition);
        targetTypes.Add(effectTarget);
    }

    public override void EffectAction(PlayerController player)
    {
        Hades hadesComponent = GetComponent<Hades>();

        foreach (var target in targets)
        {
            GameBoard.instance.chessBoard.KillByCardEffect(effectPrefab, hadesComponent.InfusedPiece, target as ChessPiece);
        }
    }
}