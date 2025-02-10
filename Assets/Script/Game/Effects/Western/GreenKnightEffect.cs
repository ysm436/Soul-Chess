using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenKnightEffect : TargetingEffect
{
    [SerializeField] private ChessPiece.PieceType additionalBuffPieceType;

    public override void EffectAction(PlayerController player)
    {
        GreenKnight greenKnightComponent = gameObject.GetComponent<GreenKnight>();

        greenKnightComponent.InfusedPiece.SetKeyword(Keyword.Type.Shield);
        greenKnightComponent.InfusedPiece.buff.AddBuffByKeyword(greenKnightComponent.cardName, Buff.BuffType.Shield);

        foreach (var target in targets)
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

        if (greenKnightComponent.InfusedPiece.pieceType == additionalBuffPieceType)
        {
            greenKnightComponent.InfusedPiece.SetKeyword(Keyword.Type.Shield); //자신에게 보호막
            greenKnightComponent.InfusedPiece.buff.AddBuffByKeyword(greenKnightComponent.cardName, Buff.BuffType.Shield);
        }

    }
}
