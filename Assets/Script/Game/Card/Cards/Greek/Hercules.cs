using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hercules : SoulCard
{
    protected override int CardID => Card.cardIdDict["헤라클레스"];
    public int multipleAD = 2;
    private int multiplyCount = 0;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void AddEffect()
    {   
        InfusedPiece.OnAttacked += ADmultiply;
        InfusedPiece.OnSoulRemoved += RemoveEffect;
    }

    public override void RemoveEffect()
    {
        ADoriginate();
        InfusedPiece.OnAttacked -= ADmultiply;
    }

    public void ADmultiply(ChessPiece chessPiece, int damage)
    {
        InfusedPiece.AD *= multipleAD;
        multiplyCount += 1;
    }

    public void ADoriginate()
    {
        for (int i = 0; i < multiplyCount; i++)
        {
            InfusedPiece.AD /= multipleAD;
        }
    }
}