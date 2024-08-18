using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cerberus : SoulCard
{
    protected override int CardID => Card.cardIdDict["케르베로스"];
    protected override void Awake()
    {
        base.Awake();
    }

    public override void AddEffect()
    {
        InfusedPiece.moveCount += 2;
        InfusedPiece.buff.AddBuffByValue(gameObject.GetComponent<SoulCard>().cardName, Buff.BuffType.MoveCount, 2, true);
    }

    public override void RemoveEffect()
    {
        InfusedPiece.moveCount -= 2;
        InfusedPiece.buff.TryRemoveSpecificBuff(cardName, Buff.BuffType.MoveCount);
    }
}
