using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JackFrostEffect : TargetingEffect
{
    public override void EffectAction(PlayerController player)
    {
        JackFrost jackFrostComponent = gameObject.GetComponent<JackFrost>();

        foreach (var target in targets)
        {
            (target as ChessPiece).SetKeyword(Keyword.Type.Stun);
            (target as ChessPiece).buff.AddBuffByKeyword(jackFrostComponent.cardName, Buff.BuffType.Stun);
        }
    }
}
