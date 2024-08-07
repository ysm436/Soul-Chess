using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SimpleDeck : MonoBehaviour
{
    public int deck_index;
    public TextMeshProUGUI DeckNameText;
    public Button LoadDeckButton;

    private void Awake()
    {
        LoadDeckButton.onClick.AddListener(LoadDeck);
    }

    public void LoadDeck()
    {
        GetComponentInParent<DeckBuildingSceneUI>().LoadDeck(deck_index);
    }
}
