using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO 이벤트 시스템 수정 후 재수정

public class Abel : SoulCard
{
    ChessPiece recent;
    private bool myturn = true;

    protected override void Awake()
    {
        base.Awake();
        gameObject.GetComponent<SoulCard>().OnInfuse += InfuseEffect;
    }

    public void InfuseEffect(ChessPiece chessPiece)
    {
        chessPiece.OnAttacked += OnAttackedEffect;
        chessPiece.OnKilled += OnkilledEffect;
        GameManager.instance.whiteController.OnMyDraw += MyTurnSignal;
        GameManager.instance.whiteController.OnMyTurnEnd += NotMyTurnSignal;
    }

    public void MyTurnSignal()
    {
        myturn = true;
    }

    public void NotMyTurnSignal()
    {
        myturn = false;
    }

    public void OnAttackedEffect(ChessPiece chessPiece, int damamge)
    {
        recent = chessPiece;
    }

    public void OnkilledEffect(ChessPiece chessPiece)
    {
        if (myturn)
        {
            recent.Kill();
        }
    }

}