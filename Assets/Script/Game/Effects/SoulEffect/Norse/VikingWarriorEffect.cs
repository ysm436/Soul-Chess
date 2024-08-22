using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VikingWarriorEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        VikingWarrior vikingwarrior_component = gameObject.GetComponent<VikingWarrior>();

        vikingwarrior_component.AddEffect();
        vikingwarrior_component.InfusedPiece.OnSoulRemoved += vikingwarrior_component.RemoveEffect;
    }
}