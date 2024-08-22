using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HeavyArmoredInfantryEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        gameObject.GetComponent<SoulCard>().AddEffect();
    }
}
