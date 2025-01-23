using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YmirEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        Ymir ymirComponent = gameObject.GetComponent<Ymir>();

        ymirComponent.InfusedPiece.buff.AddBuffByDescription(ymirComponent.cardName, Buff.BuffType.Description, "이미르: 사망 시 패가 가득찰 때까지 카드를 뽑습니다.", true);

        ymirComponent.AddEffect();
        ymirComponent.InfusedPiece.OnSoulRemoved += ymirComponent.RemoveEffect;
    }
}