using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System.Linq;

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
        DeckManager deckmanager = GetComponentInParent<DeckManager>();

        for (int i = 0; i < costGraph.childCount; i++)
        {
            RectTransform recttransform = costGraph.GetChild(i).GetComponent<RectTransform>();
            Vector2 graphSize = recttransform.sizeDelta;
            graphSize.y = 10 * deckmanager.loadedDeckCosts[i];
            recttransform.sizeDelta = graphSize;
        }

        soulCost.text = deckmanager.loadedDeckTypes[0].ToString();
        spellCost.text = deckmanager.loadedDeckTypes[1].ToString();

        commonCost.text = deckmanager.loadedDeckRarities[0].ToString();
        legendaryCost.text = deckmanager.loadedDeckRarities[1].ToString() + " / 9";
        mythicalCost.text = deckmanager.loadedDeckRarities[2].ToString() + " / 3";

        deckInfoPanel.SetActive(true);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        deckInfoPanel.SetActive(false);
    }
}
