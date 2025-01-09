using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewScytheTank : SoulCard
{
    protected override int CardID => cardIdDict["신식-낫전차"];

    public int defenseAmount = 10;

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
