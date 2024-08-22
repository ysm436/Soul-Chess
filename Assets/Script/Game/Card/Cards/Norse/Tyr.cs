using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tyr : SoulCard
{
    protected override int CardID => Card.cardIdDict["티르"];
    int defense_quantity = 10;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void AddEffect()
    {
        InfusedPiece.SetKeyword(Keyword.Type.Taunt);
        InfusedPiece.SetKeyword(Keyword.Type.Defense, defense_quantity);
        //버프 관련 변경 머지 후 버프 추가
        InfusedPiece.buff.AddBuffByKeyword(cardName, Buff.BuffType.Taunt);
        InfusedPiece.buff.AddBuffByValue(cardName, Buff.BuffType.Defense, defense_quantity, true);
    }

    public override void RemoveEffect()
    {
        //defense_quantity = InfusedPiece.GetKeyword(Keyword.Type.Defense);
        InfusedPiece.SetKeyword(Keyword.Type.Taunt, 0);
        InfusedPiece.SetKeyword(Keyword.Type.Defense, 0);
        //버프 관련 변경 머지 후 버프 추가
        InfusedPiece.buff.TryRemoveSpecificBuff(cardName, Buff.BuffType.Taunt);
        InfusedPiece.buff.TryRemoveSpecificBuff(cardName, Buff.BuffType.Defense);
    }
}