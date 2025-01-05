using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeckManager : MonoBehaviour, IDropHandler
{
    private int CARD_LIMIT = 30;
    public int loaded_deck_index = 0;
    public int local_card_count = 0;
    public int[] local_costs = new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
    public int[] local_chesspieces = new int[6] { 0, 0, 0, 0, 0, 0 };
    public int[] local_extra_chesspieces = new int[6] { 0, 0, 0, 0, 0, 0 };
    public int[] local_rarities = new int[3] { 0, 0, 0 };

    [SerializeField] private RectTransform trashcan;
    [SerializeField] private RectTransform CardSlot;
    [SerializeField] private RectTransform DeckSlot;
    [SerializeField] private GameObject Simple_Card;
    [SerializeField] private GameObject Simple_Deck;
    [SerializeField] private TMP_InputField deckname_inputfield;

    [SerializeField] public GameObject CautionPanel;
    [SerializeField] public TextMeshProUGUI CautionText;
    [SerializeField] private TextMeshProUGUI CardCountText;

    public bool newDeckSignal = false;
    private bool duplicateSignal = false;
    private int tempdeckinputindex = 0;
    public List<int> TempDeck = new List<int>();

    private DeckBuildingManager dbm;
    
    public bool debug_button = false;

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
        Transform displaystorage = dbm.DisplayStorage;
        DisplayCard CardInfo = dropped.GetComponent<DisplayCard>();

        if (CardInfo)
        {
            if (!TryInputCard(CardInfo)) //제한을 초과하지 않는 경우
            {
                if (duplicateSignal)
                {
                    int card_index = CardInfo.cardindex;
                    for (int simple_index = 0; simple_index < CardSlot.childCount; simple_index++)
                    {
                        SimpleCard simplecard_info = CardSlot.GetChild(simple_index).GetComponent<SimpleCard>();
                        if (simplecard_info.cardindex == card_index)
                        {
                            simplecard_info.quantity += 1;
                            break;
                        }
                    }
                    TempDeck.Insert(tempdeckinputindex, card_index);
                }
                else
                {
                    GameObject simplecard = Instantiate(Simple_Card, CardSlot);
                    SimpleCard simplecard_info = simplecard.GetComponent<SimpleCard>();

                    simplecard.name = dropped.name.Replace("display", "deck");
                    simplecard_info.cardindex = CardInfo.cardindex;
                    simplecard_info.cardNameText.text = CardInfo.CardName;
                    simplecard_info.cost.text = CardInfo.Cost.ToString();
                    simplecard_info.quantity = 1;

                    // 코스트가 낮을 수록 앞으로 가게끔
                    for (int card_order = 0; card_order < CardSlot.childCount - 1; card_order++)
                    {
                        if (int.Parse(CardSlot.GetChild(card_order).GetComponent<SimpleCard>().cost.text) > CardInfo.Cost)
                        {
                            simplecard.transform.SetSiblingIndex(card_order);
                            break;
                        }
                    }

                    TempDeck.Insert(tempdeckinputindex, simplecard_info.cardindex);
                }

                CardInfo.quantity--;
                //덱에 더 이상 카드가 들어갈 수 없을 때 디스플레이에서 없애버립니다.
                if (CardInfo.quantity <= 0)
                {
                    dbm.DisplayCardList.Remove(dropped);
                    dropped.transform.SetParent(displaystorage);
                    dropped.SetActive(false);
                }
                AddCardInfoInDeck(CardInfo);
            }
            else
            {
                CautionPanel.SetActive(true);
            }
        }
    }

    //CardSlot을 초기화 합니다.
    public void CardSlotReset()
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
        List<int> newDeckcards = TempDeck.ToList();

        if (newDeckSignal) // 덱 새로 생성 시
        {
            Deck newdeck = new Deck
            {
                index = GameManager.instance.deckList.Count,
                deckname = deckname_inputfield.text,
                card_count = local_card_count,
                costs = (int[])local_costs.Clone(),
                chesspieces = (int[])local_chesspieces.Clone(),
                extra_chesspieces = (int[])local_extra_chesspieces.Clone(),
                Rarities = (int[])local_rarities.Clone(),
                cards = newDeckcards
            };
            GameManager.instance.deckList.Add(newdeck);

            GameObject newDeckDisplay = Instantiate(Simple_Deck, DeckSlot);
            SimpleDeck newDeckInfo = newDeckDisplay.GetComponent<SimpleDeck>();
            newDeckInfo.DeckNameText.text = newdeck.deckname;
            newDeckInfo.deck_index = newdeck.index;

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
        TempDeck = GameManager.instance.deckList[loaded_deck_index].cards.ToList();

        MakeSimpleCard();
        LocalDeckInfoLoad(loaded_deck_index);

        deckname_inputfield.text = GameManager.instance.deckList[loaded_deck_index].deckname;
    }

    public void MakeSimpleCard()
    {
        List<GameObject> allcardlist = GameManager.instance.AllCards.ToList();

        for (int i = 0; i < TempDeck.Count; i++)
        {
            int card = TempDeck[i];

            if (i == 0 || card != TempDeck[i - 1])
            {
                Card cardinfo = allcardlist[card].GetComponent<Card>();
                GameObject simplecard = Instantiate(Simple_Card, CardSlot);
                SimpleCard simplecard_info = simplecard.GetComponent<SimpleCard>();

                simplecard_info.cardindex = card;
                simplecard_info.cardNameText.text = cardinfo.cardName;
                simplecard_info.cost.text = cardinfo.cost.ToString();
                simplecard_info.quantity = 1;
            }
            else
            {
                for (int simple_index = 0; simple_index < CardSlot.childCount; simple_index++)
                {
                    SimpleCard simplecard_info = CardSlot.GetChild(simple_index).GetComponent<SimpleCard>();
                    if (simplecard_info.cardindex == card)
                    {
                        simplecard_info.quantity += 1;
                        break;
                    }
                }
            }

            foreach (var display in dbm.DisplayCardList)
            {
                DisplayCard displaycardinfo = display.GetComponent<DisplayCard>();
                if (displaycardinfo.cardindex == card)
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

    private void DeckListLoad()
    {
        List<Deck> decklist = GameManager.instance.deckList.ToList();
        if (decklist != null)
        {
            for (int i = 0; i < decklist.Count; i++)
            {
                if (decklist[i].index != -1)
                {
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
        Array.Clear(local_costs, 0, local_costs.Length);
        Array.Clear(local_chesspieces, 0, local_chesspieces.Length);
        Array.Clear(local_extra_chesspieces, 0, local_extra_chesspieces.Length);
        Array.Clear(local_rarities, 0, local_rarities.Length);
    }

    private void LocalDeckInfoLoad(int index)
    {
        Deck loaded_deck = GameManager.instance.deckList[index];

        local_card_count = loaded_deck.card_count;
        local_costs = (int[])loaded_deck.costs.Clone();
        local_chesspieces = (int[])loaded_deck.chesspieces.Clone();
        local_extra_chesspieces = (int[])loaded_deck.extra_chesspieces.Clone();
        local_rarities = (int[])loaded_deck.Rarities.Clone();
        CardCountText.text = "카드개수 : " + local_card_count.ToString() + " / 30";
    }
    private void DeckInfoSave()
    {
        Deck loaded_deck = GameManager.instance.deckList[loaded_deck_index];

        loaded_deck.card_count = local_card_count;
        loaded_deck.costs = (int[])local_costs.Clone();
        loaded_deck.chesspieces = (int[])local_chesspieces.Clone();
        loaded_deck.extra_chesspieces = (int[])local_extra_chesspieces.Clone();
        loaded_deck.Rarities = (int[])local_rarities.Clone();
    }

    private bool TryInputCard(DisplayCard cardinfo)
    {
        List<GameObject> allcardlist = GameManager.instance.AllCards.ToList();
        bool error_signal = false;
        int duplicate_quantity = 0;

        duplicateSignal = false;
        tempdeckinputindex = TempDeck.Count;

        foreach (var card in TempDeck)
        {
            if (cardinfo.cardindex == card)
            {
                duplicate_quantity += 1;
                duplicateSignal = true;
                tempdeckinputindex = TempDeck.IndexOf(card);
            }
        }

        if (!duplicateSignal)
        {
            for (int i = 0; i < TempDeck.Count; i++)
            {
                int temp_cost = allcardlist[TempDeck[i]].GetComponent<Card>().cost;

                if (cardinfo.Cost == temp_cost)
                {
                    tempdeckinputindex = i + 1;
                }
                else
                {
                    if (cardinfo.Cost < temp_cost)
                    {
                        tempdeckinputindex = i;
                        break;
                    }
                }

            }
        }

        if (debug_button)
            return false;

        if (local_card_count == 30)
        {
            CautionText.text = "덱에 카드는 " + CARD_LIMIT.ToString() + "개만 넣을 수 있습니다.";
            error_signal = true;
        }
        else if (cardinfo.Rarity == Card.Rarity.Mythical)
        {
            if (duplicate_quantity >= 1)
            {
                CautionText.text = "신화카드는 덱에 동일한 카드를 한장만 넣을 수 있습니다.";
                error_signal = true;
            }

            if (local_rarities[2] == 5)
            {
                CautionText.text = "신화카드는 덱에 5장만 넣을 수 있습니다.";
                error_signal = true;
            }
        }
        else if (cardinfo.Rarity == Card.Rarity.Legendary)
        {
            if (duplicate_quantity >= 1)
            {
                CautionText.text = "전설카드는 덱에 동일한 카드를 한장만 넣을 수 있습니다.";
                error_signal = true;
            }

            /* if (local_rarities[1] == 9)
            {
                CautionText.text = "전설카드는 덱에 9장만 넣을 수 있습니다.";
                error_signal = true;
            } */
        }
        else
        {
            if (duplicate_quantity >= 2)
            {
                CautionText.text = "일반카드는 덱에 동일한 카드를 2장만 넣을 수 있습니다.";
                error_signal = true;
            }
        }

        return error_signal;
    }

    private void AddCardInfoInDeck(DisplayCard cardinfo)
    {
        local_card_count += 1;

        switch (cardinfo.Cost)
        {
            case 0: local_costs[0] += 1; break;
            case 1: local_costs[1] += 1; break;
            case 2: local_costs[2] += 1; break;
            case 3: local_costs[3] += 1; break;
            case 4: local_costs[4] += 1; break;
            case 5: local_costs[5] += 1; break;
            case 6: local_costs[6] += 1; break;
            case 7: local_costs[7] += 1; break;
            case 8: local_costs[8] += 1; break;
            default: local_costs[9] += 1; break;
        }

        List<ChessPiece.PieceType> includedTypes = new List<ChessPiece.PieceType>();
        foreach (ChessPiece.PieceType piecetype in Enum.GetValues(typeof(ChessPiece.PieceType)))
        {
            if (piecetype != ChessPiece.PieceType.None && cardinfo.ChessPiece.HasFlag(piecetype))
            {
                includedTypes.Add(piecetype);
            }
        }

        for (int i = includedTypes.Count - 1; i >= 0; i--)
        {
            if (i == includedTypes.Count - 1)
            {
                switch (includedTypes[i])
                {
                    case ChessPiece.PieceType.Pawn: local_chesspieces[0] += 1; break;
                    case ChessPiece.PieceType.Knight: local_chesspieces[1] += 1; break;
                    case ChessPiece.PieceType.Bishop: local_chesspieces[2] += 1; break;
                    case ChessPiece.PieceType.Rook: local_chesspieces[3] += 1; break;
                    case ChessPiece.PieceType.Quene: local_chesspieces[4] += 1; break;
                    case ChessPiece.PieceType.King: local_chesspieces[5] += 1; break;
                }
            }
            else
            {
                switch (includedTypes[i])
                {
                    case ChessPiece.PieceType.Pawn: local_extra_chesspieces[0] += 1; break;
                    case ChessPiece.PieceType.Knight: local_extra_chesspieces[1] += 1; break;
                    case ChessPiece.PieceType.Bishop: local_extra_chesspieces[2] += 1; break;
                    case ChessPiece.PieceType.Rook: local_extra_chesspieces[3] += 1; break;
                    case ChessPiece.PieceType.Quene: local_extra_chesspieces[4] += 1; break;
                    case ChessPiece.PieceType.King: local_extra_chesspieces[5] += 1; break;
                }
            }
        }

        switch (cardinfo.Rarity)
        {
            case Card.Rarity.Common: local_rarities[0] += 1; break;
            case Card.Rarity.Legendary: local_rarities[1] += 1; break;
            case Card.Rarity.Mythical: local_rarities[2] += 1; break;
        }

        CardCountText.text = "카드개수 : " + local_card_count.ToString() + " / 30";
    }

    public void RemoveCardInfoInDeck(DisplayCard cardinfo)
    {
        local_card_count -= 1;

        switch (cardinfo.Cost)
        {
            case 0: local_costs[0] -= 1; break;
            case 1: local_costs[1] -= 1; break;
            case 2: local_costs[2] -= 1; break;
            case 3: local_costs[3] -= 1; break;
            case 4: local_costs[4] -= 1; break;
            case 5: local_costs[5] -= 1; break;
            case 6: local_costs[6] -= 1; break;
            case 7: local_costs[7] -= 1; break;
            case 8: local_costs[8] -= 1; break;
            default: local_costs[9] -= 1; break;
        }

        List<ChessPiece.PieceType> includedTypes = new List<ChessPiece.PieceType>();
        foreach (ChessPiece.PieceType piecetype in Enum.GetValues(typeof(ChessPiece.PieceType)))
        {
            if (piecetype != ChessPiece.PieceType.None && cardinfo.ChessPiece.HasFlag(piecetype))
            {
                includedTypes.Add(piecetype);
            }
        }

        for (int i = includedTypes.Count - 1; i >= 0; i--)
        {
            if (i == includedTypes.Count - 1)
            {
                switch (includedTypes[i])
                {
                    case ChessPiece.PieceType.Pawn: local_chesspieces[0] -= 1; break;
                    case ChessPiece.PieceType.Knight: local_chesspieces[1] -= 1; break;
                    case ChessPiece.PieceType.Bishop: local_chesspieces[2] -= 1; break;
                    case ChessPiece.PieceType.Rook: local_chesspieces[3] -= 1; break;
                    case ChessPiece.PieceType.Quene: local_chesspieces[4] -= 1; break;
                    case ChessPiece.PieceType.King: local_chesspieces[5] -= 1; break;
                }
            }
            else
            {
                switch (includedTypes[i])
                {
                    case ChessPiece.PieceType.Pawn: local_extra_chesspieces[0] -= 1; break;
                    case ChessPiece.PieceType.Knight: local_extra_chesspieces[1] -= 1; break;
                    case ChessPiece.PieceType.Bishop: local_extra_chesspieces[2] -= 1; break;
                    case ChessPiece.PieceType.Rook: local_extra_chesspieces[3] -= 1; break;
                    case ChessPiece.PieceType.Quene: local_extra_chesspieces[4] -= 1; break;
                    case ChessPiece.PieceType.King: local_extra_chesspieces[5] -= 1; break;
                }
            }
        }

        switch (cardinfo.Rarity)
        {
            case Card.Rarity.Common: local_rarities[0] -= 1; break;
            case Card.Rarity.Legendary: local_rarities[1] -= 1; break;
            case Card.Rarity.Mythical: local_rarities[2] -= 1; break;
        }

        CardCountText.text = "카드개수 : " + local_card_count.ToString() + " / " + CARD_LIMIT.ToString();
    }

    public void CancelCaution()
    {
        CautionPanel.SetActive(false);
    }


}