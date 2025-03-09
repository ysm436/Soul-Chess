using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenKnightEffect : TargetingEffect
{
    [SerializeField] private ChessPiece.PieceType additionalBuffPieceType;
    private ChessPiece.PieceType targetPieceRestriction =
            ChessPiece.PieceType.Pawn |
            ChessPiece.PieceType.Knight |
            ChessPiece.PieceType.Bishop |
            ChessPiece.PieceType.Rook |
            ChessPiece.PieceType.Quene;

    SoulCard soul = null;

    void Awake()
    {
        if (soul == null) soul = gameObject.GetComponent<SoulCard>();
        Predicate<ChessPiece> condition = (ChessPiece piece) => (piece.soul != null) && (piece != soul.InfusedPiece);
        EffectTarget effectTarget = new EffectTarget(TargetType.Piece, targetPieceRestriction, true, false, condition);
        targetTypes.Add(effectTarget);
    }

    public override void EffectAction(PlayerController player)
    {
        GreenKnight greenKnightComponent = gameObject.GetComponent<GreenKnight>();

        if (greenKnightComponent.InfusedPiece.pieceType == additionalBuffPieceType)
        {
            greenKnightComponent.InfusedPiece.SetKeyword(Keyword.Type.Shield); //자신에게 보호막
            greenKnightComponent.InfusedPiece.buff.AddBuffByKeyword(greenKnightComponent.cardName, Buff.BuffType.Shield);
        }

        foreach (var target in targets)
        {
            ChessPiece greenKnightPiece = greenKnightComponent.InfusedPiece;
            ChessPiece targetPiece = target as ChessPiece;

            targetPiece.Attack(greenKnightComponent.InfusedPiece);

            targetPiece.GetComponent<Animator>().SetTrigger("moveTrigger");
            targetPiece.GetComponent<Animator>().SetBool("isReturning", true);
            GameBoard.instance.chessBoard.ForthBackPieceAnimation(targetPiece, greenKnightPiece);

            greenKnightPiece.Attack(targetPiece);

            greenKnightPiece.GetComponent<Animator>().SetTrigger("moveTrigger");
            greenKnightPiece.GetComponent<Animator>().SetBool("isReturning", true);
            GameBoard.instance.chessBoard.ForthBackPieceAnimation(greenKnightPiece, targetPiece);
        }

    }
    /*foreach (var target in targets)
    {
        if ((target as ChessPiece).Attack(greenKnightComponent.InfusedPiece))
        {
            (target as ChessPiece).Move(greenKnightComponent.InfusedPiece.coordinate);
            GameBoard.instance.chessBoard.KillAnimation((target as ChessPiece), greenKnightComponent.InfusedPiece);
        }
        else
        {
            (target as ChessPiece).GetComponent<Animator>().SetTrigger("moveTrigger");
            (target as ChessPiece).GetComponent<Animator>().SetBool("isReturning", true);
            GameBoard.instance.chessBoard.ForthBackPieceAnimation((target as ChessPiece), greenKnightComponent.InfusedPiece);
        }
    }
}*/
}
