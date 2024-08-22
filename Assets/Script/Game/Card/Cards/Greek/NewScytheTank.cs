using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewScytheTank : SoulCard
{
    protected override int CardID => cardIdDict["신식-낫전차"];

    protected override void Awake()
    {
        base.Awake();
    }

    public override void AddEffect()
    {
        InfusedPiece.SetKeyword(Keyword.Type.Defense, 10);
        InfusedPiece.SetKeyword(Keyword.Type.Shield);

        InfusedPiece.buff.AddBuffByValue(cardName, Buff.BuffType.Defense, 10, true);
        InfusedPiece.buff.AddBuffByKeyword(cardName, Buff.BuffType.Shield);

        InfusedPiece.OnSoulRemoved += RemoveEffect;
    }

    public override void RemoveEffect()
    {
        InfusedPiece.SetKeyword(Keyword.Type.Defense, -10);
        InfusedPiece.SetKeyword(Keyword.Type.Shield, 0);

        InfusedPiece.buff.TryRemoveSpecificBuff(cardName, Buff.BuffType.Defense);
        InfusedPiece.buff.TryRemoveSpecificBuff(cardName, Buff.BuffType.Shield);
    }
}
