using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Merlin : SoulCard
{
    protected override int CardID => Card.cardIdDict["멀린"];

    [HideInInspector] public PlayerController player = null;
    protected override void Awake()
    {
        base.Awake();
    }

    public override void AddEffect()
    {
        foreach (var piece in GameBoard.instance.gameData.pieceObjects)
        {
            piece.OnSpellAttacked += MultiplyDmg;
        }
        InfusedPiece.OnSoulRemoved += RemoveEffect;
    }

    public void MultiplyDmg(ChessPiece piece)
    {/*
        if (player == GameBoard.instance.CurrentPlayerController())
            piece.spellDamageCoefficient = 2;
    */
    }

    public override void RemoveEffect()
    {
        foreach (var piece in GameBoard.instance.gameData.pieceObjects)
        {
            piece.OnSpellAttacked -= MultiplyDmg;
        }
    }
}
