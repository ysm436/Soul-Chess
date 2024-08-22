using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyArmoredInfantry : SoulCard
{
    protected override int CardID => cardIdDict["중기갑 보병"];

    protected override void Awake()
    {
        base.Awake();
    }

    public override void AddEffect()
    {
        InfusedPiece.SetKeyword(Keyword.Type.Defense, 5);
        InfusedPiece.buff.AddBuffByValue(cardName, Buff.BuffType.Defense, 5, true);
    }
    public override void RemoveEffect()
    {
        InfusedPiece.SetKeyword(Keyword.Type.Defense, -5);
        InfusedPiece.buff.TryRemoveSpecificBuff(cardName, Buff.BuffType.Defense);
    }
}
