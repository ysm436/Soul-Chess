using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AresEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        Ares aresComponent = gameObject.GetComponent<Ares>();

        //강림 시 버프 목록에 효과 설명 추가
        aresComponent.InfusedPiece.buff.AddBuffByDescription(aresComponent.cardName, Buff.BuffType.Description, "아레스: 모든 기물 사망 시 +" + aresComponent.increasedAD + "/+" + aresComponent.increasedHP + " 부여", true);
        aresComponent.AddEffect(); //실제 효과
    }
}
