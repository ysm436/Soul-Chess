using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehemothEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        Behemoth behemothComponent = gameObject.GetComponent<Behemoth>();
        behemothComponent.player = player;

        behemothComponent.InfusedPiece.buff.AddBuffByDescription(behemothComponent.cardName, Buff.BuffType.Description, "베헤모스: 자신 턴 종료 시 +" + behemothComponent.increasedAD + "/+" + behemothComponent.increasedHP + " 획득", true);
        behemothComponent.AddEffect();
        behemothComponent.InfusedPiece.OnSoulRemoved += RemoveBuffInfo;
    }

    public void RemoveBuffInfo()
    {
        Behemoth behemoth_component = gameObject.GetComponent<Behemoth>();
        behemoth_component.InfusedPiece.buff.TryRemoveSpecificBuff(behemoth_component.cardName, Buff.BuffType.Description);
    }
}
