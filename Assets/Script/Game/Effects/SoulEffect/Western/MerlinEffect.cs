using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerlinEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        gameObject.GetComponent<SoulCard>().InfusedPiece.buff.AddBuffByDescription(gameObject.GetComponent<SoulCard>().cardName, Buff.BuffType.Description, "멀린: 자신 마법 카드 피해 2배", true);

        gameObject.GetComponent<Merlin>().player = player;
        gameObject.GetComponent<SoulCard>().AddEffect();

        gameObject.GetComponent<SoulCard>().InfusedPiece.OnSoulRemoved += RemoveBuffInfo;
    }

    public void RemoveBuffInfo()
    {
        gameObject.GetComponent<SoulCard>().InfusedPiece.buff.TryRemoveSpecificBuff(gameObject.GetComponent<SoulCard>().cardName, Buff.BuffType.Description);
    }
}
