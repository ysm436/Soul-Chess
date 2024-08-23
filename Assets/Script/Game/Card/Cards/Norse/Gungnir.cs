using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gungnir : SpellCard
{
    protected override int CardID => Card.cardIdDict["궁니르"];

    private PlayerController Player;
    [SerializeField] private GameObject gungnir_prefab;

    public void ReadyToGetGungnir(PlayerController player)
    {
        Player = player;

        Player.OnMyTurnStart += GetGungnir;
        gameObject.SetActive(false);
    }

    public void GetGungnir()
    {
        gameObject.SetActive(true);
        Player.OnMyTurnStart -= GetGungnir;
        
        if (Player.playerColor == GameBoard.PlayerColor.White)
        {
            if (!GameBoard.instance.gameData.playerWhite.TryAddCardInHand(this))
            {
                Debug.Log("궁니르 : Hand is Full");
                Destroy(this);
                return;
            }
        }
        else
        {
            if (!GameBoard.instance.gameData.playerBlack.TryAddCardInHand(this))
            {
                Debug.Log("궁니르 : Hand is Full");
                Destroy(this);
                return;
            }
        }
    }
}
