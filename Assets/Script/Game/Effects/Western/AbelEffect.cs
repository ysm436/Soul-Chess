using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AbelEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        Abel abelComponent = gameObject.GetComponent<Abel>();

        abelComponent.InfusedPiece.buff.AddBuffByDescription(abelComponent.cardName, Buff.BuffType.Description, "아벨: [유언] 이 기물이 기물에게 공격당해 사망 시 해당 기물 파괴", true);
        abelComponent.AddEffect();
        abelComponent.InfusedPiece.OnSoulRemoved += abelComponent.RemoveEffect;
        abelComponent.InfusedPiece.OnSoulRemoved += RemoveBuffInfo;
    }

    public void RemoveBuffInfo()
    {
        Abel abelComponent = gameObject.GetComponent<Abel>();
        abelComponent.InfusedPiece.buff.TryRemoveSpecificBuff(abelComponent.cardName, Buff.BuffType.Description);
    }
}