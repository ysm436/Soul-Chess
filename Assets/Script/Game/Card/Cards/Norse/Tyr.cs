using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tyr : SoulCard
{
    protected override int CardID => Card.cardIdDict["티르"];
    [HideInInspector] public ChessPiece restraint_target = null;

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
        if (restraint_target != null) restraint_target.Unrestraint();
    }
}