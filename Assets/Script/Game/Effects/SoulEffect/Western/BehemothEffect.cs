using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehemothEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        Behemoth behemoth_component = gameObject.GetComponent<Behemoth>();
        behemoth_component.player = player;

        behemoth_component.InfusedPiece.buff.AddBuffByDescription(behemoth_component.cardName, Buff.BuffType.Description, "베헤모스: 자신 턴 종료 시 +10/+10 획득", true);
        behemoth_component.AddEffect();
        behemoth_component.InfusedPiece.OnSoulRemoved += RemoveBuffInfo;
    }

    public void RemoveBuffInfo()
    {
        Behemoth behemoth_component = gameObject.GetComponent<Behemoth>();
        behemoth_component.InfusedPiece.buff.TryRemoveSpecificBuff(behemoth_component.cardName, Buff.BuffType.Description);
    }
}
