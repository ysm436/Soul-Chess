using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CerberusEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        Cerberus cerberusComponent = gameObject.GetComponent<Cerberus>();

        cerberusComponent.AddEffect();
    }
}
