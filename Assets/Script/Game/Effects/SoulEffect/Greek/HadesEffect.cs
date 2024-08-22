using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HadesEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        Hades hades_component = gameObject.GetComponent<Hades>();

        hades_component.AddEffect();
        hades_component.InfusedPiece.OnSoulRemoved += hades_component.RemoveEffect;
    }
}