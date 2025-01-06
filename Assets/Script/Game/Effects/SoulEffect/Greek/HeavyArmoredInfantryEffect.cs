using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HeavyArmoredInfantryEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        HeavyArmoredInfantry HeavyArmoredInfantryComponent = gameObject.GetComponent<HeavyArmoredInfantry>();

        HeavyArmoredInfantryComponent.InfusedPiece.SetKeyword(Keyword.Type.Defense, HeavyArmoredInfantryComponent.DefenseAmount);
        HeavyArmoredInfantryComponent.InfusedPiece.buff.AddBuffByValue(HeavyArmoredInfantryComponent.cardName, Buff.BuffType.Defense, HeavyArmoredInfantryComponent.DefenseAmount, true);
    }
}
