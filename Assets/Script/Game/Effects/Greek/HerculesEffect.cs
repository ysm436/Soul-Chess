using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HerculesEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        Hercules herculesComponent = gameObject.GetComponent<Hercules>();
        herculesComponent.AddEffect();
    }
}