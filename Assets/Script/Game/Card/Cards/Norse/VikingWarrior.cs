using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.Rendering;

public class VikingWarrior : SoulCard
{
    protected override int CardID => Card.cardIdDict["바이킹 전사"];
    private bool increasedFlag;
    public int increasedAD = 20;

    protected override void Awake()
    {
        base.Awake();
        increasedFlag = false;
    }

    public override void AddEffect()
    {
        if (increasedFlag)
        {
            InfusedPiece.AD += increasedAD;
            InfusedPiece.buff.AddBuffByValue("바이킹 전사", Buff.BuffType.AD, increasedAD, true);
        }
        InfusedPiece.OnAttackedAfter += IncreasingAD;
        
        InfusedPiece.OnSoulRemoved += RemoveEffect;
    }

    public override void RemoveEffect()
    {
        if (increasedFlag)
        {
            InfusedPiece.AD -= increasedAD;
            InfusedPiece.buff.TryRemoveSpecificBuff("바이킹 전사", Buff.BuffType.AD);
        }
        InfusedPiece.OnAttackedAfter -= IncreasingAD;

        InfusedPiece.OnSoulRemoved -= RemoveEffect;
    }

    private void IncreasingAD(ChessPiece chessPiece)
    {
        if (!increasedFlag && InfusedPiece.GetHP < InfusedPiece.maxHP)
        {
            increasedFlag = true;
            InfusedPiece.AD += 20;
            InfusedPiece.buff.AddBuffByValue("바이킹 전사", Buff.BuffType.AD, increasedAD, true);
        }
    }
}