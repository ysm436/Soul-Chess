using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraupnirEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        if (player.playerColor == GameBoard.PlayerColor.White)
            GameBoard.instance.gameData.playerWhite.soulEssence = GameBoard.instance.gameData.playerWhite.soulOrbs;
        else
            GameBoard.instance.gameData.playerBlack.soulEssence = GameBoard.instance.gameData.playerBlack.soulOrbs;
    }
}
