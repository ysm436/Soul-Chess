using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlunderShip : SoulCard
{
    protected override int CardID => Card.cardIdDict["약탈선"];

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