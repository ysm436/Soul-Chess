using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loki : SoulCard
{
    protected override int CardID => cardIdDict["로키"];

    [HideInInspector] public int targetPieceAD = -1000;
    [HideInInspector] public int targetPieceHP = -1000;
    [HideInInspector] public SoulCard targetSoul = null;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void AddEffect()
    {
        if (targetPieceAD == -1000 && targetPieceHP == -1000) return;

        //선택한 기물 스탯만큼 버프 획득
        InfusedPiece.AD += targetPieceAD;
        InfusedPiece.maxHP += targetPieceHP;

        InfusedPiece.buff.AddBuffByValue(cardName, Buff.BuffType.AD, targetPieceAD, true);
        InfusedPiece.buff.AddBuffByValue(cardName, Buff.BuffType.HP, targetPieceHP, true);

        //선택한 기물 영혼이 있다면 영혼 효과도 여기에서 실행
        if (targetSoul != null)
        {
            targetSoul.AddEffect();
        }
    }
    public override void RemoveEffect()
    {
        //배낀 영혼 효과부터 제거
        if (targetSoul != null) targetSoul.RemoveEffect();

        //버프 제거
        for(int i = InfusedPiece.buff.buffList.Count-1; i >= 0; i--)
        {
            Buff.BuffInfo buffInfo = InfusedPiece.buff.buffList[i];
            if (buffInfo.sourceName == cardName)
            {
                if (buffInfo.buffType == Buff.BuffType.AD)
                {
                    InfusedPiece.AD -= buffInfo.value;
                    InfusedPiece.buff.TryRemoveSpecificBuff(cardName, Buff.BuffType.AD);
                }
                else if (buffInfo.buffType == Buff.BuffType.HP)
                {
                    InfusedPiece.maxHP = (InfusedPiece.maxHP-buffInfo.value) > 0 ? InfusedPiece.maxHP-buffInfo.value : 1;
                    InfusedPiece.buff.TryRemoveSpecificBuff(cardName, Buff.BuffType.HP);
                }
            }
        }
    }
}
