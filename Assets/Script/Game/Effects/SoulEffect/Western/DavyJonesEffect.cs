using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DavyJonesEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        DavyJones davyJonesComponent = gameObject.GetComponent<DavyJones>();

        davyJonesComponent.InfusedPiece.buff.AddBuffByDescription(davyJonesComponent.cardName, Buff.BuffType.Description, "데비 존스: 죽은 아군 수 만큼 +" + davyJonesComponent.increasedAD + "/+" + davyJonesComponent.increasedHP + " 증가", true);

        davyJonesComponent.AddEffect();
        davyJonesComponent.InfusedPiece.OnSoulRemoved += RemoveBuffInfo;
    }

    public void RemoveBuffInfo()
    {
        gameObject.GetComponent<SoulCard>().InfusedPiece.buff.TryRemoveSpecificBuff(gameObject.GetComponent<SoulCard>().cardName, Buff.BuffType.Description);
    }
}
