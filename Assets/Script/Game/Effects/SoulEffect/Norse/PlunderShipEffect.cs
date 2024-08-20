using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlunderShipEffect : Effect
{
    [SerializeField] private GameObject plunderer_card;
    public override void EffectAction(PlayerController player)
    {
        PlunderShip plundership_component = gameObject.GetComponent<PlunderShip>();
        PlayerData playercolor;

        if (plundership_component.InfusedPiece.pieceColor == GameBoard.PlayerColor.White)
            playercolor = GameBoard.instance.gameData.playerWhite;
        else
            playercolor = GameBoard.instance.gameData.playerBlack;

        for (int i = 0; i < 3; i++)
        {
            GameObject plunderer = Instantiate(plunderer_card);
            Card plunderer_info = plunderer.GetComponent<Card>();
            plunderer_info.isMine = true;
            if (!playercolor.TryAddCardInHand(plunderer_info))
            {
                Destroy(plunderer);
            };
        }
    }
}