using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tyr : SoulCard
{
    protected override int CardID => Card.cardIdDict["티르"];
    ChessPiece restraint_target;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void AddEffect()
    {
        InfusedPiece.OnKilled += Restraint_remove;
        InfusedPiece.OnSoulRemoved += RemoveEffect;
    }

    public override void RemoveEffect()
    {
        InfusedPiece.OnKilled -= Restraint_remove;
        InfusedPiece.OnSoulRemoved -= RemoveEffect;
    }

    public void Restraint_remove(ChessPiece chessPiece)
    {
        restraint_target.Unrestraint();
    }
}