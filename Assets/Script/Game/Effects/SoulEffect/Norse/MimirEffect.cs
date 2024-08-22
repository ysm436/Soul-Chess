using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MimirEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        Mimir mimir_component = gameObject.GetComponent<Mimir>();

        mimir_component.InfusedPiece.buff.AddBuffByDescription(mimir_component.cardName, Buff.BuffType.Description, "미미르: 내 패의 카드 비용 1 감소", true);

        mimir_component.AddEffect();
        mimir_component.InfusedPiece.OnSoulRemoved += mimir_component.RemoveEffect;
        mimir_component.InfusedPiece.OnSoulRemoved += RemoveBuffInfo;
    }

    public void RemoveBuffInfo()
    {
        Mimir mimir_component = gameObject.GetComponent<Mimir>();
        mimir_component.InfusedPiece.buff.TryRemoveSpecificBuff(mimir_component.cardName, Buff.BuffType.Description);
    }
}