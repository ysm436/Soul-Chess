using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewScytheTankEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        gameObject.GetComponent<SoulCard>().AddEffect();
    }
}
