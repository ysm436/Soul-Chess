using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KingArthur : SoulCard
{
    protected override int CardID => Card.cardIdDict["아서왕"];

    public int multipleAmount = 2;

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