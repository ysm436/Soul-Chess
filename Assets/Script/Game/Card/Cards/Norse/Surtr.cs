using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Surtr : SoulCard
{
    protected override int CardID => Card.cardIdDict["수르트"];

    protected override void Awake()
    {
        base.Awake();
        // �ڽ�Ʈ ����: ī�� ���� �ñ⿡ ���� �޶��� �ʿ� ����
        GameBoard.instance.whiteController.OnMyTurnEnd += DecreaseCost;

        OnInfuse += (chessPiece) => GameBoard.instance.whiteController.OnMyTurnEnd -= DecreaseCost;
        OnInfuse += DestroyAllCards;
        OnInfuse += (chessPiece) => GameBoard.instance.whiteController.OnMyTurnEnd += DestroyInfusedPiece;
    }

    private void DecreaseCost()
    {
        cost--;
    }

    private void DestroyAllCards(ChessPiece chessPiece)
    {
        GameBoard.instance.gameData.playerWhite.hand.Clear();
        GameBoard.instance.gameData.playerWhite.deck.Clear();
        GameBoard.instance.gameData.playerBlack.hand.Clear();
        GameBoard.instance.gameData.playerBlack.deck.Clear();

        // Instantiate�� ī�嵵 ���� �ʿ�
    }

    private void DestroyInfusedPiece()
    {
        InfusedPiece.Kill();
    }
}
