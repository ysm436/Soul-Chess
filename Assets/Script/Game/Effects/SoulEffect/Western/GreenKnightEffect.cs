using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenKnightEffect : TargetingEffect
{
    public override void EffectAction(PlayerController player)
    {
        GreenKnight greenKnightComponent = gameObject.GetComponent<GreenKnight>();

        greenKnightComponent.InfusedPiece.SetKeyword(Keyword.Type.Shield);
        greenKnightComponent.InfusedPiece.buff.AddBuffByKeyword(greenKnightComponent.cardName, Buff.BuffType.Shield);

        foreach (var target in targets)
        {
            (target as ChessPiece).Attack(greenKnightComponent.InfusedPiece);
        }
    }
}
