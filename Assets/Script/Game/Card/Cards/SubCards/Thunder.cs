using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Thunder : SpellCard
{
    protected override int CardID => Card.cardIdDict["천둥벼락"];

    protected override void Awake()
    {
        base.Awake();
    }
}