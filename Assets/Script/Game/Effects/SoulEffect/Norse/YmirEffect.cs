using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YmirEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        Ymir ymir_component = gameObject.GetComponent<Ymir>();

        ymir_component.InfusedPiece.buff.AddBuffByDescription(ymir_component.cardName, Buff.BuffType.Description, "이미르: 사망 시 패가 가득찰 때까지 드로우", true);

        ymir_component.AddEffect();
        ymir_component.InfusedPiece.OnSoulRemoved += ymir_component.RemoveEffect;
        ymir_component.InfusedPiece.OnSoulRemoved += RemoveBuffInfo;
    }

    public void RemoveBuffInfo()
    {
        Ymir ymir_component = gameObject.GetComponent<Ymir>();
        ymir_component.InfusedPiece.buff.TryRemoveSpecificBuff(ymir_component.cardName, Buff.BuffType.Description);
    }
}