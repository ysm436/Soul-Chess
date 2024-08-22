using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HerculesEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        Hercules hercules_component = gameObject.GetComponent<Hercules>();
        hercules_component.playercontroller = player;

        hercules_component.InfusedPiece.buff.AddBuffByDescription(hercules_component.cardName, Buff.BuffType.Description, "헤라클레스: 본인 턴 동안 공격력 2배", true);
        hercules_component.AddEffect();
        hercules_component.InfusedPiece.OnSoulRemoved += hercules_component.RemoveEffect;
        hercules_component.InfusedPiece.OnSoulRemoved += RemoveBuffInfo;
    }

    public void RemoveBuffInfo()
    {
        Hercules hercules_component = gameObject.GetComponent<Hercules>();
        hercules_component.InfusedPiece.buff.TryRemoveSpecificBuff(hercules_component.cardName, Buff.BuffType.Description);
    }
}