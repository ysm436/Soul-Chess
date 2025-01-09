using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurtrEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        Surtr surtrComponent = gameObject.GetComponent<Surtr>();
        GameBoard.instance.whiteController.OnMyTurnEnd -= surtrComponent.DecreaseCost;
        GameBoard.instance.blackController.OnMyTurnEnd -= surtrComponent.DecreaseCost;

        GameBoard.instance.gameData.playerWhite.RemoveHandCards();
        GameBoard.instance.gameData.playerWhite.RemoveDeckCards();
        GameBoard.instance.gameData.playerBlack.RemoveHandCards();
        GameBoard.instance.gameData.playerBlack.RemoveDeckCards();

        if (surtrComponent.InfusedPiece.pieceColor == GameBoard.PlayerColor.Black)
        {
            GameBoard.instance.gameData.playerBlack.TryAddCardInHand(GetComponent<Card>());
        }
        else
        {
            GameBoard.instance.gameData.playerWhite.TryAddCardInHand(GetComponent<Card>());
        }
    }
}
