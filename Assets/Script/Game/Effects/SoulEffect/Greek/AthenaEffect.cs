using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AthenaEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        Athena athenaComponent = gameObject.GetComponent<Athena>();

        athenaComponent.InfusedPiece.buff.AddBuffByDescription(athenaComponent.cardName, Buff.BuffType.Description, "아테나: 다른 아군 기물 +" + athenaComponent.increasedAD + "/+" + athenaComponent.increasedHP + " 부여", true);
        athenaComponent.AddEffect();
        athenaComponent.InfusedPiece.OnSoulRemoved += RemoveBuffInfo;
    }

    public void RemoveBuffInfo()
    {
        Athena athena_component = gameObject.GetComponent<Athena>();
        athena_component.InfusedPiece.buff.TryRemoveSpecificBuff(athena_component.cardName, Buff.BuffType.Description);
    }
}