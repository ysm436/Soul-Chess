using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behemoth : SoulCard
{
    protected override int CardID => Card.cardIdDict["베헤모스"];

    [HideInInspector] public PlayerController player = null;
    private int buffedAD = 0;
    private int buffedHP = 0;
    public int increasedAD = 1;
    public int increasedHP = 1;

    protected override void Awake()
    {
        base.Awake();
    }

    public void IncreaseStat()
    {
        InfusedPiece.maxHP += increasedHP;
        InfusedPiece.AD += increasedAD;
        buffedHP += increasedHP;
        buffedAD += increasedAD;

        InfusedPiece.buff.AddBuffByValue(cardName, Buff.BuffType.HP, increasedHP, true);
        InfusedPiece.buff.AddBuffByValue(cardName, Buff.BuffType.AD, increasedAD, true);
    }

    public override void AddEffect()
    {
        InfusedPiece.maxHP += buffedHP;
        InfusedPiece.AD += buffedAD;
        InfusedPiece.buff.AddBuffByValue(cardName, Buff.BuffType.HP, buffedHP, true);
        InfusedPiece.buff.AddBuffByValue(cardName, Buff.BuffType.AD, buffedAD, true);
        buffedHP = 0;
        buffedAD = 0;

        if (player != null) player.OnMyTurnEnd += IncreaseStat;
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

        player.OnMyTurnEnd -= IncreaseStat;
    }
}
