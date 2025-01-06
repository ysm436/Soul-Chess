using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Poseidon : SoulCard
{
    protected override int CardID => Card.cardIdDict["포세이돈"];
    public int damageAmount = 25;
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
