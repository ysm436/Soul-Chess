using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Data.Common;
using System.Linq;
using Unity.VisualScripting;

public class CardFilter : MonoBehaviour
{
    private DeckBuildingManager dbm;
    
    private bool[] typeToggleList = new bool[2] {true, true}; // {soul, spell}
    private bool[] regionToggleList = new bool[3] {true, true, true}; // {greek, western, norse}
    private bool[] rarityToggleList = new bool[3] {true, true, true}; // {common, legendary, mythical}

    private List<DisplayInfo> displayCardInfoList = new List<DisplayInfo>();
    private List<int> cardFilterOnIndexList = new List<int>();
    private List<int> cardFilterOffIndexList = new List<int>();
    public List<int> searchIndexList = new List<int>();

    [SerializeField] TMP_InputField searchInputField;
    private bool searchSignal = false;

    private void Awake()
    {
        searchInputField.onValueChanged.AddListener(SearchCard);
        dbm = GetComponent<DeckBuildingManager>();

        foreach (var displayCard in dbm.cardDisplayList)
        {
            DisplayInfo targetDisplay = displayCard.GetComponent<DisplayInfo>();
            displayCardInfoList.Add(targetDisplay);
            cardFilterOnIndexList.Add(targetDisplay.cardDisplayIndex);
            searchIndexList.Add(targetDisplay.cardDisplayIndex);
        }
    }

    public void SearchCard(string searchtext)
    {
        if (searchtext != "")
        {
            Debug.Log("??");
            searchSignal = true;
            searchIndexList.Clear();
        }
        else
        {
            searchSignal = false;
            DisplayReload();
            return;
        }

        foreach (var displayCardInfo in displayCardInfoList)
        {
            string cardname = displayCardInfo.CardName;
            int cardnamelength = cardname.Length;
            int searchlength = searchtext.Length;

            if (cardnamelength >= searchtext.Length)
            {
                for (int i = 0; i <= cardnamelength - searchlength; i++)
                {
                    if (searchtext.ToLower() == cardname.Substring(i, searchlength).ToLower())
                    {
                        searchIndexList.Add(displayCardInfo.cardDisplayIndex);
                        break;
                    }
                }
            }
        }
        DisplayReload();
    }

    //스펠 카드 토글
    public void SoulToggle(bool soul)
    {
        if (soul)
        {
            typeToggleList[0] = true;
            ForOnToggle();
        }
        else
        {
            typeToggleList[0] = false;
            foreach (var targetDisplayIndex in cardFilterOnIndexList.ToList())
            {
                DisplayInfo targetDisplay = displayCardInfoList[targetDisplayIndex];
                if (targetDisplay.cardType == Card.Type.Soul)
                {
                    cardFilterOnIndexList.Remove(targetDisplayIndex);
                    cardFilterOffIndexList.Add(targetDisplayIndex);
                }
            }
            DisplayReload();
        }
    }

    //스펠 카드 토글
    public void SpellToggle(bool spell)
    {
        if (spell)
        {
            typeToggleList[1] = true;
            ForOnToggle();
        }
        else
        {
            typeToggleList[1] = false;
            foreach (var targetDisplayIndex in cardFilterOnIndexList.ToList())
            {
                DisplayInfo targetDisplay = displayCardInfoList[targetDisplayIndex];
                if (targetDisplay.cardType == Card.Type.Spell)
                {
                    Debug.Log(targetDisplayIndex);
                    cardFilterOffIndexList.Add(targetDisplayIndex);
                    cardFilterOnIndexList.Remove(targetDisplayIndex);
                }
            }
            DisplayReload();
        }
    }

    //그리스 지역 토글
    public void GreekToggle(bool greek)
    {
        if (greek)
        {
            regionToggleList[0] = true;
            ForOnToggle();
        }
        else
        {
            regionToggleList[0] = false;
            foreach (var targetDisplayIndex in cardFilterOnIndexList.ToList())
            {
                DisplayInfo targetDisplay = displayCardInfoList[targetDisplayIndex];
                if (targetDisplay.reigon == Card.Reigon.Greek)
                {
                    cardFilterOnIndexList.Remove(targetDisplayIndex);
                    cardFilterOffIndexList.Add(targetDisplayIndex);
                }
            }
            DisplayReload();
        }
    }

    //서유럽 지역 토글
    public void WesternToggle(bool western)
    {
        if (western)
        {
            regionToggleList[1] = true;
            ForOnToggle();
        }
        else
        {
            regionToggleList[1] = false;
            foreach (var targetDisplayIndex in cardFilterOnIndexList.ToList())
            {
                DisplayInfo targetDisplay = displayCardInfoList[targetDisplayIndex];
                if (targetDisplay.reigon == Card.Reigon.Western)
                {
                    cardFilterOnIndexList.Remove(targetDisplayIndex);
                    cardFilterOffIndexList.Add(targetDisplayIndex);
                }
            }
            DisplayReload();
        }
    }

