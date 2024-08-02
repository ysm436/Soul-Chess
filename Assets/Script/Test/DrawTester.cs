using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawTester : MonoBehaviour
{
    public GameObject CardBoard;
    public GameManager gameBoard;

    PlayerData player;

    public List<Card> deck;

    private Transform handAnchor;

    private void Start()
    {
        player = gameBoard.gameData.playerWhite;

        var deckAnchor = new GameObject("Deck").transform;

        handAnchor = new GameObject("Hand").transform;

        player.deck = new();
        player.hand = new();

        foreach (Card card in deck)
        {
            for (int i = 0; i < 10; i++)
            {

                var g = Instantiate(card, deckAnchor);
                g.FlipBack();
                g.transform.localPosition = new Vector2(-8f, 4f);
                player.deck.Add(g);
            }
        }

        gameBoard.whiteController.OnOpponentTurnEnd += () => player.DrawCard();

        player.OnGetCard += (card) => card.transform.SetParent(handAnchor);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            player.RemoveHandCards();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("Try to Remove First Card in Hand");
            bool result = player.TryRemoveCardInHand(player.hand[0]);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            bool result = player.TryAddCardInHand(Instantiate(deck[0]));
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            player.RemoveDeckCards();
        }
    }

}
