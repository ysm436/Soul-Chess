using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VikingWarriorEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        VikingWarrior vikingwarrior_component = gameObject.GetComponent<VikingWarrior>();

        vikingwarrior_component.InfusedPiece.buff.AddBuffByDescription(vikingwarrior_component.cardName, Buff.BuffType.Description, "바이킹 전사: 피해 입은 상태면 공격력 20 증가", true);
        vikingwarrior_component.AddEffect();
        vikingwarrior_component.InfusedPiece.OnSoulRemoved += vikingwarrior_component.RemoveEffect;
        vikingwarrior_component.InfusedPiece.OnSoulRemoved += RemoveBuffInfo;
    }

    public void RemoveBuffInfo()
    {
        VikingWarrior vikingwarrior_component = gameObject.GetComponent<VikingWarrior>();
        vikingwarrior_component.InfusedPiece.buff.TryRemoveSpecificBuff(vikingwarrior_component.cardName, Buff.BuffType.Description);
    }
}