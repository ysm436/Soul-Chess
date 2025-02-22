using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Surtr : SoulCard
{
    protected override int CardID => Card.cardIdDict["수르트"];

    private PlayerController playerController;

    [SerializeField] private int countdown = 3;
    [SerializeField] private int decreaseCost = 1;

    protected override void Awake()
    {
        base.Awake();

        GameBoard.instance.whiteController.OnMyTurnEnd += DecreaseCost;
        GameBoard.instance.blackController.OnMyTurnEnd += DecreaseCost;
    }

    private void OnDisable()
    {
        GameBoard.instance.whiteController.OnMyTurnEnd -= DecreaseCost;
        GameBoard.instance.blackController.OnMyTurnEnd -= DecreaseCost;
    }

    public void DecreaseCost()
    {
        if (cost > 0)
        {
            cost -= decreaseCost;
            
            if (cost < 0)
                cost = 0;
        }
    }

    private void KillCountDown()
    {
        countdown--;
        Debug.Log(countdown);

        if (countdown == 0)
        {
            InfusedPiece.GetComponent<Animator>().SetTrigger("killedTrigger");
            InfusedPiece.MakeAttackedEffect();
            InfusedPiece.Kill();
        }
    }


    public override void AddEffect()
    {
        if (InfusedPiece.pieceColor == GameBoard.PlayerColor.White)
            playerController = GameBoard.instance.whiteController;
        else
            playerController = GameBoard.instance.blackController;

        playerController.OnMyTurnStart += KillCountDown;
        InfusedPiece.buff.AddBuffByDescription(cardName, Buff.BuffType.Description, "수르트: 3턴 뒤, 이 기물은 파괴됩니다.", true);
        InfusedPiece.OnSoulRemoved += RemoveEffect;

    }
    public override void RemoveEffect()
    {
        if (InfusedPiece.pieceColor == GameBoard.PlayerColor.White)
            playerController = GameBoard.instance.whiteController;
        else
            playerController = GameBoard.instance.blackController;

        playerController.OnMyTurnStart -= KillCountDown;
        InfusedPiece.buff.TryRemoveSpecificBuff(cardName, Buff.BuffType.Description);
    }
}
