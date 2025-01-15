using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO 이벤트 시스템 수정 후 재수정

public class Abel : SoulCard
{
    protected override int CardID => Card.cardIdDict["아벨"];
    private ChessPiece recent_attacked;
    private bool attack_signal;

    protected override void Awake()
    {
        base.Awake();
    }

    private void OnAttackedByChessPiece(ChessPiece chessPiece, int damage)
    {
        recent_attacked = chessPiece;
    }
    private void OnAttackedBySpell(ChessPiece attackedPiece)
    {
        recent_attacked = null;
    }
    private void AbelStartAttack(ChessPiece chessPiece)
    {
        attack_signal = true;
    }
    private void AbelEndAttack(ChessPiece chessPiece)
    {
        attack_signal = false;
    }

    private void OnKilledEffect(ChessPiece chessPiece)
    {
        //아벨이 공격을 해서 피해를 받는 경우 제외 및 스펠카드로 죽지 않았을 때 활성화
        if (!attack_signal && recent_attacked != null)
        {
            recent_attacked.AffectByAbel = true;
            recent_attacked.GetComponent<Animator>().SetTrigger("killedTrigger");
            recent_attacked.MakeAttackedEffect();
            recent_attacked.Kill();
        }
    }


    public override void AddEffect()
    {
        InfusedPiece.OnStartAttack += AbelStartAttack;
        InfusedPiece.OnEndAttack += AbelEndAttack;
        InfusedPiece.OnAttacked += OnAttackedByChessPiece;
        InfusedPiece.OnSpellAttacked += OnAttackedBySpell;
        InfusedPiece.OnKilled += OnKilledEffect;
    }

    public override void RemoveEffect()
    {
        InfusedPiece.OnKilled -= OnKilledEffect;
    }
}
