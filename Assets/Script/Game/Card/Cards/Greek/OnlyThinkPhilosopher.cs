using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyThinkPhilosopher : SoulCard
{
    protected override int CardID => Card.cardIdDict["생각뿐인 철학자"];
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
