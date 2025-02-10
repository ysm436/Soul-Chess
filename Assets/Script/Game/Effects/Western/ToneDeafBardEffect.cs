using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ToneDeafBardEffect : TargetingEffect
{
    [SerializeField] private int changeAD;
    [SerializeField] private int changeHP;

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

        foreach (var target in targets)
        {
            GameBoard.instance.chessBoard.TileEffect(effectPrefab, target as ChessPiece);
            (target as ChessPiece).AD += changeAD;
            (target as ChessPiece).maxHP += changeHP;

            (target as ChessPiece).buff.AddBuffByValue(gameObject.GetComponent<SoulCard>().cardName, Buff.BuffType.AD, changeAD, true);
            (target as ChessPiece).buff.AddBuffByValue(gameObject.GetComponent<SoulCard>().cardName, Buff.BuffType.HP, changeHP, true);
        }
    }
}
