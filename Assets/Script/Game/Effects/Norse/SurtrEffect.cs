using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SurtrEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        Surtr surtrComponent = gameObject.GetComponent<Surtr>();
        GameBoard.instance.whiteController.OnMyTurnEnd -= surtrComponent.DecreaseCost;
        GameBoard.instance.blackController.OnMyTurnEnd -= surtrComponent.DecreaseCost;

        GameBoard.instance.gameData.playerBlack.RemoveDeckCards();
        GameBoard.instance.gameData.playerWhite.RemoveDeckCards();

        if (surtrComponent.InfusedPiece.pieceColor == GameBoard.PlayerColor.Black)
        {
            GameBoard.instance.gameData.playerWhite.RemoveHandCards();
            foreach (var card in GameBoard.instance.gameData.playerBlack.hand.ToList())
            {
                if (card != GetComponent<Surtr>())
                {
                    GameBoard.instance.gameData.playerBlack.hand.Remove(card);
                    card.Destroy();
                }
            }
        }
        else
        {
            GameBoard.instance.gameData.playerBlack.RemoveHandCards();
            GameBoard.instance.gameData.playerWhite.TryAddCardInHand(GetComponent<Card>());
            foreach (var card in GameBoard.instance.gameData.playerWhite.hand.ToList())
            {
                if (card != GetComponent<Surtr>())
                {
                    GameBoard.instance.gameData.playerBlack.hand.Remove(card);
                    card.Destroy();
                }
            }
        }
    }
}
