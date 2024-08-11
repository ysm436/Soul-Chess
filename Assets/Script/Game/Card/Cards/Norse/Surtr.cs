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

        // Instantiate�� ī�嵵 ���� �ʿ�
    }

    private void DestroyInfusedPiece()
    {
        InfusedPiece.Kill();
    }
}
