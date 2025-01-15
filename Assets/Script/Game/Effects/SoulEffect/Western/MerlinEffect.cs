using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MerlinEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        gameObject.GetComponent<Merlin>().AddEffect();
    }
}
