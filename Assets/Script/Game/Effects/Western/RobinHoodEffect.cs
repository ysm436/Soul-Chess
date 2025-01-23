using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobinHoodEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        RobinHood robinHoodComponent = gameObject.GetComponent<RobinHood>();

        robinHoodComponent.AddEffect();
        robinHoodComponent.InfusedPiece.buff.AddBuffByDescription(robinHoodComponent.cardName, Buff.BuffType.Description, "로빈 후드: 적 기물 처치 시 그 영혼 카드 획득", true);
        robinHoodComponent.InfusedPiece.OnSoulRemoved += RemoveBuffInfo;
    }

    public void RemoveBuffInfo()
    {
        gameObject.GetComponent<SoulCard>().InfusedPiece.buff.TryRemoveSpecificBuff(gameObject.GetComponent<SoulCard>().cardName, Buff.BuffType.Description);
    }
}
