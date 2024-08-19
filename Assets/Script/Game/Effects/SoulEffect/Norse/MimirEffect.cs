using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MimirEffect : Effect
{
    public override void EffectAction()
    {
        Mimir mimir_component = gameObject.GetComponent<Mimir>();

        mimir_component.AddEffect();
        mimir_component.InfusedPiece.OnSoulRemoved += mimir_component.RemoveEffect;
    }
}