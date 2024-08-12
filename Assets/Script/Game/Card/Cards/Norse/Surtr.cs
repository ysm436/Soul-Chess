using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Surtr : SoulCard
{
    protected override void Awake()
    {
        base.Awake();

        GameBoard.instance.myController.OnMyTurnEnd += DecreaseCost;
        GameBoard.instance.opponentController.OnMyTurnEnd += DecreaseCost;

        OnInfuse += (ChessPiece chessPiece) => GameBoard.instance.myController.OnMyTurnEnd -= DecreaseCost;
        OnInfuse += (ChessPiece chessPiece) => GameBoard.instance.opponentController.OnMyTurnEnd -= DecreaseCost;

        OnInfuse += DestroyAllCards;
        OnInfuse += (ChessPiece chessPiece) => GameBoard.instance.myController.OnMyTurnEnd += DestroyInfusedPiece;

        OnInfuse += (ChessPiece chessPiece) => chessPiece.OnSoulRemoved += RemoveEffect;
    }

    private void OnDisable()
    {
        GameBoard.instance.myController.OnMyTurnEnd -= DecreaseCost;
        GameBoard.instance.opponentController.OnMyTurnEnd -= DecreaseCost;
    }

    private void DecreaseCost()
    {
        if (cost > 0)
        {
            cost--;
        }
    }

    private void DestroyAllCards(ChessPiece chessPiece)
    {
        GameBoard.instance.gameData.playerWhite.RemoveHandCards();
        GameBoard.instance.gameData.playerWhite.RemoveDeckCards();
        GameBoard.instance.gameData.playerBlack.RemoveHandCards();
        GameBoard.instance.gameData.playerBlack.RemoveDeckCards();
    }

    private void DestroyInfusedPiece()
    {
        InfusedPiece.Kill();
    }

    private void RemoveEffect()
    {
        GameBoard.instance.myController.OnMyTurnEnd -= DestroyInfusedPiece;
    }
}
