using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VikingWarriorEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        VikingWarrior vikingwarriorComponent = gameObject.GetComponent<VikingWarrior>();

        vikingwarriorComponent.InfusedPiece.buff.AddBuffByDescription(vikingwarriorComponent.cardName, Buff.BuffType.Description, "바이킹 전사: 피해 입은 상태면 공격력 " + vikingwarriorComponent.increasedAD +" 증가", true);
        vikingwarriorComponent.AddEffect();
        vikingwarriorComponent.InfusedPiece.OnSoulRemoved += RemoveBuffInfo;
    }

    public void RemoveBuffInfo()
    {
        VikingWarrior vikingwarriorComponent = gameObject.GetComponent<VikingWarrior>();
        vikingwarriorComponent.InfusedPiece.buff.TryRemoveSpecificBuff(vikingwarriorComponent.cardName, Buff.BuffType.Description);
    }
}