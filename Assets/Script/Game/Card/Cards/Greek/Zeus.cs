using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zeus : SoulCard
{
    protected override int CardID => Card.cardIdDict["제우스"];
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