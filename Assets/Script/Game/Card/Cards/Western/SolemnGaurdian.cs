using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SolemnGuardian : SoulCard
{
    protected override int CardID => cardIdDict["근엄한 경비병"];

    protected override void Awake()
    {
        base.Awake();
    }

    public override void AddEffect()
    {
        InfusedPiece.SetKeyword(Keyword.Type.Shield);
        InfusedPiece.SetKeyword(Keyword.Type.Taunt);

        InfusedPiece.buff.AddBuffByKeyword(cardName, Buff.BuffType.Shield);
        InfusedPiece.buff.AddBuffByKeyword(cardName, Buff.BuffType.Taunt);
    }
    public override void RemoveEffect()
    {
        InfusedPiece.SetKeyword(Keyword.Type.Shield, 0);
        InfusedPiece.SetKeyword(Keyword.Type.Taunt, 0);

        InfusedPiece.buff.TryRemoveSpecificBuff(cardName, Buff.BuffType.Shield);
        InfusedPiece.buff.TryRemoveSpecificBuff(cardName, Buff.BuffType.Taunt);
    }
}
