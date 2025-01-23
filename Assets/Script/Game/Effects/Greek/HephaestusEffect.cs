using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HephaestusEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        Hephaestus HephaestusComponent = gameObject.GetComponent<Hephaestus>();

        HephaestusComponent.AddEffect();
    }
}
