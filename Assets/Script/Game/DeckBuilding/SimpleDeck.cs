using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SimpleDeck : MonoBehaviour, IPointerClickHandler
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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            GameManager.instance.deckList[deck_index].index = -1;
            GameManager.instance.SaveDeckData();

            Destroy(gameObject);
        }
    }
}
