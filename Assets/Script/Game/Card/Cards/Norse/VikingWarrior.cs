using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.Rendering;

public class VikingWarrior : SoulCard
{
    protected override int CardID => Card.cardIdDict["바이킹 전사"];
    private bool increased;

    protected override void Awake()
    {
        base.Awake();
        increased = false;
    }

    public override void AddEffect()
    {
        if (increased)
        {
            InfusedPiece.AD += 20;
            InfusedPiece.buff.AddBuffByValue("바이킹 전사", Buff.BuffType.AD, 20, true);
        }
        InfusedPiece.OnAttackedAfter += increaseAD;
        
        InfusedPiece.OnSoulRemoved += RemoveEffect;
    }

    public override void RemoveEffect()
    {
        if (increased)
        {
            InfusedPiece.AD -= 20;
            InfusedPiece.buff.TryRemoveSpecificBuff("바이킹 전사", Buff.BuffType.AD);
        }
        InfusedPiece.OnAttackedAfter -= increaseAD;

        InfusedPiece.OnSoulRemoved -= RemoveEffect;
    }

    private void increaseAD(ChessPiece chessPiece)
    {
        if (!increased && InfusedPiece.GetHP < InfusedPiece.maxHP)
        {
            increased = true;
            InfusedPiece.AD += 20;
            InfusedPiece.buff.AddBuffByValue("바이킹 전사", Buff.BuffType.AD, 20, true);
        }
    }
}