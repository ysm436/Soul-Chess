using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenKnight : SoulCard
{
    protected override int CardID => cardIdDict["녹색 기사"];

    protected override void Awake()
    {
        base.Awake();
    }

    public override void AddEffect()
    {
        InfusedPiece.SetKeyword(Keyword.Type.Taunt);
        InfusedPiece.buff.AddBuffByKeyword(cardName, Buff.BuffType.Taunt);
    }
    public override void RemoveEffect()
    {
        InfusedPiece.SetKeyword(Keyword.Type.Taunt, 0);
        InfusedPiece.buff.TryRemoveSpecificBuff(cardName, Buff.BuffType.Taunt);
    }
}
