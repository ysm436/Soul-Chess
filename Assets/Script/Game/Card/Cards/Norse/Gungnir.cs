using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gungnir : SpellCard
{
    protected override int CardID => Card.cardIdDict["궁니르"];

    [HideInInspector] public PlayerController player = null;

    public void ReadyToGetGungnir()
    {
        if (player != null) player.OnMyTurnStart += GetGungnir;
        gameObject.SetActive(false);
    }

    public void GetGungnir()
    {
        gameObject.SetActive(true);
        if (player != null) player.OnMyTurnStart -= GetGungnir;
        
        if (player.playerColor == GameBoard.PlayerColor.White)
        {
            if (!GameBoard.instance.gameData.playerWhite.TryAddCardInHand(this))
            {
                Debug.Log("궁니르 : Hand is Full");
                Destroy();
                return;
            }
        }
        else
        {
            if (!GameBoard.instance.gameData.playerBlack.TryAddCardInHand(this))
            {
                Debug.Log("궁니르 : Hand is Full");
                GameBoard.instance.myController.OnMyTurnStart -= GetGungnir;
                Destroy();
                return;
            }
        }
    }
}
