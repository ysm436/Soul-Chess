using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behemoth : SoulCard
{
    protected override int CardID => Card.cardIdDict["베헤모스"];

    [HideInInspector] public PlayerController player = null;
    private int buffedStat = 0;
    private int IncreaseAmount = 10;

    protected override void Awake()
    {
        base.Awake();
    }

    public void IncreaseStat()
    {
        InfusedPiece.maxHP += IncreaseAmount;
        InfusedPiece.AD += IncreaseAmount;
        buffedStat += IncreaseAmount;

        InfusedPiece.buff.AddBuffByValue(cardName, Buff.BuffType.HP, IncreaseAmount, true);
        InfusedPiece.buff.AddBuffByValue(cardName, Buff.BuffType.AD, IncreaseAmount, true);
    }

    public override void AddEffect()
    {
        InfusedPiece.maxHP += buffedStat;
        InfusedPiece.AD += buffedStat;
        InfusedPiece.buff.AddBuffByValue(cardName, Buff.BuffType.HP, buffedStat, true);
        InfusedPiece.buff.AddBuffByValue(cardName, Buff.BuffType.AD, buffedStat, true);
        buffedStat = 0;

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

        GameBoard.instance.myController.OnMyTurnEnd -= IncreaseStat;
    }
}
