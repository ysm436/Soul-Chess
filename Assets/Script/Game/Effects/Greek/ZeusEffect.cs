using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeusEffect : Effect
{
    [SerializeField] private GameObject thunderCard;
    public override void EffectAction(PlayerController player)
    {
        Zeus zeusComponent = gameObject.GetComponent<Zeus>();
        PlayerData playercolor;

        if (zeusComponent.InfusedPiece.pieceColor == GameBoard.PlayerColor.White)
            playercolor = GameBoard.instance.gameData.playerWhite;
        else
            playercolor = GameBoard.instance.gameData.playerBlack;

        for (int i = 0; i < zeusComponent.cardAmount; i++)
        {
            GameObject thunder = Instantiate(thunderCard);
            Card thunderInfo = thunder.GetComponent<Card>();
            thunderInfo.owner = GetComponent<Card>().owner;

            if (playercolor.playerColor != GameBoard.instance.myController.playerColor)
                thunder.transform.localEulerAngles = new Vector3(0, 0, 180); //적이 카드를 사용한 경우 카드가 회전해서 상대의 손에 들어가도록 변경

            if (!playercolor.TryAddCardInHand(thunderInfo))
            {
                Destroy(thunder);
            };
        }
    }
}
