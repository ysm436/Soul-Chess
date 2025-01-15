using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SolemnGuardianEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        SolemnGuardian solemnGuardianComponent = gameObject.GetComponent<SolemnGuardian>();

        solemnGuardianComponent.InfusedPiece.SetKeyword(Keyword.Type.Shield);
        solemnGuardianComponent.InfusedPiece.buff.AddBuffByKeyword(solemnGuardianComponent.cardName, Buff.BuffType.Shield);
    }
}
