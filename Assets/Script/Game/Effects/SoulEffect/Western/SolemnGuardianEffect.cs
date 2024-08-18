using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SolemnGuardianEffect : Effect
{
    public override void EffectAction()
    {
        string cardName = gameObject.GetComponent<SoulCard>().cardName;

        //보호막, 도발 부여
        gameObject.GetComponent<SoulCard>().InfusedPiece.SetKeyword(Keyword.Type.Shield);
        gameObject.GetComponent<SoulCard>().InfusedPiece.SetKeyword(Keyword.Type.Taunt);

        gameObject.GetComponent<SoulCard>().InfusedPiece.buff.AddBuffByKeyword(cardName, Buff.BuffType.Shield);
        gameObject.GetComponent<SoulCard>().InfusedPiece.buff.AddBuffByKeyword(cardName, Buff.BuffType.Taunt);
    }
}
