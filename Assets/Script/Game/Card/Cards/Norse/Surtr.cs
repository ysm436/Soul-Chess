using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Surtr : SoulCard
{
    protected override int CardID => Card.cardIdDict["수르트"];

    [HideInInspector] public PlayerController player = null;
    
    [SerializeField] private int decreaseCost = 1;

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
            cost -= decreaseCost;
            
            if (cost < 0)
                cost = 0;
        }
    }

    public override void AddEffect()
    {

    }
    public override void RemoveEffect()
    {

    }
}
