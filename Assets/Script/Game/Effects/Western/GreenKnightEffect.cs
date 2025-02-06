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
            (target as ChessPiece).Attack(greenKnightComponent.InfusedPiece);
        }

        if (greenKnightComponent.InfusedPiece.pieceType == additionalBuffPieceType)
        {
            greenKnightComponent.InfusedPiece.SetKeyword(Keyword.Type.Shield); //자신에게 보호막
            greenKnightComponent.InfusedPiece.buff.AddBuffByKeyword(greenKnightComponent.cardName, Buff.BuffType.Shield);
        }

    }
}
