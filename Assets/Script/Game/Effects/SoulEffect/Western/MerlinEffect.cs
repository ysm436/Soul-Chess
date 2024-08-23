using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MerlinEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        PlayerData playerData = null;
        if (player.playerColor == GameBoard.PlayerColor.White)
            playerData = GameBoard.instance.gameData.playerWhite;
        else
            playerData = GameBoard.instance.gameData.playerBlack;      

        List<Card> ourSpellCards = playerData.hand.Where(card => card.GetComponent<SpellCard>() != null).ToList();

        foreach (var card in ourSpellCards)
        {
            card.cost = 0;
        }
    }
}
