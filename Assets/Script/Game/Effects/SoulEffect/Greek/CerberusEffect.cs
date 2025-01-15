using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CerberusEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        Cerberus cerberusComponent = gameObject.GetComponent<Cerberus>();

        cerberusComponent.AddEffect();

        cerberusComponent.InfusedPiece.buff.AddBuffByValue(cerberusComponent.cardName, Buff.BuffType.MoveCount, cerberusComponent.increasedMoveCount, true);
        cerberusComponent.InfusedPiece.OnSoulRemoved += RemoveBuffInfo;
    }

    public void RemoveBuffInfo()
    {
        gameObject.GetComponent<SoulCard>().InfusedPiece.buff.TryRemoveSpecificBuff(gameObject.GetComponent<SoulCard>().cardName, Buff.BuffType.MoveCount);
    }
}
