using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ToneDeafBard : SoulCard
{
    protected override int CardID => Card.cardIdDict["음치 음유시인"];

    [HideInInspector]
    public ChessPiece buffedPiece = null;
    [HideInInspector] public int decreaseAmount = 20;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void AddEffect()
    {
        
    }
    public override void RemoveEffect()
    {
        if (buffedPiece != null && buffedPiece != gameObject.GetComponent<SoulCard>().InfusedPiece)
        {
            //원래 효과
            ///*
            buffedPiece.buff.TryRemoveSpecificBuff(gameObject.GetComponent<SoulCard>().cardName, Buff.BuffType.AD);
            buffedPiece.buff.TryRemoveSpecificBuff(gameObject.GetComponent<SoulCard>().cardName, Buff.BuffType.HP);
            buffedPiece.AD += decreaseAmount;
            buffedPiece.maxHP = (buffedPiece.maxHP-20) > 0 ? (buffedPiece.maxHP-20) : 1;
            //*/
            //아군 구속 테스트용
            //buffedPiece.Unrestraint();

            //아군 침묵 테스트용
            //buffedPiece.SetKeyword(Keyword.Type.Silence, 0);
            //buffedPiece.buff.TryRemoveSpecificBuff(cardName, Buff.BuffType.Silence);
        }
    }
}
