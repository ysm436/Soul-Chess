using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeusEffect : Effect
{
    [SerializeField] private GameObject thunder_card;
    public override void EffectAction(PlayerController player)
    {
        Zeus zeus_component = gameObject.GetComponent<Zeus>();
        PlayerData playercolor;

        if (zeus_component.InfusedPiece.pieceColor == GameBoard.PlayerColor.White)
            playercolor = GameBoard.instance.gameData.playerWhite;
        else
            playercolor = GameBoard.instance.gameData.playerBlack;

        for (int i = 0; i < 3; i++)
        {
            GameObject thunder = Instantiate(thunder_card);
            Card thunder_info = thunder.GetComponent<Card>();
            thunder_info.owner = GetComponent<Card>().owner;

            if (playercolor.playerColor != GameBoard.instance.myController.playerColor)
                thunder.transform.localEulerAngles = new Vector3(0, 0, 180); //적이 카드를 사용한 경우 카드가 회전해서 상대의 손에 들어가도록 변경

            if (!playercolor.TryAddCardInHand(thunder_info))
            {
                Destroy(thunder);
            };
        }
    }
}
