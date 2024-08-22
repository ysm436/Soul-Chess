using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DavyJonesEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        gameObject.GetComponent<SoulCard>().InfusedPiece.buff.AddBuffByDescription(gameObject.GetComponent<SoulCard>().cardName, Buff.BuffType.Description, "데비 존스: 죽은 아군 수 만큼 +10/+10 증가", true);

        gameObject.GetComponent<SoulCard>().AddEffect();
        gameObject.GetComponent<SoulCard>().InfusedPiece.OnSoulRemoved += RemoveBuffInfo;
    }

    public void RemoveBuffInfo()
    {
        gameObject.GetComponent<SoulCard>().InfusedPiece.buff.TryRemoveSpecificBuff(gameObject.GetComponent<SoulCard>().cardName, Buff.BuffType.Description);
    }
}
