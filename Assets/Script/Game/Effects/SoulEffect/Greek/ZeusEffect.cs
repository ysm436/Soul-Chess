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
            thunder_info.isMine = true;
            if (!playercolor.TryAddCardInHand(thunder_info))
            {
                Destroy(thunder);
            };
        }
    }
}
