using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeckInfo : MonoBehaviour
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
