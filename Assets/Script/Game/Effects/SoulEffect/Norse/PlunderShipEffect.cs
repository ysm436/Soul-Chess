using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlunderShipEffect : Effect
{
    [SerializeField] private GameObject plunderer_card;
    public override void EffectAction(PlayerController player)
    {
        PlunderShip plundershipComponent = gameObject.GetComponent<PlunderShip>();
        PlayerData playercolor;

        if (plundershipComponent.InfusedPiece.pieceColor == GameBoard.PlayerColor.White)
            playercolor = GameBoard.instance.gameData.playerWhite;
        else
            playercolor = GameBoard.instance.gameData.playerBlack;

        for (int i = 0; i < plundershipComponent.cardAmount; i++)
        {
            GameObject plunderer = Instantiate(plunderer_card);
            Card plundererInfo = plunderer.GetComponent<Card>();
            plundererInfo.owner = GetComponent<Card>().owner;
            
            if (playercolor.playerColor != GameBoard.instance.myController.playerColor)
                plunderer.transform.localEulerAngles = new Vector3(0, 0, 180); //적이 카드를 사용할 경우 카드가 회전해서 상대의 손에 들어가도록 변경

            if (!playercolor.TryAddCardInHand(plundererInfo))
            {
                Destroy(plunderer);
            };
        }
    }
}