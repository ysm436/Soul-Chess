using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UtgardaLokiEffect : TargetingEffect
{
    public override void EffectAction(PlayerController player)
    {
        foreach (var target in targets)
        {
            (target as ChessPiece).SetKeyword(Keyword.Type.Silence);
            (target as ChessPiece).buff.AddBuffByKeyword(gameObject.GetComponent<SoulCard>().cardName, Buff.BuffType.Silence);
        }
    }
}
