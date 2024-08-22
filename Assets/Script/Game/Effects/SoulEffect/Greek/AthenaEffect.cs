using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AthenaEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        Athena athena_component = gameObject.GetComponent<Athena>();

        athena_component.InfusedPiece.buff.AddBuffByDescription(athena_component.cardName, Buff.BuffType.Description, "아테나: 다른 아군 기물 +10/+10 부여", true);
        athena_component.AddEffect();
        athena_component.InfusedPiece.OnSoulRemoved += RemoveBuffInfo;
    }

    public void RemoveBuffInfo()
    {
        Athena athena_component = gameObject.GetComponent<Athena>();
        athena_component.InfusedPiece.buff.TryRemoveSpecificBuff(athena_component.cardName, Buff.BuffType.Description);
    }
}