    //북유럽 지역 토글
    public void NorseToggle(bool norse)
    {
        if (norse)
        {
            regionToggleList[2] = true;
            ForOnToggle();
        }
        else
        {
            regionToggleList[2] = false;
            foreach (var targetDisplayIndex in cardFilterOnIndexList.ToList())
            {
                DisplayInfo targetDisplay = displayCardInfoList[targetDisplayIndex];
                if (targetDisplay.reigon == Card.Reigon.Norse)
                {
                    cardFilterOnIndexList.Remove(targetDisplayIndex);
                    cardFilterOffIndexList.Add(targetDisplayIndex);
                }
            }
            DisplayReload();
        }
    }

    //일반 등급 토글
    public void CommonToggle(bool common)
    {
        if (common)
        {
            rarityToggleList[0] = true;
            ForOnToggle();
        }
        else
        {
            rarityToggleList[0] = false;
            foreach (var targetDisplayIndex in cardFilterOnIndexList.ToList())
            {
                DisplayInfo targetDisplay = displayCardInfoList[targetDisplayIndex];
                if (targetDisplay.Rarity == Card.Rarity.Common)
                {
                    cardFilterOnIndexList.Remove(targetDisplayIndex);
                    cardFilterOffIndexList.Add(targetDisplayIndex);
                }
            }
            DisplayReload();
        }
    }

    //전설 등급 토글
    public void LegendaryToggle(bool legendary)
    {
        if (legendary)
        {
            rarityToggleList[1] = true;
            ForOnToggle();
        }
        else
        {
            rarityToggleList[1] = false;
            foreach (var targetDisplayIndex in cardFilterOnIndexList.ToList())
            {
                DisplayInfo targetDisplay = displayCardInfoList[targetDisplayIndex];
                if (targetDisplay.Rarity == Card.Rarity.Legendary)
                {
                    cardFilterOnIndexList.Remove(targetDisplayIndex);
                    cardFilterOffIndexList.Add(targetDisplayIndex);
                }
            }
            DisplayReload();
        }
    }

    //신화 등급 토글
    public void MythicalToggle(bool mythical)
    {
        if (mythical)
        {
            rarityToggleList[2] = true;
            ForOnToggle();
        }
        else
        {
            rarityToggleList[2] = false;
            foreach (var targetDisplayIndex in cardFilterOnIndexList.ToList())
            {
                DisplayInfo targetDisplay = displayCardInfoList[targetDisplayIndex];
                if (targetDisplay.Rarity == Card.Rarity.Mythical)
                {
                    cardFilterOnIndexList.Remove(targetDisplayIndex);
                    cardFilterOffIndexList.Add(targetDisplayIndex);
                }
            }
            DisplayReload();
        }
    }

    private void ForOnToggle()
    {
        foreach (var targetDisplayIndex in cardFilterOffIndexList.ToList())
        {
            DisplayInfo cardinfo = displayCardInfoList[targetDisplayIndex];

            if (cardinfo.cardType == Card.Type.Soul)
            {
                if (!typeToggleList[0]) continue; 
            }
            else
            {
                if (!typeToggleList[1]) continue;
            }

            if (cardinfo.reigon == Card.Reigon.Greek)
            {
                if (!regionToggleList[0]) continue;
            }
            else if (cardinfo.reigon == Card.Reigon.Western)
            {
                if (!regionToggleList[1]) continue;
            }
            else
            {
                if (!regionToggleList[2]) continue;
            }

            if (cardinfo.Rarity == Card.Rarity.Common)
            {
                if (!rarityToggleList[0]) continue;
            }
            else if (cardinfo.Rarity == Card.Rarity.Legendary)
            {
                if (!rarityToggleList[1]) continue;
            }
            else
            {
                if (!rarityToggleList[2]) continue;
            }

            cardFilterOnIndexList.Add(targetDisplayIndex);
            cardFilterOffIndexList.Remove(targetDisplayIndex);
        }
        DisplayReload();
    }

    private void DisplayReload()
    {
        if (searchSignal)
        {
            foreach (var cardDisplay in dbm.cardDisplayList)
                cardDisplay.SetActive(false);

            foreach (var cardDisplayIndex in cardFilterOnIndexList.Intersect(searchIndexList))
                displayCardInfoList[cardDisplayIndex].gameObject.SetActive(true);
        }
        else
        {
            foreach (var cardDisplayIndex in cardFilterOnIndexList)
                displayCardInfoList[cardDisplayIndex].gameObject.SetActive(true);
        }
        
        foreach (var cardDisplayIndex in cardFilterOffIndexList)
            displayCardInfoList[cardDisplayIndex].gameObject.SetActive(false);
    }
}
