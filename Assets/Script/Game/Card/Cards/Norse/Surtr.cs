using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Surtr : SoulCard
{
    protected override void Awake()
    {
        base.Awake();
        // �ڽ�Ʈ ����: ī�� ���� �ñ⿡ ���� �޶��� �ʿ� ����
        GameBoard.instance.myController.OnMyTurnEnd += DecreaseCost;

        OnInfuse += (chessPiece) => GameBoard.instance.myController.OnMyTurnEnd -= DecreaseCost;
        OnInfuse += DestroyAllCards;
        OnInfuse += (chessPiece) => GameBoard.instance.myController.OnMyTurnEnd += DestroyInfusedPiece;
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
