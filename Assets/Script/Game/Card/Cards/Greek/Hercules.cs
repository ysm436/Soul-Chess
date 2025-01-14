using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hercules : SoulCard
{
    protected override int CardID => Card.cardIdDict["헤라클레스"];
    [HideInInspector] public PlayerController playercontroller = null;
    public int multipleAD = 2;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void AddEffect()
    {
        if (playercontroller == GameBoard.instance.CurrentPlayerController())
        {
            ADmultiply();
        }
        
        playercontroller.OnMyTurnStart += ADmultiply;
        playercontroller.OnMyTurnEnd += ADoriginate;
        InfusedPiece.OnSoulRemoved += RemoveEffect;
    }

    public override void RemoveEffect()
    {
        if (playercontroller == GameBoard.instance.CurrentPlayerController())
        {
            ADoriginate();
        }

        playercontroller.OnMyTurnStart -= ADmultiply;
        playercontroller.OnMyTurnEnd -= ADoriginate;
        InfusedPiece.OnSoulRemoved -= RemoveEffect;
    }

    public void ADmultiply()
    {
        InfusedPiece.AD *= multipleAD;
    }

    public void ADoriginate()
    {
        InfusedPiece.AD /= multipleAD;
    }
}