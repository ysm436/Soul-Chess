using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Surtr : SoulCard
{
    protected override void Awake()
    {
        base.Awake();
        // 코스트 감소: 카드 생성 시기에 따라 달라질 필요 있음
        GameManager.instance.whiteController.OnMyTurnEnd += DecreaseCost;

        OnInfuse += (chessPiece) => GameManager.instance.whiteController.OnMyTurnEnd -= DecreaseCost;
        OnInfuse += DestroyAllCards;
        OnInfuse += (chessPiece) => GameManager.instance.whiteController.OnMyTurnEnd += DestroyInfusedPiece;
    }

    private void DecreaseCost()
    {
        cost--;
    }

    private void DestroyAllCards(ChessPiece chessPiece)
    {
        GameManager.instance.gameData.playerWhite.hand.Clear();
        GameManager.instance.gameData.playerWhite.deck.Clear();
        GameManager.instance.gameData.playerBlack.hand.Clear();
        GameManager.instance.gameData.playerBlack.deck.Clear();

        // Instantiate된 카드도 삭제 필요
    }

    private void DestroyInfusedPiece()
    {
        InfusedPiece.Kill();
    }
}
