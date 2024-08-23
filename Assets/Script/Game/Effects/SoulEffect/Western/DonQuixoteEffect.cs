using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonQuixoteEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        gameObject.GetComponent<DonQuixote>().InfusedPiece.SetKeyword(Keyword.Type.Rush);
        gameObject.GetComponent<DonQuixote>().InfusedPiece.buff.AddBuffByKeyword(gameObject.GetComponent<DonQuixote>().cardName, Buff.BuffType.Rush);

        gameObject.GetComponent<DonQuixote>().InfusedPiece.buff.AddBuffByDescription(gameObject.GetComponent<DonQuixote>().cardName, Buff.BuffType.Description, "돈키호테: 공격력 100 이상 기물 공격 시 40 추가 피해", true);
        gameObject.GetComponent<DonQuixote>().InfuseEffect();
        gameObject.GetComponent<DonQuixote>().InfusedPiece.OnSoulRemoved += RemoveBuffInfo;
    }

    public void RemoveBuffInfo()
    {
        gameObject.GetComponent<DonQuixote>().InfusedPiece.buff.TryRemoveSpecificBuff(gameObject.GetComponent<DonQuixote>().cardName, Buff.BuffType.Description);
    }
}
