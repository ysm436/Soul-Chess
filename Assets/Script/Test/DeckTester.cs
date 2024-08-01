using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeckTester : MonoBehaviour
{
    public GameObject CardBoard;
    public GameManager gameBoard;

    PlayerData player;

    public List<Card> deck;
    public List<Card> hand;

    private void Start()
    {
        GameObject g;

        player = gameBoard.gameData.playerWhite;
        player.deck = deck;
        player.OnGetCard += UpdateHand;

        foreach (Card card in hand)
        {
            g = Instantiate(card.gameObject);
            g.GetComponent<Card>().FlipFront();
            player.GetCard(g.GetComponent<Card>());
        }
    }


    public void UpdateHand()
    {
        for (int i = 0; i < player.hand.Count; i++)
        {
            player.hand[i].transform.position = new Vector3(0.5f * i - 8, -4, -0.1f * i);
        }
    }
}
