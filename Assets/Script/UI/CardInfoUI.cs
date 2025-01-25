using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class CardInfoUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI cardNameText;
    [SerializeField] private GameObject[] costImages;
    [SerializeField] private TextMeshProUGUI cardCostText;
    [SerializeField] private GameObject[] cardRankes;

    private Card cardInfo = null;
    public void SetCardInfoUI(Card card)
    {
        cardNameText.text = card.cardName;
        cardCostText.text = card.cost.ToString();

        if (card is SoulCard)
        {
            costImages[0].SetActive(true);
            costImages[1].SetActive(false);
            cardCostText.color = new Color(92 / 255f, 0, 0);
        }
        else
        {
            costImages[0].SetActive(false);
            costImages[1].SetActive(true);
            cardCostText.color = new Color(29 / 255f, 62 / 255f, 7 / 225f);
        }

        foreach (var cardRank in cardRankes)
            cardRank.SetActive(false);

        for (int i = 0; i < (int)card.rarity; i++)
        {
            cardRankes[i].SetActive(true);
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

        EventTrigger.Entry entry_OnScroll = new EventTrigger.Entry();
        entry_OnScroll.eventID = EventTriggerType.Scroll;
        entry_OnScroll.callback.AddListener((data) => {OnScroll(data);});
        eventTrigger.triggers.Add(entry_OnScroll);
    }

    public void OnScroll(BaseEventData data)
    {
        transform.parent.parent.parent.GetComponent<ScrollRect>().OnScroll(data as PointerEventData);
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
