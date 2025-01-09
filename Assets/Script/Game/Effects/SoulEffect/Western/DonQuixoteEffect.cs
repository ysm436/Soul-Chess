using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonQuixoteEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        DonQuixote donQuixoteComponent = gameObject.GetComponent<DonQuixote>();

        donQuixoteComponent.InfusedPiece.SetKeyword(Keyword.Type.Rush);
        donQuixoteComponent.InfusedPiece.buff.AddBuffByKeyword(donQuixoteComponent.cardName, Buff.BuffType.Rush);

        donQuixoteComponent.InfusedPiece.buff.AddBuffByDescription(donQuixoteComponent.cardName, Buff.BuffType.Description, "돈키호테: 공격력 " + donQuixoteComponent.standardAD + " 이상 기물 공격 시 " + donQuixoteComponent.extraAD + " 추가 피해", true);
        donQuixoteComponent.InfuseEffect();
        donQuixoteComponent.InfusedPiece.OnSoulRemoved += RemoveBuffInfo;
    }

    public void RemoveBuffInfo()
    {
        gameObject.GetComponent<DonQuixote>().InfusedPiece.buff.TryRemoveSpecificBuff(gameObject.GetComponent<DonQuixote>().cardName, Buff.BuffType.Description);
    }
}
