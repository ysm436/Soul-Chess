using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlunderShipEffect : Effect
{
    [SerializeField] private GameObject plunderer_card;
    public override void EffectAction()
    {
        PlayerData playercolor;
        if (GameBoard.instance.myController.playerColor == GameBoard.PlayerColor.White)
            playercolor = GameBoard.instance.gameData.playerWhite;
        else
            playercolor = GameBoard.instance.gameData.playerBlack;

        for (int i = 0; i < 3; i++)
        {
            GameObject plunderer = Instantiate(plunderer_card);
            Card plunderer_info = plunderer.GetComponent<Card>();
            playercolor.TryAddCardInHand(plunderer_info);
        }
    }
}