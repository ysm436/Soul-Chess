using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ShowDeckInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject deckInfoPanel;
    [SerializeField] private Transform costGraph;
    [SerializeField] private TextMeshProUGUI soulCost;
    [SerializeField] private TextMeshProUGUI spellCost;
    [SerializeField] private TextMeshProUGUI commonCost;
    [SerializeField] private TextMeshProUGUI legendaryCost;
    [SerializeField] private TextMeshProUGUI mythicalCost;

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        DeckManager deckManager = GetComponentInParent<DeckManager>();

        for (int i = 0; i < costGraph.childCount; i++)
        {
            RectTransform rectTransform = costGraph.GetChild(i).GetComponent<RectTransform>();
            Vector2 graphSize = rectTransform.sizeDelta;
            graphSize.y = 10 * deckManager.loadedDeckCosts[i];
            rectTransform.sizeDelta = graphSize;
        }

        soulCost.text = deckManager.loadedDeckTypes[0].ToString();
        spellCost.text = deckManager.loadedDeckTypes[1].ToString();

        commonCost.text = deckManager.loadedDeckRarities[0].ToString();
        legendaryCost.text = deckManager.loadedDeckRarities[1].ToString() + " / 9";
        mythicalCost.text = deckManager.loadedDeckRarities[2].ToString() + " / 3";

        deckInfoPanel.SetActive(true);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        deckInfoPanel.SetActive(false);
    }
}
