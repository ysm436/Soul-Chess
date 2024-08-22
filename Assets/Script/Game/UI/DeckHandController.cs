using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

public class DeckHandController : MonoBehaviour
{
    PhotonView photonView;
    readonly float CARD_DISTANCE_IN_HAND = 0.5f;


    public GameObject CardBoard;
    public GameBoard gameBoard;

    public Transform myDeckTransform;
    public Transform myHandTransform;
    public Transform opponentDeckTransform;
    public Transform opponentHandTransform;

    PlayerData player;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        GameBoard.instance.gameData.myPlayerData.UpdateHandPosition += UpdateMyHandPosition;

        player = GameBoard.instance.gameData.myPlayerData;

        Card instantiatedCard;

        var deckAnchor = myDeckTransform;

        var handAnchor = myHandTransform;

        foreach (Card card in GameManager.instance.GetCardListFrom(GameManager.instance.selectedDeck.cards))
        {
            card.owner = GameBoard.instance.gameData.myPlayerData;
            instantiatedCard = Instantiate(card, deckAnchor);
            instantiatedCard.FlipBack();
            player.deck.Add(instantiatedCard);
        }
        foreach (Card card in gameBoard.gameData.myPlayerData.deck)
        {
            AddCardInMyDeckObject(card);
        }

        player.Initialize();

        photonView.RPC("InitializeRemote", RpcTarget.Others, Card.GetCardIDArray(player.deck), Card.GetCardIDArray(player.hand));
    }

    [PunRPC]
    public void InitializeRemote(int[] deckData, int[] handData)
    {
        GameBoard.instance.gameData.opponentPlayerData.UpdateHandPosition += UpdateOpponentHandPosition;

        Card instantiatedCard;

        var deckAnchor = opponentDeckTransform;

        var handAnchor = opponentHandTransform;

        foreach (Card card in GameManager.instance.GetCardListFrom(deckData.ToList<int>()))
        {
            card.owner = GameBoard.instance.gameData.opponentPlayerData;
            instantiatedCard = Instantiate(card, deckAnchor);
            instantiatedCard.FlipBack();
            GameBoard.instance.gameData.opponentPlayerData.deck.Add(instantiatedCard);
        }

        foreach (Card card in GameManager.instance.GetCardListFrom(handData.ToList<int>()))
        {
            card.owner = GameBoard.instance.gameData.opponentPlayerData;
            instantiatedCard = Instantiate(card, handAnchor);
            GameBoard.instance.gameData.opponentPlayerData.TryAddCardInHand(instantiatedCard);
        }

        foreach (Card card in gameBoard.gameData.opponentPlayerData.deck)
        {
            AddCardInOpponentDeckObject(card);
        }
    }

    public void AddCardInMyDeckObject(Card card)
    {
        card.FlipBack();
        card.transform.position = myDeckTransform.position;
    }
    public void AddCardInOpponentDeckObject(Card card)
    {
        card.FlipBack();
        card.transform.position = opponentDeckTransform.position;
    }

    public void UpdateMyHandPosition()
    {
        List<Card> hand = GameBoard.instance.gameData.myPlayerData.hand;
        float anchor_x;

        if (hand.Count == 0)
            anchor_x = 0;
        else if (hand.Count % 2 == 0)
            anchor_x = -(hand.Count / 2f - 0.5f) * CARD_DISTANCE_IN_HAND;
        else
            anchor_x = -(hand.Count / 2f) * CARD_DISTANCE_IN_HAND;


        for (int i = 0; i < hand.Count; i++)
        {
            hand[i].handIndex = i;
            hand[i].transform.SetParent(myHandTransform);
            hand[i].transform.localPosition = new Vector3(anchor_x + CARD_DISTANCE_IN_HAND * i, 0, -0.1f * i); //UI에 맞게 좌표수정
        }
    }

    public void UpdateOpponentHandPosition()
    {
        List<Card> hand = GameBoard.instance.gameData.opponentPlayerData.hand;
        float anchor_x;

        if (hand.Count == 0)
            anchor_x = 0;
        else if (hand.Count % 2 == 0)
            anchor_x = (hand.Count / 2f - 0.5f) * CARD_DISTANCE_IN_HAND;
        else
            anchor_x = (hand.Count / 2f) * CARD_DISTANCE_IN_HAND;


        for (int i = 0; i < hand.Count; i++)
        {
            hand[i].handIndex = i;
            hand[i].transform.SetParent(opponentHandTransform);
            hand[i].transform.localPosition = new Vector3(anchor_x - CARD_DISTANCE_IN_HAND * i, 0, -0.1f * i); //UI에 맞게 좌표수정
        }
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
