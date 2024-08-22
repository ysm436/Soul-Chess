using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyThinkPhilosopherEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        OnlyThinkPhilosopher onlythinkphilosopher_component = gameObject.GetComponent<OnlyThinkPhilosopher>();
        PlayerData playerdata;

        if (onlythinkphilosopher_component.InfusedPiece.pieceColor == GameBoard.PlayerColor.White)
            playerdata = GameBoard.instance.gameData.playerWhite;
        else
            playerdata = GameBoard.instance.gameData.playerBlack;

        playerdata.DrawCard();
    }
}
