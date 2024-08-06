using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DeckManager : MonoBehaviour, IDropHandler
{
    public int deck_length = -1;
    public int loaded_deck_index = 0;
    public RectTransform CardSlot;
    public RectTransform DeckSlot;
    public GameObject Simple_Card;
    public GameObject Simple_Deck;
    public GameObject TrashCan;
    public bool newDeckSignal = false;
    public List<GameObject> TempDeck = new List<GameObject>();
    public List<List<GameObject>> DeckList = new List<List<GameObject>>();

    public void OnDrop(PointerEventData eventData)
    {
        CardForDeckBuilding CardInfo = eventData.pointerDrag.GetComponent<CardForDeckBuilding>();

        if (CardInfo)
        {
            GameObject indeck = Instantiate(Simple_Card, CardSlot);
            CardInDeck indeck_info = indeck.GetComponent<CardInDeck>();

            indeck.name = eventData.pointerDrag.name.Replace("display", "deck");
            indeck_info.cardindex = CardInfo.cardindex;
            indeck_info.cardNameText.text = CardInfo.CardName;
            indeck_info.cost.text = CardInfo.Cost.ToString();

            Destroy(eventData.pointerDrag);

            TempDeck.Add(indeck);
        }
    }

    //TODO 조금 더 가볍게 만들 수 있다면 그렇게 만들기
    public void TempDeckReset()
    {
        for(int i = CardSlot.childCount; i > 0; i--)
        {
            Transform card = CardSlot.GetChild(i - 1);
            card.SetParent(TrashCan.transform);
            card.gameObject.SetActive(false);
        }
    }

    public void DeckSave(int loaded_deck_index)
    {
        List<GameObject> newDeck = TempDeck.ToList();
        
        if(newDeckSignal) // 덱 생성 시
        {
            deck_length++;
            DeckList.Add(newDeck);
            GameObject newDeckDisplay = Instantiate(Simple_Deck, DeckSlot);
            DeckInfo newDeckInfo = newDeckDisplay.GetComponent<DeckInfo>();
            newDeckInfo.deck_index = deck_length;
            TempDeck.Clear();
            newDeckSignal = false;
        }
        else // 덱 수정 시
        {
            DeckList.RemoveAt(loaded_deck_index);
            DeckList.Insert(loaded_deck_index, newDeck);
            TempDeck.Clear();
        }
    }

    public void DeckLoad(int index)
    {
        loaded_deck_index = index;
        foreach (var card in DeckList[index])
        {
            GameObject indeck = Instantiate(Simple_Card, CardSlot);
            CardInDeck indeck_info = indeck.GetComponent<CardInDeck>();

            CardInDeck original_info = card.GetComponent<CardInDeck>();

            indeck_info.cardindex = original_info.cardindex;
            indeck_info.cardNameText.text = original_info.cardNameText.text;
            indeck_info.cost.text = original_info.cost.text;
        }

        TempDeck = DeckList[index].ToList();
    }

}