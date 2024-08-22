using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HephaestusEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        gameObject.GetComponent<SoulCard>().AddEffect();

        gameObject.GetComponent<SoulCard>().InfusedPiece.buff.AddBuffByDescription(gameObject.GetComponent<SoulCard>().cardName, Buff.BuffType.Description, "헤파이스토스: 이동 후 주위의 모든 기물에게 20 피해", true);
        gameObject.GetComponent<SoulCard>().InfusedPiece.OnSoulRemoved += RemoveBuffInfo;
    }

    public void RemoveBuffInfo()
    {
        gameObject.GetComponent<SoulCard>().InfusedPiece.buff.TryRemoveSpecificBuff(gameObject.GetComponent<SoulCard>().cardName, Buff.BuffType.Description);
    }
}
