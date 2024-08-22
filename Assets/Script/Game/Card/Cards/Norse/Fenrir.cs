using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Fenrir : SoulCard
{
    protected override int CardID => Card.cardIdDict["펜리르"];

    private int IncreaseAmountAD = 20;
    private int IncreaseAmountHP = 20;
    private int buffedStat = 0;

    protected override void Awake()
    {
        base.Awake();
    }

    private void IncreaseStat(ChessPiece chessPiece)
    {
        InfusedPiece.AD += IncreaseAmountAD;
        InfusedPiece.maxHP += IncreaseAmountHP;
        buffedStat += IncreaseAmountAD;

        InfusedPiece.buff.AddBuffByValue(this.cardName, Buff.BuffType.AD, IncreaseAmountAD, true);
        InfusedPiece.buff.AddBuffByValue(this.cardName, Buff.BuffType.HP, IncreaseAmountHP, true);
    }

    public override void AddEffect()
    {
        InfusedPiece.AD += buffedStat;
        InfusedPiece.maxHP += buffedStat;
        InfusedPiece.buff.AddBuffByValue(this.cardName, Buff.BuffType.AD, buffedStat, true);
        InfusedPiece.buff.AddBuffByValue(this.cardName, Buff.BuffType.HP, buffedStat, true);
        buffedStat = 0;

        InfusedPiece.OnKill += IncreaseStat;
        InfusedPiece.OnSoulRemoved += RemoveEffect;
    }

    public override void RemoveEffect()
    {
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
        InfusedPiece.OnKill -= IncreaseStat;
    }
}
