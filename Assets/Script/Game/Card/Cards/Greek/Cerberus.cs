using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cerberus : SoulCard
{
    protected override int CardID => Card.cardIdDict["케르베로스"];

    public int increasedMoveCount = 2;
    protected override void Awake()
    {
        base.Awake();
    }

    public override void AddEffect()
    {
        InfusedPiece.moveCount += increasedMoveCount;
        InfusedPiece.OnSoulRemoved += RemoveEffect;
    }

    public override void RemoveEffect()
    {
        InfusedPiece.moveCount -= increasedMoveCount;
    }
}
