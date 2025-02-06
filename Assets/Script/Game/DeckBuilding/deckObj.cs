using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class deckObj : MonoBehaviour, IPointerClickHandler
{
    public int deckIndex;
    public TextMeshProUGUI deckNameText;
    public Button loadDeckButton;

    private void Awake()
    {
        loadDeckButton.onClick.AddListener(LoadDeck);
    }

    public void LoadDeck()
    {
        GetComponentInParent<DeckBuildingSceneUI>().LoadDeckButtonFunction(deckIndex);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            GameManager.instance.deckList[deckIndex].index = -1;
            GameManager.instance.SaveDeckData();

            Destroy(gameObject);
        }
    }
}
