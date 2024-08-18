using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AthenaEffect : Effect
{
    public override void EffectAction()
    {
        Athena athena_component = gameObject.GetComponent<Athena>();
        
        athena_component.AddEffect();
        athena_component.InfusedPiece.OnSoulRemoved += athena_component.RemoveEffect;
    }
}