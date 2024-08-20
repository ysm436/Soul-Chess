using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Plunderer : SoulCard
{
    protected override int CardID => Card.cardIdDict["약탈자"];

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