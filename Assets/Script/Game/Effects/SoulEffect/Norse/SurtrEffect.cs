using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurtrEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        Surtr surtr_component = gameObject.GetComponent<Surtr>();
        GameBoard.instance.whiteController.OnMyTurnEnd -= surtr_component.DecreaseCost;
        GameBoard.instance.blackController.OnMyTurnEnd -= surtr_component.DecreaseCost;

        GameBoard.instance.gameData.playerWhite.RemoveHandCards();
        GameBoard.instance.gameData.playerWhite.RemoveDeckCards();
        GameBoard.instance.gameData.playerBlack.RemoveHandCards();
        GameBoard.instance.gameData.playerBlack.RemoveDeckCards();

        if (surtr_component.InfusedPiece.pieceColor == GameBoard.PlayerColor.Black)
        {
            GameBoard.instance.gameData.playerBlack.TryAddCardInHand(GetComponent<Card>());
        }
        else
        {
            GameBoard.instance.gameData.playerWhite.TryAddCardInHand(GetComponent<Card>());
        }
    }
}
