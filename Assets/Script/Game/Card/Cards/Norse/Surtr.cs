using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Surtr : SoulCard
{
    protected override int CardID => Card.cardIdDict["수르트"];

    [HideInInspector] public PlayerController player = null;

    protected override void Awake()
    {
        base.Awake();

        GameBoard.instance.whiteController.OnMyTurnEnd += DecreaseCost;
        GameBoard.instance.blackController.OnMyTurnEnd += DecreaseCost;
    }

    private void OnDisable()
    {
        GameBoard.instance.whiteController.OnMyTurnEnd -= DecreaseCost;
        GameBoard.instance.blackController.OnMyTurnEnd -= DecreaseCost;
    }

    public void DecreaseCost()
    {
        if (cost > 0)
        {
            cost--;
        }
    }

    public override void AddEffect()
    {

    }
    public override void RemoveEffect()
    {

    }
}
