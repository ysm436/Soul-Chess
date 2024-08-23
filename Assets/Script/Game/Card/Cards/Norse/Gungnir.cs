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

        /* Player.OnMyTurnStart += GetGungnir;
        gameObject.SetActive(false); */
    }

    public void GetGungnir()
    {
        /* gameObject.SetActive(true);
        Debug.Log("GetGungnir");
        Player.OnMyTurnStart -= GetGungnir; */

        /* GameObject gungnir_card = Instantiate(gungnir_prefab);
        Card gungnir_cardcomponent = gungnir_card.GetComponent<Card>();
        gungnir_cardcomponent.isInSelection = false;
        gungnir_cardcomponent.owner = owner; */

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

    /* public void GetGungnir()
    { */
        /* gameObject.SetActive(true); */
        /* if (playercontroller != null) playercontroller.OnMyTurnStart -= GetGungnir;
        GameObject gungnir_card = Instantiate(gungnir_prefab);
        Card gungnir_cardcomponent = gungnir_card.GetComponent<Card>();
        gungnir_cardcomponent.owner = owner;

        if (playercontroller.playerColor == GameBoard.PlayerColor.White)
        {
            if (!GameBoard.instance.gameData.playerWhite.TryAddCardInHand(gungnir_cardcomponent))
            {
                Debug.Log("궁니르 : Hand is Full");
                Destroy();
                return;
            }
        }
        else
        {
            if (!GameBoard.instance.gameData.playerBlack.TryAddCardInHand(gungnir_cardcomponent))
            {
                Debug.Log("궁니르 : Hand is Full");
                playercontroller.OnMyTurnStart -= GetGungnir;
                Destroy();
                return;
            }
        }
    } */
}
