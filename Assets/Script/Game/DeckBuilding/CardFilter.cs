using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardFilter : MonoBehaviour
{
    private bool[] typeToggleList = new bool[2] {true, true}; // {soul, spell}
    private bool[] regionToggleList = new bool[3] {true, true, true}; // {greek, western, norse}
    private bool[] rarityToggleList = new bool[3] {true, true, true}; // {common, legendary, mythical}
    [SerializeField] TMP_InputField searchInputField;
    [SerializeField] GameObject preventPanel;    
    private bool searchSignal = false;

    private void Awake()
    {
        searchInputField.onValueChanged.AddListener(SearchCard);
    }

    public void SearchCard(string searchtext)
    {
        List<GameObject> DisplayCardList = GetComponent<DeckBuildingManager>().cardDisplayList;

        if (searchtext != "" && !searchSignal)
        {
            searchSignal = true;
            preventPanel.SetActive(true);
        }

        foreach (var card in DisplayCardList)
        {
            string cardname = card.GetComponent<DisplayInfo>().CardName;
            int cardnamelength = cardname.Length;
            int searchlength = searchtext.Length;

            if (cardnamelength >= searchtext.Length)
            {
                for (int i = 0; i <= cardnamelength - searchlength; i++)
                {
                    if (searchtext.ToLower() == cardname.Substring(i, searchlength).ToLower())
                    {
                        card.SetActive(true);
                        break;
                    }
                    else
                    {
                        card.SetActive(false);
                    }
                }
            }
        }

        if (searchtext == "" && searchSignal)
        {
            searchSignal = false;
            foreach (var card in DisplayCardList)
            {
                card.SetActive(false);
            }

            preventPanel.SetActive(false);
            ForOnToggle();
        }
    }

    //스펠 카드 토글
    public void SoulToggle(bool soul)
    {
        List<GameObject> DisplayCardList = GetComponent<DeckBuildingManager>().cardDisplayList;

        if (soul)
        {
            typeToggleList[0] = true;
            ForOnToggle();
        }
        else
        {
            typeToggleList[0] = false;
            foreach (var card in DisplayCardList)
            {
                if (!searchSignal && card.activeSelf && card.GetComponent<DisplayInfo>().cardType == Card.Type.Soul)
                {
                    card.SetActive(false);
                }
            }
        }
    }
    //스펠 카드 토글
    public void SpellToggle(bool spell)
    {
        List<GameObject> DisplayCardList = GetComponent<DeckBuildingManager>().cardDisplayList;

        if (spell)
        {
            typeToggleList[1] = true;
            ForOnToggle();
        }
        else
        {
            typeToggleList[1] = false;
            foreach (var card in DisplayCardList)
            {
                if (!searchSignal && card.activeSelf && card.GetComponent<DisplayInfo>().cardType == Card.Type.Spell)
                {
                    card.SetActive(false);
                }
            }
        }
    }

    //그리스 지역 토글
    public void GreekToggle(bool western)
    {
        List<GameObject> DisplayCardList = GetComponent<DeckBuildingManager>().cardDisplayList;

        if (western)
        {
            regionToggleList[0] = true;
            ForOnToggle();
        }
        else
        {
            regionToggleList[0] = false;
            foreach (var card in DisplayCardList)
            {
                if (!searchSignal && card.activeSelf && card.GetComponent<DisplayInfo>().reigon == Card.Reigon.Greek)
                {
                    card.SetActive(false);
                }
            }
        }
    }

    //서유럽 지역 토글
    public void WesternToggle(bool western)
    {
        List<GameObject> DisplayCardList = GetComponent<DeckBuildingManager>().cardDisplayList;

        if (western)
        {
            regionToggleList[1] = true;
            ForOnToggle();
        }
        else
        {
            regionToggleList[1] = false;
            foreach (var card in DisplayCardList)
            {
                if (!searchSignal && card.activeSelf && card.GetComponent<DisplayInfo>().reigon == Card.Reigon.Western)
                {
                    card.SetActive(false);
                }
            }
        }
    }

    //북유럽 지역 토글
    public void NorseToggle(bool norse)
    {
        List<GameObject> DisplayCardList = GetComponent<DeckBuildingManager>().cardDisplayList;

        if (norse)
        {
            regionToggleList[2] = true;
            ForOnToggle();
        }
        else
        {
            regionToggleList[2] = false;
            foreach (var card in DisplayCardList)
            {
                if (!searchSignal && card.activeSelf && card.GetComponent<DisplayInfo>().reigon == Card.Reigon.Norse)
                {
                    card.SetActive(false);
                }
            }
        }
    }

    //일반 등급 토글
    public void CommonToggle(bool common)
    {
        List<GameObject> DisplayCardList = GetComponent<DeckBuildingManager>().cardDisplayList;

        if (common)
        {
            rarityToggleList[0] = true;
            ForOnToggle();
        }
        else
        {
            rarityToggleList[0] = false;
            foreach (var card in DisplayCardList)
            {
                if (!searchSignal && card.activeSelf && card.GetComponent<DisplayInfo>().Rarity == Card.Rarity.Common)
                {
                    card.SetActive(false);
                }
            }
        }
    }

    //전설 등급 토글
    public void LegendaryToggle(bool legendary)
    {
        List<GameObject> DisplayCardList = GetComponent<DeckBuildingManager>().cardDisplayList;

        if (legendary)
        {
            rarityToggleList[1] = true;
            ForOnToggle();
        }
        else
        {
            rarityToggleList[1] = false;
            foreach (var card in DisplayCardList)
            {
                if (!searchSignal && card.activeSelf && card.GetComponent<DisplayInfo>().Rarity == Card.Rarity.Legendary)
                {
                    card.SetActive(false);
                }
            }
        }
    }

    //신화 등급 토글
    public void MythicalToggle(bool mythical)
    {
        List<GameObject> DisplayCardList = GetComponent<DeckBuildingManager>().cardDisplayList;

        if (mythical)
        {
            rarityToggleList[2] = true;
            ForOnToggle();
        }
        else
        {
            rarityToggleList[2] = false;
            foreach (var card in DisplayCardList)
            {
                if (!searchSignal && card.activeSelf && card.GetComponent<DisplayInfo>().Rarity == Card.Rarity.Mythical)
                {
                    card.SetActive(false);
                }
            }
        }
    }

    private void ForOnToggle()
    {
        List<GameObject> DisplayCardList = GetComponent<DeckBuildingManager>().cardDisplayList;
        
        foreach (var card in DisplayCardList)
        {
            DisplayInfo cardinfo = card.GetComponent<DisplayInfo>();
            bool onflag = false;

            if (!card.activeSelf && !searchSignal)
            {   
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
 
                if (onflag || cardinfo.cardType == Card.Type.Spell)
                {
                    card.SetActive(true);
                }
            }
        }
    }
}
