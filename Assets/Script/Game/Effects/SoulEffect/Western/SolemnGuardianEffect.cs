using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SolemnGuardianEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        gameObject.GetComponent<SoulCard>().InfusedPiece.SetKeyword(Keyword.Type.Shield);
        gameObject.GetComponent<SoulCard>().InfusedPiece.SetKeyword(Keyword.Type.Taunt);

        gameObject.GetComponent<SoulCard>().InfusedPiece.buff.AddBuffByKeyword(gameObject.GetComponent<SoulCard>().cardName, Buff.BuffType.Shield);
        gameObject.GetComponent<SoulCard>().InfusedPiece.buff.AddBuffByKeyword(gameObject.GetComponent<SoulCard>().cardName, Buff.BuffType.Taunt);
    }
}
