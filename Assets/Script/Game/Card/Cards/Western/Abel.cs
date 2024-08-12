using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO 이벤트 시스템 수정 후 재수정

public class Abel : SoulCard
{
    ChessPiece recent;

    protected override void Awake()
    {
        base.Awake();
        OnInfuse += InfuseEffect;
    }

    public void InfuseEffect(ChessPiece chessPiece)
    {
        chessPiece.OnAttacked += OnAttackedEffect;
        chessPiece.OnKilled += OnKilledEffect;

        chessPiece.OnSoulRemoved += RemoveEffect;
    }

    public void OnAttackedEffect(ChessPiece chessPiece, int damamge)
    {
        recent = chessPiece;
    }

    public void OnKilledEffect(ChessPiece chessPiece)
    {
        if (!GameBoard.instance.myController.isMyTurn)
        {
            recent.Kill();
        }
    }

    private void RemoveEffect()
    {
        InfusedPiece.OnAttacked -= OnAttackedEffect;
        InfusedPiece.OnKilled -= OnKilledEffect;
    }
}
