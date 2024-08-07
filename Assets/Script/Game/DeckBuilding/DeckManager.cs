using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeckManager : MonoBehaviour, IDropHandler
{
    public int deck_length = -1;
    public int loaded_deck_index = 0;
    public RectTransform CardSlot;
    public RectTransform DeckSlot;
    public GameObject Simple_Card;
    public GameObject Simple_Deck;
    
    public bool newDeckSignal = false;
    public List<GameObject> TempDeck = new List<GameObject>();
    public List<List<GameObject>> DeckList = new List<List<GameObject>>();

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        DisplayCard CardInfo = dropped.GetComponent<DisplayCard>();

        if (CardInfo)
        {
            GameObject indeck = Instantiate(Simple_Card, CardSlot);
            SimpleCard indeck_info = indeck.GetComponent<SimpleCard>();

            indeck.name = dropped.name.Replace("display", "deck");
            indeck_info.cardindex = CardInfo.cardindex;
            indeck_info.cardNameText.text = CardInfo.CardName;
            indeck_info.cost.text = CardInfo.Cost.ToString();
            CardInfo.quantity--;

            Transform displaystorage = GetComponentInParent<DeckBuildingManager>().DisplayStorage;

            //카드가 한장 남으면 없애기
            if (CardInfo.quantity <= 0)
            {
                GetComponentInParent<DeckBuildingManager>().DisplayCardList.Remove(dropped);
                dropped.transform.SetParent(displaystorage);
                dropped.SetActive(false);
            }

            TempDeck.Add(indeck);
        }
    }

    //TODO 조금 더 가볍게 만들 수 있다면 그렇게 만들기
    public void TempDeckReset()
    {
        Transform trashcan = GetComponentInParent<DeckBuildingManager>().TrashCan;
        
        for(int i = CardSlot.childCount; i > 0; i--)
        {
            Transform card = CardSlot.GetChild(i - 1);
            card.SetParent(trashcan);
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
            SimpleDeck newDeckInfo = newDeckDisplay.GetComponent<SimpleDeck>();
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

    public void DeckCancel()
    {
        newDeckSignal = false;
        TempDeck.Clear();
    }

    public void DeckLoad(int deck_index)
    {
        loaded_deck_index = deck_index;
        foreach (var card in DeckList[loaded_deck_index])
        {
            GameObject indeck = Instantiate(Simple_Card, CardSlot);
            SimpleCard indeck_info = indeck.GetComponent<SimpleCard>();

            SimpleCard original_info = card.GetComponent<SimpleCard>();

            indeck_info.cardindex = original_info.cardindex;
            indeck_info.cardNameText.text = original_info.cardNameText.text;
            indeck_info.cost.text = original_info.cost.text;
            TempDeck.Add(indeck);
        }
        
        DeckBuildingManager dbm = GetComponentInParent<DeckBuildingManager>();

        //이거 이중 for문이라 문제가 생길 수 있다.
        foreach (var card in TempDeck)
        {
            int card_index = card.GetComponent<SimpleCard>().cardindex;

            foreach (var display in dbm.DisplayCardList)
            {
                DisplayCard displaycardinfo = display.GetComponent<DisplayCard>();
                if (displaycardinfo.cardindex == card_index)
                {
                    displaycardinfo.quantity -= 1;

                    if(displaycardinfo.quantity <= 0)
                    {
                        dbm.DisplayCardList.Remove(display);
                        display.transform.SetParent(dbm.DisplayStorage);
                        display.gameObject.SetActive(false);
                    }
                    break;
                }
            }
        }
    }

}