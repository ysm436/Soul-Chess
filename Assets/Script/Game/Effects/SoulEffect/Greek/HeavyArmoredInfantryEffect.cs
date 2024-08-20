using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HeavyArmoredInfantryEffect : Effect
{
    public override void EffectAction()
    {
        gameObject.GetComponent<SoulCard>().InfusedPiece.SetKeyword(Keyword.Type.Defense, 5);
        gameObject.GetComponent<SoulCard>().InfusedPiece.buff.AddBuffByValue(gameObject.GetComponent<SoulCard>().cardName, Buff.BuffType.Defense, 5, true);
    }
}
