using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeckManager : MonoBehaviour, IDropHandler
{
    private int deck_length = -1;
    public int loaded_deck_index = 0;
    private int local_card_count = 0;
    private int[] local_chesspieces = new int[6]{0, 0, 0, 0, 0, 0};
    private int[] local_rarities = new int[3]{0, 0, 0};

    [SerializeField] private RectTransform trashcan;
    [SerializeField] private RectTransform CardSlot;
    [SerializeField] private RectTransform DeckSlot;
    [SerializeField] private GameObject Simple_Card;
    [SerializeField] private GameObject Simple_Deck;
    [SerializeField] private TMP_InputField deckname_inputfield;

    public bool newDeckSignal = false;
    public List<GameObject> TempDeck = new List<GameObject>();

    private DeckBuildingManager dbm;

    //덱 데이터로부터 덱 리스트와 이름 파일을 불러옵니다.
    public void Awake()
    {
        DeckListLoad();
        dbm = GetComponentInParent<DeckBuildingManager>();
    }

    //디스플레이된 카드를 덱에 넣으려고 할 때
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        DisplayCard CardInfo = dropped.GetComponent<DisplayCard>();

        if (CardInfo)
        {
            if (!TryInputCard(CardInfo)) //제한을 초과하지 않는 경우
            {
                GameObject indeck = Instantiate(Simple_Card, CardSlot);
                SimpleCard indeck_info = indeck.GetComponent<SimpleCard>();
                int card_order = 0;

                indeck.name = dropped.name.Replace("display", "deck");
                indeck_info.cardindex = CardInfo.cardindex;
                indeck_info.cardNameText.text = CardInfo.CardName;
                indeck_info.cost.text = CardInfo.Cost.ToString();
                CardInfo.quantity--;

                // 코스트가 낮을 수록 앞으로 가게끔
                for (; card_order < CardSlot.childCount - 1; card_order++)
                {
                    if (int.Parse(CardSlot.GetChild(card_order).GetComponent<SimpleCard>().cost.text) > CardInfo.Cost)
                    {
                        indeck.transform.SetSiblingIndex(card_order);
                        break;
                    }
                }

                Transform displaystorage = dbm.DisplayStorage;

                //덱에 더 이상 카드가 들어갈 수 없을 때 디스플레이에서 없애버립니다.
                if (CardInfo.quantity <= 0)
                {
                    dbm.DisplayCardList.Remove(dropped);
                    dropped.transform.SetParent(displaystorage);
                    dropped.SetActive(false);
                }

                AddCardInfoInDeck(CardInfo);
                TempDeck.Insert(card_order, indeck);
            }
        }
    }

    //TempDeck을 초기화 합니다.
    //TODO 더 효율적으로 바꿀 수 있다면 좋을 것 같습니다.
    public void TempDeckReset()
    {
        for (int i = CardSlot.childCount; i > 0; i--)
        {
            Transform card = CardSlot.GetChild(i - 1);
            card.SetParent(trashcan);
            card.gameObject.SetActive(false);
        }
    }

    //덱 생성 / 수정 후 저장
    public void DeckSave(int loaded_deck_index)
    {
        List<int> newDeckcards = MakeCardindexDeckList(TempDeck);

        if (newDeckSignal) // 덱 새로 생성 시
        {
            deck_length++;

            Deck newdeck = new Deck
            {
                deckname = deckname_inputfield.text,
                card_count = local_card_count,
                chesspieces = (int[])local_chesspieces.Clone(),
                Rarities = (int[])local_rarities.Clone(),
                cards = newDeckcards
            };
            GameManager.instance.deckList.Add(newdeck);

            GameObject newDeckDisplay = Instantiate(Simple_Deck, DeckSlot);
            SimpleDeck newDeckInfo = newDeckDisplay.GetComponent<SimpleDeck>();
            newDeckInfo.DeckNameText.text = newdeck.deckname;
            newDeckInfo.deck_index = deck_length;

            TempDeck.Clear();
            newDeckSignal = false;
        }
        else // 덱 수정 시
        {
            Deck loaded_deck = GameManager.instance.deckList[loaded_deck_index];
            loaded_deck.deckname = deckname_inputfield.text;
            loaded_deck.cards = newDeckcards;
            TempDeck.Clear();

            for (int i = 0; i < DeckSlot.childCount; i++)
            {
                Destroy(DeckSlot.GetChild(i).gameObject);
            }
            DeckListLoad();
            DeckInfoSave();
        }

        LocalDeckInfoReset();
        deckname_inputfield.text = "";
    }

    // 덱 생성 / 수정 취소
    public void DeckCancel()
    {
        newDeckSignal = false;
        TempDeck.Clear();
        LocalDeckInfoReset();
        deckname_inputfield.text = "";
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
        LocalDeckInfoLoad();
        deckname_inputfield.text = GameManager.instance.deckList[deck_index].deckname;
    }

    //덱 로드를 위해 index를 통해 gameobject 리스트로 변환
    public List<GameObject> MakeGameobjectDeckList(int deck_index)
    {
        List<GameObject> allcardlist = dbm.AllCardList;
        List<GameObject> GameobjectList = new List<GameObject>();

        foreach (var card_index in GameManager.instance.deckList[deck_index].cards)
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

    private void DeckListLoad()
    {
        deck_length = -1;
        List<Deck> decklist = GameManager.instance.deckList.ToList();
        if (decklist != null)
        {
            for (int i = 0; i < decklist.Count; i++)
            {
                if (decklist[i] != null)
                {
                    deck_length++;
                    GameObject newDeckDisplay = Instantiate(Simple_Deck, DeckSlot);
                    SimpleDeck newDeckInfo = newDeckDisplay.GetComponent<SimpleDeck>();
                    newDeckInfo.deck_index = i;
                    newDeckInfo.DeckNameText.text = decklist[i].deckname;
                }
            }
        }
    }

    private void LocalDeckInfoReset()
    {
        local_card_count = 0;
        Array.Clear(local_chesspieces, 0, local_chesspieces.Length);
        Array.Clear(local_rarities, 0, local_rarities.Length);
    }

    private void LocalDeckInfoLoad()
    {
        Deck loaded_deck = GameManager.instance.deckList[loaded_deck_index];

        local_card_count = loaded_deck.card_count;
        local_chesspieces = (int[])loaded_deck.chesspieces.Clone();
        local_rarities = (int[])loaded_deck.Rarities.Clone();
    }
    private void DeckInfoSave()
    {
        Deck loaded_deck = GameManager.instance.deckList[loaded_deck_index];

        loaded_deck.card_count = local_card_count;
        loaded_deck.chesspieces = (int[])local_chesspieces.Clone();
        loaded_deck.Rarities = (int[])local_rarities.Clone();
    }

    private bool TryInputCard(DisplayCard cardinfo)
    {
        bool error_signal = false;

        if (cardinfo.Rarity == Card.Rarity.Mythical)
        {
            if (local_rarities[2] == 3)
            {
                error_signal = true;
            }
        }
        else if (cardinfo.Rarity == Card.Rarity.Legendary)
        {
            if (local_rarities[1] == 9)
            {
                error_signal = true;
            }
        }

        return error_signal;
    }

    private void AddCardInfoInDeck(DisplayCard cardinfo)
    {
        local_card_count += 1;

        List<ChessPiece.PieceType> includedTypes = new List<ChessPiece.PieceType>();
        foreach (ChessPiece.PieceType piecetype in Enum.GetValues(typeof(ChessPiece.PieceType)))
        {
            if (piecetype != ChessPiece.PieceType.None && cardinfo.ChessPiece.HasFlag(piecetype))
            {
                includedTypes.Add(piecetype);
            }
        }

        foreach (var piecetype in includedTypes)
        {
            switch (piecetype)
            {
                case ChessPiece.PieceType.Pawn : local_chesspieces[0] += 1; break;
                case ChessPiece.PieceType.Knight : local_chesspieces[1] += 1; break;
                case ChessPiece.PieceType.Bishop : local_chesspieces[2] += 1; break;
                case ChessPiece.PieceType.Rook : local_chesspieces[3] += 1; break;
                case ChessPiece.PieceType.Quene : local_chesspieces[4] += 1; break;
                case ChessPiece.PieceType.King : local_chesspieces[5] += 1; break;
            }
        }

        switch (cardinfo.Rarity)
        {
            case Card.Rarity.Common : local_rarities[0] += 1; break;
            case Card.Rarity.Legendary : local_rarities[1] += 1; break;
            case Card.Rarity.Mythical : local_rarities[2] += 1; break;
        }
    }

    public void RemoveCardInfoInDeck(DisplayCard cardinfo)
    {
        local_card_count -= 1;

        List<ChessPiece.PieceType> includedTypes = new List<ChessPiece.PieceType>();
        foreach (ChessPiece.PieceType piecetype in Enum.GetValues(typeof(ChessPiece.PieceType)))
        {
            if (piecetype != ChessPiece.PieceType.None && cardinfo.ChessPiece.HasFlag(piecetype))
            {
                includedTypes.Add(piecetype);
            }
        }

        foreach (var piecetype in includedTypes)
        {
            switch (piecetype)
            {
                case ChessPiece.PieceType.Pawn : local_chesspieces[0] -= 1; break;
                case ChessPiece.PieceType.Knight : local_chesspieces[1] -= 1; break;
                case ChessPiece.PieceType.Bishop : local_chesspieces[2] -= 1; break;
                case ChessPiece.PieceType.Rook : local_chesspieces[3] -= 1; break;
                case ChessPiece.PieceType.Quene : local_chesspieces[4] -= 1; break;
                case ChessPiece.PieceType.King : local_chesspieces[5] -= 1; break;
            }
        }

        switch (cardinfo.Rarity)
        {
            case Card.Rarity.Common : local_rarities[0] -= 1; break;
            case Card.Rarity.Legendary : local_rarities[1] -= 1; break;
            case Card.Rarity.Mythical : local_rarities[2] -= 1; break;
        }
    }
}