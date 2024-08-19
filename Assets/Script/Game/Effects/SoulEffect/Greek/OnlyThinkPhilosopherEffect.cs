using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyThinkPhilosopherEffect : Effect
{
    public override void EffectAction()
    {
        OnlyThinkPhilosopher onlythinkphilosopher_component = gameObject.GetComponent<OnlyThinkPhilosopher>();
        PlayerData playercolor;
        
        if (onlythinkphilosopher_component.InfusedPiece.pieceColor == GameBoard.PlayerColor.White)
            playercolor = GameBoard.instance.gameData.playerWhite;
        else
            playercolor = GameBoard.instance.gameData.playerBlack;

        playercolor.DrawCard();
    }
}
