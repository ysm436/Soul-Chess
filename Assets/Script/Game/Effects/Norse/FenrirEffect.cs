using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenrirEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        Fenrir fenrirComponent = gameObject.GetComponent<Fenrir>();

        //강림 시 버프 목록에 효과 설명 추가
        fenrirComponent.InfusedPiece.buff.AddBuffByDescription(fenrirComponent.cardName, Buff.BuffType.Description, "펜리르: 적 기물 처치 시 +" + fenrirComponent.increasedAD + "/+"+ fenrirComponent.increasedHP + " 부여", true);

        fenrirComponent.AddEffect(); //실제 효과

        fenrirComponent.InfusedPiece.OnSoulRemoved += RemoveBuffInfo; //영혼 제거 시에만 효과 설명 삭제
    }

    public void RemoveBuffInfo() //버프 목록 삭제 (구속 등 RemoveEffect만 호출되어 효과 비활성화되는 경우 버프 정보는 사라지지 않게)
    {
        gameObject.GetComponent<SoulCard>().InfusedPiece.buff.TryRemoveSpecificBuff(gameObject.GetComponent<SoulCard>().cardName, Buff.BuffType.Description);
    }
}
