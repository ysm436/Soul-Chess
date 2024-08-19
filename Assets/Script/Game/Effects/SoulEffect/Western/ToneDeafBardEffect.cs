using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ToneDeafBardEffect : TargetingEffect
{
    public override void EffectAction(PlayerController player)
    {
        int ADchange = -20;
        int maxHPchange = 20;

        foreach (var target in targets)
        {
            (target as ChessPiece).AD -= 20;
            (target as ChessPiece).maxHP += 20;

            (target as ChessPiece).buff.AddBuffByValue(gameObject.GetComponent<SoulCard>().cardName, Buff.BuffType.AD, ADchange, true);
            (target as ChessPiece).buff.AddBuffByValue(gameObject.GetComponent<SoulCard>().cardName, Buff.BuffType.HP, maxHPchange, true);
            gameObject.GetComponent<ToneDeafBard>().buffedPiece = target as ChessPiece;
        }
        gameObject.GetComponent<SoulCard>().InfusedPiece.OnSoulRemoved += gameObject.GetComponent<SoulCard>().RemoveEffect;
    }
}
