using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonQuixote : SoulCard
{
    protected override int CardID => Card.cardIdDict["늙은 방랑기사"];

    public int standardAD;
    public int extraAD;
    private bool extraAttack = false;
    protected override void Awake()
    {
        base.Awake();
    }

    public void StartAttackEffect(ChessPiece chessPiece)
    {
        if (chessPiece.AD >= standardAD)
        {
            this.InfusedPiece.AD += extraAD;
            extraAttack = true;
        }
    }

    public void EndAttackEffect(ChessPiece chessPiece)
    {
        if (extraAttack)
        {
            this.InfusedPiece.AD -= extraAD;
            extraAttack = false;
        }
    }

    public override void AddEffect()
    {
        InfusedPiece.OnStartAttack += StartAttackEffect;
        InfusedPiece.OnEndAttack += EndAttackEffect;
        InfusedPiece.buff.AddBuffByDescription(cardName, Buff.BuffType.Description, "돈키호테: 공격력 " + standardAD + " 이상 기물 공격 시 " + extraAD + " 추가 피해", true);
    }

    public override void RemoveEffect()
    {
        InfusedPiece.OnStartAttack -= StartAttackEffect;
        InfusedPiece.OnEndAttack -= EndAttackEffect;
        InfusedPiece.buff.TryRemoveSpecificBuff(cardName, Buff.BuffType.Description);
    }
}
