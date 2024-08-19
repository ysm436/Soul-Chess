using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YmirEffect : Effect
{
    public override void EffectAction()
    {
        Ymir ymir_component = gameObject.GetComponent<Ymir>();

        ymir_component.AddEffect();
        ymir_component.InfusedPiece.OnSoulRemoved += ymir_component.RemoveEffect;
    }
}