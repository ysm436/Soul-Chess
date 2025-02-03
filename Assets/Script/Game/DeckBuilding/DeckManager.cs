using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeckManager : MonoBehaviour
{
    private int CARD_LIMIT = 24;
    public int loadedDeckIndex = 0;
    public int loadedDeckCardCount = 0;
    public int[] loadedDeckCosts = new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
    public int[] loadedDeckTypes = new int[2] { 0, 0 };
    public int[] loadedDeckRarities = new int[3] { 0, 0, 0 };

    [SerializeField] private RectTransform trashCan;
    [SerializeField] private RectTransform cardInDeckSlot;
    [SerializeField] private RectTransform deckSlot;
    [SerializeField] private GameObject cardInDeckPrefab;
    [SerializeField] private GameObject deckObj;
    [SerializeField] private TMP_InputField deckNameInputfield;

    [SerializeField] public GameObject cautionPanel;
    [SerializeField] public TextMeshProUGUI cautionText;
    [SerializeField] private TextMeshProUGUI cardCountText;
    [SerializeField] private Text legendaryCountText;
    [SerializeField] private Text mythicalCountText;

    public bool newDeckSignal = false;
    private bool duplicateSignal = false;
    private int tempDeckInputIndex = 0;
    public List<int> tempDeck = new List<int>();

    private DeckBuildingManager dbm;
    
    public bool debugButton = false;

    //덱 데이터로부터 덱 리스트와 이름 파일을 불러옵니다.
    public void Awake()
    {
        DeckListLoad();
        dbm = GetComponentInParent<DeckBuildingManager>();
    }

    //디스플레이된 카드를 덱에 넣으려고 할 때
    public void InsertCardInDeck(DisplayInfo cardInfo)
    {
        DisplayInfo targetCard = dbm.cardDisplayList[cardInfo.cardDisplayIndex].GetComponent<DisplayInfo>();

        if (targetCard)
        {
            if (!TryInputCard(targetCard)) //제한을 초과하지 않는 경우
            {
                if (duplicateSignal)
                {
                    int displayCardIndex = targetCard.cardDisplayIndex;
                    for (int cardInDeckIndex = 0; cardInDeckIndex < cardInDeckSlot.childCount; cardInDeckIndex++)
                    {
                        CardInDeck cardInDeckInfo = cardInDeckSlot.GetChild(cardInDeckIndex).GetComponent<CardInDeck>();
                        if (cardInDeckInfo.cardDisplayIndex == displayCardIndex)
                        {
                            cardInDeckInfo.Quantity += 1;
                            break;
                        }
                    }
                    tempDeck.Insert(tempDeckInputIndex, displayCardIndex);
                }
                else
                {
                    GameObject cardInDeck = Instantiate(cardInDeckPrefab, cardInDeckSlot);
                    CardInDeck cardInDeckInfo = cardInDeck.GetComponent<CardInDeck>();

                    cardInDeckInfo.name = cardInfo.name.Replace("Display", "InDeck");
                    cardInDeckInfo.cardDisplayIndex = targetCard.cardDisplayIndex;
                    cardInDeckInfo.cardIndex = targetCard.cardIndex;
                    cardInDeckInfo.cardNameText.text = targetCard.CardName;
                    cardInDeckInfo.costText.text = targetCard.Cost.ToString();
                    cardInDeckInfo.Quantity = 1;

                    for (int cardInDeckOrder = 0; cardInDeckOrder < cardInDeckSlot.childCount - 1; cardInDeckOrder++)
                    {
                        if (cardInDeckSlot.GetChild(cardInDeckOrder).GetComponent<CardInDeck>().cardDisplayIndex > targetCard.cardDisplayIndex)
                        {
                            cardInDeck.transform.SetSiblingIndex(cardInDeckOrder);
                            break;
                        }
                    }
                    tempDeck.Insert(tempDeckInputIndex, cardInDeckInfo.cardDisplayIndex);
                }

                targetCard.Quantity--;
                AddCardInfoInDeck(targetCard);
                Destroy(cardInfo.gameObject);
            }
            else
            {
                cautionPanel.SetActive(true);
            }
        }
    }

    private bool TryInputCard(DisplayInfo cardInfo)
    {
        bool errorSignal = false;
        duplicateSignal = false;
        tempDeckInputIndex = tempDeck.Count;

        foreach (var card in tempDeck)
        {
            if (cardInfo.cardDisplayIndex == card)
            {
                duplicateSignal = true;
                tempDeckInputIndex = tempDeck.IndexOf(card);
            }
        }

        if (!duplicateSignal)
        {
            for (int i = 0; i < tempDeck.Count; i++)
            {
                if (cardInfo.cardDisplayIndex <= tempDeck[i])
                {
                    tempDeckInputIndex = i;
                    break;
                }
            }
        }

        if (debugButton)
            return false;

        if (loadedDeckCardCount == CARD_LIMIT)
        {
            cautionText.text = "덱에 카드는 " + CARD_LIMIT.ToString() + "개만 넣을 수 있습니다.";
            errorSignal = true;
        }
        else if (cardInfo.Rarity == Card.Rarity.Mythical)
        {
            if (loadedDeckRarities[2] == 3)
            {
                cautionText.text = "신화카드는 덱에 3장만 넣을 수 있습니다.";
                errorSignal = true;
            }
        }
        else if (cardInfo.Rarity == Card.Rarity.Legendary)
        {
            if (loadedDeckRarities[1] == 9)
            {
                cautionText.text = "전설카드는 덱에 9장만 넣을 수 있습니다.";
                errorSignal = true;
            }
        }

        return errorSignal;
    }

    // CardSlot을 초기화 합니다.
    public void CardSlotReset()
    {
        for (int i = cardInDeckSlot.childCount; i > 0; i--)
        {
            Transform card = cardInDeckSlot.GetChild(i - 1);
            card.SetParent(trashCan);
            card.gameObject.SetActive(false);
        }
    }

    // 덱 생성 / 수정 후 저장
    public void DeckSave(int loadedDeckIndex)
    {
        List<int> newDeckcards = new List<int>();

        foreach (var displayIndex in tempDeck)
        {
            newDeckcards.Add(dbm.cardDisplayList[displayIndex].GetComponent<DisplayInfo>().cardIndex);
        }

        if (newDeckSignal) // 덱 새로 생성 시
        {
            Deck newdeck = new Deck
            {
                index = GameManager.instance.deckList.Count,
                deckName = deckNameInputfield.text,
                cardCount = loadedDeckCardCount,
                costs = (int[])loadedDeckCosts.Clone(),
                types = (int[])loadedDeckTypes.Clone(),
                rarities = (int[])loadedDeckRarities.Clone(),
                cards = newDeckcards
            };
            GameManager.instance.deckList.Add(newdeck);

            GameObject newDeckDisplay = Instantiate(deckObj, deckSlot);
            deckObj newDeckInfo = newDeckDisplay.GetComponent<deckObj>();
            newDeckInfo.deckNameText.text = newdeck.deckName;
            newDeckInfo.deckIndex = newdeck.index;

            tempDeck.Clear();
            newDeckSignal = false;
        }
        else // 덱 수정 시
        {
            Deck loadedDeck = GameManager.instance.deckList[loadedDeckIndex];
            loadedDeck.deckName = deckNameInputfield.text;
            loadedDeck.cards = newDeckcards;
            tempDeck.Clear();

            for (int i = 0; i < deckSlot.childCount; i++)
            {
                Destroy(deckSlot.GetChild(i).gameObject);
            }
            DeckListLoad();
            DeckInfoSave();
        }

        LocalDeckInfoReset();
        deckNameInputfield.text = "";
    }

    // 덱 생성 / 수정 취소
    public void DeckCancel()
    {
        newDeckSignal = false;
        tempDeck.Clear();
        LocalDeckInfoReset();
        deckNameInputfield.text = "";
    }

    // 덱 로드
    public void DeckLoad(int deckIndex)
    {
        loadedDeckIndex = deckIndex;

        foreach (var cardIndex in GameManager.instance.deckList[loadedDeckIndex].cards)
        {
            tempDeck.Add(dbm.cardIndexList.IndexOf(cardIndex));
        }

        MakeCardInDeck();
        LocalDeckInfoLoad(loadedDeckIndex);
        deckNameInputfield.text = GameManager.instance.deckList[loadedDeckIndex].deckName;
    }

    public void MakeCardInDeck()
    {
        for (int i = 0; i < tempDeck.Count; i++)
        {
            int targetDisplayCardIndex = tempDeck[i];
            DisplayInfo targetCard = dbm.cardDisplayList[targetDisplayCardIndex].GetComponent<DisplayInfo>();

            if (i == 0 || targetDisplayCardIndex != tempDeck[i - 1])
            {
                GameObject cardInDeck = Instantiate(cardInDeckPrefab, cardInDeckSlot);
                CardInDeck cardInDeckInfo = cardInDeck.GetComponent<CardInDeck>();

                cardInDeckInfo.cardDisplayIndex = targetDisplayCardIndex;
                cardInDeckInfo.cardIndex = targetCard.cardIndex;
                cardInDeckInfo.cardNameText.text = targetCard.CardName;
                cardInDeckInfo.costText.text = targetCard.Cost.ToString();
                cardInDeckInfo.Quantity = 1;
            }
            else
            {
                for (int cardInDeckIndex = 0; cardInDeckIndex < cardInDeckSlot.childCount; cardInDeckIndex++)
                {
                    CardInDeck cardInDeckInfo = cardInDeckSlot.GetChild(cardInDeckIndex).GetComponent<CardInDeck>();
                    if (cardInDeckInfo.cardDisplayIndex == targetDisplayCardIndex)
                    {
                        cardInDeckInfo.Quantity += 1;
                        break;
                    }
                }
            }
            targetCard.Quantity -= 1;
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
                    GameObject newDeckDisplay = Instantiate(deckObj, deckSlot);
                    deckObj newDeckInfo = newDeckDisplay.GetComponent<deckObj>();
                    newDeckInfo.deckIndex = i;
                    newDeckInfo.deckNameText.text = decklist[i].deckName;
                }
            }
        }
    }

    private void LocalDeckInfoReset()
    {
        loadedDeckCardCount = 0;
        Array.Clear(loadedDeckCosts, 0, loadedDeckCosts.Length);
        Array.Clear(loadedDeckTypes, 0, loadedDeckTypes.Length);
        Array.Clear(loadedDeckRarities, 0, loadedDeckRarities.Length);
    }

    private void LocalDeckInfoLoad(int index)
    {
        Deck loadedDeck = GameManager.instance.deckList[index];

        loadedDeckCardCount = loadedDeck.cardCount;
        loadedDeckCosts = (int[])loadedDeck.costs.Clone();
        loadedDeckTypes = (int[])loadedDeck.types.Clone();
        loadedDeckRarities = (int[])loadedDeck.rarities.Clone();

        if (loadedDeckRarities[1] == 9)
        {
            foreach (var cardDisplay in dbm.cardDisplayList)
            {
                DisplayInfo targetDisplay = cardDisplay.GetComponent<DisplayInfo>();
                if (targetDisplay.Rarity == Card.Rarity.Legendary)
                {
                    targetDisplay.DisplayUnAvailable();
                }
            }
        }

        if (loadedDeckRarities[2] == 3)
        {
            foreach (var cardDisplay in dbm.cardDisplayList)
            {
                DisplayInfo targetDisplay = cardDisplay.GetComponent<DisplayInfo>();
                if (targetDisplay.Rarity == Card.Rarity.Mythical)
                {
                    targetDisplay.DisplayUnAvailable();
                }
            }
        }
        cardCountText.text = "카드 : " + loadedDeckCardCount.ToString() + " / " + CARD_LIMIT.ToString();
    }
    private void DeckInfoSave()
    {
        Deck loadedDeck = GameManager.instance.deckList[loadedDeckIndex];

        loadedDeck.cardCount = loadedDeckCardCount;
        loadedDeck.costs = (int[])loadedDeckCosts.Clone();
        loadedDeck.types = (int[])loadedDeckTypes.Clone();
        loadedDeck.rarities = (int[])loadedDeckRarities.Clone();
    }

    private void AddCardInfoInDeck(DisplayInfo cardInfo)
    {
        loadedDeckCardCount += 1;

        switch (cardInfo.Cost)
        {
            case 0: loadedDeckCosts[0] += 1; break;
            case 1: loadedDeckCosts[1] += 1; break;
            case 2: loadedDeckCosts[2] += 1; break;
            case 3: loadedDeckCosts[3] += 1; break;
            case 4: loadedDeckCosts[4] += 1; break;
            case 5: loadedDeckCosts[5] += 1; break;
            case 6: loadedDeckCosts[6] += 1; break;
            case 7: loadedDeckCosts[7] += 1; break;
            case 8: loadedDeckCosts[8] += 1; break;
            default: loadedDeckCosts[9] += 1; break;
        }

        switch (cardInfo.cardType)
        {
            case Card.Type.Soul: loadedDeckTypes[0] += 1; break;
            case Card.Type.Spell: loadedDeckTypes[1] += 1; break;
        }

        switch (cardInfo.Rarity)
        {
            case Card.Rarity.Common: loadedDeckRarities[0] += 1; break;
            case Card.Rarity.Legendary:
                loadedDeckRarities[1] += 1;
                legendaryCountText.text = "전설 (" + loadedDeckRarities[1].ToString() + "/9)";
                if (loadedDeckRarities[1] == 9)
                {
                    foreach (var cardDisplay in dbm.cardDisplayList)
                    {
                        DisplayInfo targetDisplay = cardDisplay.GetComponent<DisplayInfo>();
                        if (targetDisplay.Rarity == Card.Rarity.Legendary)
                        {
                            targetDisplay.DisplayUnAvailable();
                        }
                    }
                }
                break;
            case Card.Rarity.Mythical:
                loadedDeckRarities[2] += 1;
                mythicalCountText.text = "신화 (" + loadedDeckRarities[2].ToString() + "/3)";
                if (loadedDeckRarities[2] == 3)
                {
                    foreach (var cardDisplay in dbm.cardDisplayList)
                    {
                        DisplayInfo targetDisplay = cardDisplay.GetComponent<DisplayInfo>();
                        if (targetDisplay.Rarity == Card.Rarity.Mythical)
                        {
                            targetDisplay.DisplayUnAvailable();
                        }
                    }
                }
                break;
        }

        cardCountText.text = "카드 : " + loadedDeckCardCount.ToString() + " / " + CARD_LIMIT.ToString();
    }

    public void RemoveCardInfoInDeck(DisplayInfo cardInfo)
    {
        loadedDeckCardCount -= 1;

        switch (cardInfo.Cost)
        {
            case 0: loadedDeckCosts[0] -= 1; break;
            case 1: loadedDeckCosts[1] -= 1; break;
            case 2: loadedDeckCosts[2] -= 1; break;
            case 3: loadedDeckCosts[3] -= 1; break;
            case 4: loadedDeckCosts[4] -= 1; break;
            case 5: loadedDeckCosts[5] -= 1; break;
            case 6: loadedDeckCosts[6] -= 1; break;
            case 7: loadedDeckCosts[7] -= 1; break;
            case 8: loadedDeckCosts[8] -= 1; break;
            default: loadedDeckCosts[9] -= 1; break;
        }

        switch (cardInfo.cardType)
        {
            case Card.Type.Soul: loadedDeckTypes[0] -= 1; break;
            case Card.Type.Spell: loadedDeckTypes[1] -= 1; break;
        }

        switch (cardInfo.Rarity)
        {
            case Card.Rarity.Common: loadedDeckRarities[0] -= 1; break;
            case Card.Rarity.Legendary:
                if (loadedDeckRarities[1] == 9)
                {
                    foreach (var cardDisplay in dbm.cardDisplayList)
                    {
                        DisplayInfo targetDisplay = cardDisplay.GetComponent<DisplayInfo>();
                        if (targetDisplay.Rarity == Card.Rarity.Legendary && targetDisplay.Quantity != 0)
                        {
                            targetDisplay.DisplayAvailable();
                        }
                    }
                }
                loadedDeckRarities[1] -= 1;
                legendaryCountText.text = "전설 (" + loadedDeckRarities[1].ToString() + "/9)";
                break;
            case Card.Rarity.Mythical:
                if (loadedDeckRarities[2] == 3)
                {
                    foreach (var cardDisplay in dbm.cardDisplayList)
                    {
                        DisplayInfo targetDisplay = cardDisplay.GetComponent<DisplayInfo>();
                        if (targetDisplay.Rarity == Card.Rarity.Mythical && targetDisplay.Quantity != 0)
                        {
                            targetDisplay.DisplayAvailable();
                        }
                    }
                }
                loadedDeckRarities[2] -= 1;
                mythicalCountText.text = "신화 (" + loadedDeckRarities[2].ToString() + "/3)";
                break;
        }
        cardCountText.text = "카드 : " + loadedDeckCardCount.ToString() + " / " + CARD_LIMIT.ToString();
    }

    public void CancelCaution()
    {
        cautionPanel.SetActive(false);
    }
}