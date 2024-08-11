using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeckCountTextUI : MonoBehaviour
{
    [SerializeField] private TextMeshPro deckCountText;

    private void Awake()
    {
        deckCountText.gameObject.SetActive(false);
    }

    private void OnMouseEnter()
    {
        deckCountText.gameObject.SetActive(true);
        deckCountText.text = GameBoard.instance.gameData.playerWhite.deck.Count.ToString() + " cards left";
    }

    private void OnMouseExit()
    {
        deckCountText.gameObject.SetActive(false);
    }
}
