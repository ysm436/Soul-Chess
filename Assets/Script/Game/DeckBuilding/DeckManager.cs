using System.Collections.Generic;
using System.Linq;
using TMPro;
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
    public List<List<int>> DeckList = new List<List<int>>();
    public List<string> DeckNameList = new List<string>();

    [SerializeField] private TMP_InputField deckname;

    public void Awake()
    {        
        if (DeckData.instance.DeckList != null)
        {
            DeckList = DeckData.instance.DeckList.ToList();
            DeckNameList = DeckData.instance.DeckNameList.ToList();

            for(int i = 0; i < DeckList.Count; i++)
            {
                if(DeckList[i] != null)
                {
                    deck_length++;
                    GameObject newDeckDisplay = Instantiate(Simple_Deck, DeckSlot);
                    SimpleDeck newDeckInfo = newDeckDisplay.GetComponent<SimpleDeck>();
                    newDeckInfo.deck_index = i;
                    newDeckInfo.DeckNameText.text = DeckNameList[i];
                }
            }
        }
        //로드할 때 덱 리스트에서 널값인 애들은 빼고 로드
        Debug.Log(DeckList.Count);
    }

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

    //newDeck의 card_index만 뽑아서 저장하도록 하기
    public void DeckSave(int loaded_deck_index)
    {
        List<int> newDeck = MakeCardindexDeckList(TempDeck);

        if(newDeckSignal) // 덱 생성 시
        {
            deck_length++;
            DeckList.Add(newDeck);
            GameObject newDeckDisplay = Instantiate(Simple_Deck, DeckSlot);
            SimpleDeck newDeckInfo = newDeckDisplay.GetComponent<SimpleDeck>();
            newDeckInfo.deck_index = deck_length;
            newDeckInfo.DeckNameText.text = deckname.text;
            DeckNameList.Add(deckname.text);
            TempDeck.Clear();
            newDeckSignal = false;
            Debug.Log(DeckList.Count);
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

    //덱이 card index만으로 로드될 수 있도록 처리
    public void DeckLoad(int deck_index)
    {
        loaded_deck_index = deck_index;

        List<GameObject> Loaded_deck = MakeGameobjectDeckList(loaded_deck_index);
        TempDeck = Loaded_deck.ToList();
        
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

                    if (displaycardinfo.quantity <= 0)
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

    public List<GameObject> MakeGameobjectDeckList(int deck_index)
    {
        List<GameObject> allcardlist = GetComponentInParent<DeckBuildingManager>().AllCardList;
        List<GameObject> GameobjectList = new List<GameObject>();

        foreach (var card_index in DeckList[deck_index])
        {
            Card cardinfo = allcardlist[card_index].GetComponent<Card>();
            GameObject indeck = Instantiate(Simple_Card, CardSlot);
            SimpleCard indeck_info = indeck.GetComponent<SimpleCard>();

            indeck_info.cardindex = card_index;
            indeck_info.cardNameText.text = cardinfo.cardName;
            indeck_info.cost.text = cardinfo.cost.ToString();

            GameobjectList.Add(indeck);
        }

        return GameobjectList;
    }

    public List<int> MakeCardindexDeckList(List<GameObject> gameobjectlist)
    {
        List<int> cardindexList = new List<int>();

        foreach (var gameobject in gameobjectlist)
        {
            cardindexList.Add(gameobject.GetComponent<SimpleCard>().cardindex);
        }

        return cardindexList;
    }

    public void DeckDataSave()
    {
        Debug.Log("??");
        DeckData.instance.DeckList = DeckList.ToList();
        DeckData.instance.DeckNameList = DeckNameList.ToList();
    }
}