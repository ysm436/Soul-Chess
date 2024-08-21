using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HydraEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        gameObject.GetComponent<SoulCard>().AddEffect();
    }
}
