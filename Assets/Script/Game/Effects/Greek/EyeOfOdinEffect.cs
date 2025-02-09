using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeOfOdinEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        PlayerData playerData = null;
        PlayerData oppenent = null;

        if (player.playerColor == GameBoard.PlayerColor.White)
        { 
            playerData = GameBoard.instance.gameData.playerWhite; oppenent = GameBoard.instance.gameData.playerBlack;
        }
        else 
        { 
            playerData = GameBoard.instance.gameData.playerBlack; oppenent = GameBoard.instance.gameData.playerWhite;
        }

        playerData.isRevealed = true;
        oppenent.UpdateHandPosition();
    }
}
