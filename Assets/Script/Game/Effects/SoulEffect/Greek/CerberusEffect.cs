using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CerberusEffect : Effect
{
    public override void EffectAction()
    {
        gameObject.GetComponent<SoulCard>().AddEffect();

        gameObject.GetComponent<SoulCard>().InfusedPiece.buff.AddBuffByValue(gameObject.GetComponent<SoulCard>().cardName, Buff.BuffType.MoveCount, 2, true);
        gameObject.GetComponent<SoulCard>().InfusedPiece.OnSoulRemoved += RemoveBuffInfo;
    }

    public void RemoveBuffInfo()
    {
        gameObject.GetComponent<SoulCard>().InfusedPiece.buff.TryRemoveSpecificBuff(gameObject.GetComponent<SoulCard>().cardName, Buff.BuffType.MoveCount);
    }
}
