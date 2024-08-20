using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gungnir : SpellCard
{
    protected override int CardID => Card.cardIdDict["궁니르"];

    public void ReadyToGetGungnir()
    {
        GameBoard.instance.myController.OnMyTurnStart += GetGungnir;
        gameObject.SetActive(false);
    }

    public void GetGungnir()
    {
        if (GameBoard.instance.myController.playerColor == GameBoard.PlayerColor.White)
        {
            gameObject.SetActive(true);
            GameBoard.instance.myController.OnMyTurnStart -= GetGungnir;
            if (!GameBoard.instance.gameData.playerWhite.TryAddCardInHand(this))
            {
                Debug.Log("궁니르 : Hand is Full");
                Destroy();
                return;
            }
        }
        else
        {
            gameObject.SetActive(true);
            GameBoard.instance.myController.OnMyTurnStart -= GetGungnir;
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
