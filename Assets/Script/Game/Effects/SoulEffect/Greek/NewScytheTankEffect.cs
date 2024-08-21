using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewScytheTankEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        gameObject.GetComponent<SoulCard>().InfusedPiece.SetKeyword(Keyword.Type.Defense, 10);
        gameObject.GetComponent<SoulCard>().InfusedPiece.SetKeyword(Keyword.Type.Shield);

        gameObject.GetComponent<SoulCard>().InfusedPiece.buff.AddBuffByValue(gameObject.GetComponent<SoulCard>().cardName, Buff.BuffType.Defense, 10, true);
        gameObject.GetComponent<SoulCard>().InfusedPiece.buff.AddBuffByKeyword(gameObject.GetComponent<SoulCard>().cardName, Buff.BuffType.Shield);
    }
}
