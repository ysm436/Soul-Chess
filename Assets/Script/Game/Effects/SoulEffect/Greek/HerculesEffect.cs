using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HerculesEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        Hercules herculesComponent = gameObject.GetComponent<Hercules>();
        herculesComponent.playercontroller = player;

        herculesComponent.InfusedPiece.buff.AddBuffByDescription(herculesComponent.cardName, Buff.BuffType.Description, "헤라클레스: 본인 턴 동안 공격력 " + herculesComponent.multipleAD +"배", true);
        herculesComponent.AddEffect();
        
        herculesComponent.InfusedPiece.OnSoulRemoved += RemoveBuffInfo;
    }

    public void RemoveBuffInfo()
    {
        Hercules herculesComponent = gameObject.GetComponent<Hercules>();
        herculesComponent.InfusedPiece.buff.TryRemoveSpecificBuff(herculesComponent.cardName, Buff.BuffType.Description);
    }
}