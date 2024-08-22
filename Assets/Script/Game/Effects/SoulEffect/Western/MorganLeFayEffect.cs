using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorganLeFayEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        MorganLeFay morganLeFay = gameObject.GetComponent<MorganLeFay>();
        morganLeFay.player = player;
        morganLeFay.InfusedPiece.buff.AddBuffByDescription(morganLeFay.cardName, Buff.BuffType.Description, "모르건 르 페이: 자신 턴 종료 시 무작위 아군 기물 회복", true);
        morganLeFay.AddEffect();
        morganLeFay.InfusedPiece.OnSoulRemoved += RemoveBuffInfo;
    }

    public void RemoveBuffInfo()
    {
        MorganLeFay morganLeFay = gameObject.GetComponent<MorganLeFay>();
        morganLeFay.InfusedPiece.buff.TryRemoveSpecificBuff(morganLeFay.cardName, Buff.BuffType.Description);
    }
}
