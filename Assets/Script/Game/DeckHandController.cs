using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeckHandController : MonoBehaviour
{
    public GameObject CardBoard;
    public GameBoard gameBoard;

    public Transform deckTransform;
    public Transform handTransform;

    PlayerData player;

    private void Start()
    {
        player = GameBoard.instance.gameData.myPlayerData;

        Card instantiatedCard;

        var deckAnchor = deckTransform;

        var handAnchor = handTransform;

        //        foreach (Card card in GameBoard.instance.gameData.myPlayerData.hand)
        //        {
        //            instantiatedCard = Instantiate(card, handAnchor);
        //            player.TryAddCardInHand(instantiatedCard);
        //        }

        foreach (Card card in GameManager.instance.GetCardListFrom(GameManager.instance.selectedDeck))
        {
            instantiatedCard = Instantiate(card, deckAnchor);
            instantiatedCard.FlipBack();
            player.deck.Add(instantiatedCard);
        }

        gameBoard.myController.OnOpponentTurnEnd += () => player.DrawCard();

        player.OnGetCard += (card) => card.transform.SetParent(handAnchor);

        player.Initialize();
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
            bool result = player.TryAddCardInHand(Instantiate(GameBoard.instance.gameData.myPlayerData.deck[0]));
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            player.RemoveDeckCards();
        }
    }
}
