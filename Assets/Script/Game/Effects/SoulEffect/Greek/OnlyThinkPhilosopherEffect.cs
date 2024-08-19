using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyThinkPhilosopherEffect : Effect
{
    public override void EffectAction()
    {
        PlayerData playercolor;
        if (GameBoard.instance.myController.playerColor == GameBoard.PlayerColor.White)
            playercolor = GameBoard.instance.gameData.playerWhite;
        else
            playercolor = GameBoard.instance.gameData.playerBlack;

        playercolor.DrawCard();
    }
}
