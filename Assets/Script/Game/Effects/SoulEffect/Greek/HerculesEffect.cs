using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HerculesEffect : Effect
{
    public override void EffectAction()
    {
        Hercules hercules_component = gameObject.GetComponent<Hercules>();

        hercules_component.AddEffect();
        hercules_component.InfusedPiece.OnSoulRemoved += hercules_component.RemoveEffect;
    }
}