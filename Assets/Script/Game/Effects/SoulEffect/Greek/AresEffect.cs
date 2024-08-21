using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AresEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        //강림 시 버프 목록에 효과 설명 추가
        gameObject.GetComponent<SoulCard>().InfusedPiece.buff.AddBuffByDescription(gameObject.GetComponent<SoulCard>().cardName, Buff.BuffType.Description, "아레스: 모든 기물 사망 시 +5/+5 부여", true);

        gameObject.GetComponent<SoulCard>().AddEffect(); //실제 효과

        gameObject.GetComponent<SoulCard>().InfusedPiece.OnSoulRemoved += RemoveBuffInfo; //영혼 제거 시에만 효과 설명 삭제
    }

    public void RemoveBuffInfo() //버프 목록 삭제 (구속 등 RemoveEffect만 호출되어 효과 비활성화되는 경우 버프 정보는 사라지지 않게)
    {
        gameObject.GetComponent<SoulCard>().InfusedPiece.buff.TryRemoveSpecificBuff(gameObject.GetComponent<SoulCard>().cardName, Buff.BuffType.Description);
    }
}
