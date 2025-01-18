using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MimirEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        Mimir mimirComponent = gameObject.GetComponent<Mimir>();

        mimirComponent.InfusedPiece.buff.AddBuffByDescription(mimirComponent.cardName, Buff.BuffType.Description, "미미르: 내 패의 카드 비용 "+ mimirComponent.reductionCost +" 감소", true);

        mimirComponent.AddEffect();
        
        mimirComponent.InfusedPiece.OnSoulRemoved += RemoveBuffInfo;
    }

    public void RemoveBuffInfo()
    {
        Mimir mimirComponent = gameObject.GetComponent<Mimir>();
        mimirComponent.InfusedPiece.buff.TryRemoveSpecificBuff(mimirComponent.cardName, Buff.BuffType.Description);
    }
}