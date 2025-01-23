using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyThinkPhilosopherEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        PlayerController playerController;
        PlayerController opponentController;
        
        if (player.playerColor == GameBoard.PlayerColor.White)
        {
            playerController = GameBoard.instance.whiteController;
            opponentController = GameBoard.instance.blackController;
        }
        else
        {
            playerController = GameBoard.instance.blackController;
            opponentController = GameBoard.instance.whiteController;
        }

        playerController.LocalDraw();
        opponentController.OpponentDraw();
    }
}
