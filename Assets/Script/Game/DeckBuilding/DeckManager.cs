using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeckManager : MonoBehaviour, IDropHandler
{
    private int deck_length = -1;
    public int loaded_deck_index = 0;

    [SerializeField] private RectTransform CardSlot;
    [SerializeField] private RectTransform DeckSlot;
    [SerializeField] private GameObject Simple_Card;
    [SerializeField] private GameObject Simple_Deck;
    [SerializeField] private TMP_InputField deckname;
    
    public bool newDeckSignal = false;
    public List<GameObject> TempDeck = new List<GameObject>();
    private List<List<int>> DeckList = new List<List<int>>();
    private List<string> DeckNameList = new List<string>();

    private DeckBuildingManager dbm;

    //덱 데이터로부터 덱 리스트와 이름 파일을 불러옵니다.
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

        dbm = GetComponentInParent<DeckBuildingManager>();
    }

    //디스플레이된 카드를 덱에 넣으려고 할 때
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

            Transform displaystorage = dbm.DisplayStorage;

            //덱에 더 이상 카드가 들어갈 수 없을 때 디스플레이에서 없애버립니다.
            if (CardInfo.quantity <= 0)
            {
                dbm.DisplayCardList.Remove(dropped);
                dropped.transform.SetParent(displaystorage);
                dropped.SetActive(false);
            }

            TempDeck.Add(indeck);
        }
    }

    //TempDeck을 초기화 합니다.
    //TODO 더 효율적으로 바꿀 수 있다면 좋을 것 같습니다.
    public void TempDeckReset()
    {
        Transform trashcan = dbm.TrashCan;
        
        for(int i = CardSlot.childCount; i > 0; i--)
        {
            Transform card = CardSlot.GetChild(i - 1);
            card.SetParent(trashcan);
            card.gameObject.SetActive(false);
        }
    }

    //덱 생성 / 수정 후 저장
    public void DeckSave(int loaded_deck_index)
    {
        List<int> newDeck = MakeCardindexDeckList(TempDeck);

        if(newDeckSignal) // 덱 새로 생성 시
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
        }
        else // 덱 수정 시
        {
            DeckList.RemoveAt(loaded_deck_index);
            DeckList.Insert(loaded_deck_index, newDeck);
            TempDeck.Clear();
            DeckNameList[loaded_deck_index] = deckname.text;
        }

        deckname.text = "";
    }

    // 덱 생성 / 수정 취소
    public void DeckCancel()
    {
        newDeckSignal = false;
        TempDeck.Clear();
        deckname.text = "";
    }

    //덱 로드
    public void DeckLoad(int deck_index)
    {
        loaded_deck_index = deck_index;

        List<GameObject> Loaded_deck = MakeGameobjectDeckList(loaded_deck_index);
        TempDeck = Loaded_deck.ToList();

        //TODO 이중 for문이라 조금 더 효율적으로 바꿀 수 있으면 좋을 것 같습니다.
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

        deckname.text = DeckNameList[loaded_deck_index];
    }   

    //덱 로드를 위해 index를 통해 gameobject 리스트로 변환
    public List<GameObject> MakeGameobjectDeckList(int deck_index)
    {
        List<GameObject> allcardlist = dbm.AllCardList;
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

    //덱 리스트 저장을 위해 gameobject에서 카드 index만 뽑아 int 리스트로 변환
    public List<int> MakeCardindexDeckList(List<GameObject> gameobjectlist)
    {
        List<int> cardindexList = new List<int>();

        foreach (var gameobject in gameobjectlist)
        {
            cardindexList.Add(gameobject.GetComponent<SimpleCard>().cardindex);
        }

        return cardindexList;
    }

    //quit 버튼을 누르면 덱의 데이터를 저장합니다.
    public void DeckDataSave()
    {
        DeckData.instance.DeckList = DeckList.ToList();
        DeckData.instance.DeckNameList = DeckNameList.ToList();
    }
}