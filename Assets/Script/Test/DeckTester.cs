using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeckTester : MonoBehaviour
{
    public GameObject CardBoard;
    public GameBoard gameBoard;

    PlayerData player;

    public List<Card> hand;
    public List<Card> deck;

    private void Start()
    {
        Card instantiatedCard;

        player = gameBoard.gameData.playerWhite;
        player.hand = new();
        player.deck = new();

        var deckAnchor = new GameObject("Deck").transform;

        var handAnchor = new GameObject("Hand").transform;

        foreach (Card card in hand)
        {
            instantiatedCard = Instantiate(card, handAnchor);
            player.TryAddCardInHand(instantiatedCard);
        }

        foreach (Card card in deck)
        {
            instantiatedCard = Instantiate(card, deckAnchor);
            instantiatedCard.FlipBack();
            player.deck.Add(instantiatedCard);
        }
        player.OnGetCard += (card) => card.transform.SetParent(handAnchor);

        player.Initialize();

        //player.Mulligan();
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
