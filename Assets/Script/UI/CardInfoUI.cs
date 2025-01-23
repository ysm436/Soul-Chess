using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardInfoUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI cardNameText;
    [SerializeField] private GameObject[] costImages;
    [SerializeField] private TextMeshProUGUI cardCostText;

    private Card cardInfo = null;
    public void SetCardInfoUI(Card card)
    {
        cardNameText.text = card.cardName;
        cardCostText.text = card.cost.ToString();

        if (card is SoulCard)
        {
            costImages[0].SetActive(true);
            costImages[1].SetActive(false);
            cardCostText.color = Color.black;
        }
        else
        {
            costImages[0].SetActive(false);
            costImages[1].SetActive(true);
            cardCostText.color = Color.white;
        }

        cardInfo = card;

        AddEvent();
    }

    public void AddEvent()
    {
        EventTrigger eventTrigger = GetComponent<EventTrigger>();

        EventTrigger.Entry entry_PointerEnter = new EventTrigger.Entry();
        entry_PointerEnter.eventID = EventTriggerType.PointerEnter;
        entry_PointerEnter.callback.AddListener((data) => { ShowCardUI(); });
        eventTrigger.triggers.Add(entry_PointerEnter);

        EventTrigger.Entry entry_PointerExit = new EventTrigger.Entry();
        entry_PointerExit.eventID = EventTriggerType.PointerExit;
        entry_PointerExit.callback.AddListener((data) => { HideCardUI(); });
        eventTrigger.triggers.Add(entry_PointerExit);
    }

    [SerializeField] private CardUI cardUI;

    public void ShowCardUI()
    {
        if (cardInfo == null)
            return;
        cardUI.SetCardUI(cardInfo);
        cardUI.gameObject.SetActive(true);
    }

    public void HideCardUI()
    {
        cardUI.gameObject.SetActive(false);
    }
}
