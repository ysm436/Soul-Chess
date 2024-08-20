using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CerberusEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        gameObject.GetComponent<SoulCard>().InfusedPiece.moveCount += 2;
        gameObject.GetComponent<SoulCard>().InfusedPiece.buff.AddBuffByValue(gameObject.GetComponent<SoulCard>().cardName, Buff.BuffType.MoveCount, 2, true);
        gameObject.GetComponent<SoulCard>().InfusedPiece.OnSoulRemoved += gameObject.GetComponent<SoulCard>().RemoveEffect;
    }
}
