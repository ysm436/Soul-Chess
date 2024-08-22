using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AbelEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        Abel abel_component = gameObject.GetComponent<Abel>();

        abel_component.AddEffect();
        abel_component.InfusedPiece.OnSoulRemoved += abel_component.RemoveEffect;
    }
}