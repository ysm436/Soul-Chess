using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Fenrir : SoulCard
{
    private int IncreaseAmountAD = 20;
    private int IncreaseAmountHP = 20;

    private int buffedAD;
    private int buffedHP;

    protected override void Awake()
    {
        base.Awake();

        buffedAD = 0;
        buffedHP = 0;

        OnInfuse += (ChessPiece chessPiece) => chessPiece.OnKill += IncreaseStat;
        OnInfuse += (ChessPiece chessPiece) => chessPiece.OnSoulRemoved += RemoveEffect;
    }

    private void IncreaseStat(ChessPiece chessPiece)
    {
        InfusedPiece.AD += IncreaseAmountAD;
        InfusedPiece.maxHP += IncreaseAmountHP;


        //chessPiece.buff.AddBuffByValue(this.cardName, Buff.BuffType.AD, IncreaseAmountAD, true);
        //chessPiece.buff.AddBuffByValue(this.cardName, Buff.BuffType.HP, IncreaseAmountHP, true);

        buffedAD += IncreaseAmountAD;
        buffedHP += IncreaseAmountHP;
    }

    public override void AddEffect()
    {
        InfusedPiece.maxHP += buffedHP;
        InfusedPiece.AD += buffedAD;

        InfusedPiece.OnKill += IncreaseStat;
    }

    public override void RemoveEffect()
    {
        InfusedPiece.maxHP -= buffedHP;
        InfusedPiece.AD -= buffedAD;

        InfusedPiece.OnKill -= IncreaseStat;
    }
}
