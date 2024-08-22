using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AbelEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        Abel abel_component = gameObject.GetComponent<Abel>();

        abel_component.InfusedPiece.buff.AddBuffByDescription(abel_component.cardName, Buff.BuffType.Description, "아벨: [유언] 이 기물이 기물에게 공격당해 사망 시 해당 기물 파괴", true);
        abel_component.AddEffect();
        abel_component.InfusedPiece.OnSoulRemoved += abel_component.RemoveEffect;
        abel_component.InfusedPiece.OnSoulRemoved += RemoveBuffInfo;
    }

    public void RemoveBuffInfo()
    {
        Abel abel_component = gameObject.GetComponent<Abel>();
        abel_component.InfusedPiece.buff.TryRemoveSpecificBuff(abel_component.cardName, Buff.BuffType.Description);
    }
}