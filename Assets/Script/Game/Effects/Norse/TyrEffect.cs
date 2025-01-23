using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TyrEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        Tyr tyrComponent = gameObject.GetComponent<Tyr>();

        tyrComponent.AddEffect();
    }
}