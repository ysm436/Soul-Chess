using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zeus : SoulCard
{
    protected override int CardID => Card.cardIdDict["제우스"];

    public int cardAmount = 3;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void AddEffect()
    {
    }

    public override void RemoveEffect()
    {
    }
}