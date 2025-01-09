using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyArmoredInfantry : SoulCard
{
    protected override int CardID => cardIdDict["중기갑 보병"];

    public int DefenseAmount = 5;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void AddEffect()
    {
        InfusedPiece.SetKeyword(Keyword.Type.Defense, DefenseAmount);
        InfusedPiece.buff.AddBuffByValue(cardName, Buff.BuffType.Defense, DefenseAmount, true);
    }
    public override void RemoveEffect()
    {
        InfusedPiece.SetKeyword(Keyword.Type.Defense, -1 * DefenseAmount);
        InfusedPiece.buff.TryRemoveSpecificBuff(cardName, Buff.BuffType.Defense);
    }
}
