using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardFilter : MonoBehaviour
{
    // 필터에 맞게 카드들을 분류합니다.

    // 소울 카드 토글
    public void SoulToggle(bool soul)
    {
        List<GameObject> DisplayCardList = GetComponent<DeckBuildingManager>().DisplayCardList;

        if (soul)
        {
            foreach (var card in DisplayCardList)
            {
                if(!card.activeSelf && card.GetComponent<DisplayCard>().CardType == "SoulCard")
                {
                    card.SetActive(true);
                }
            }
        }
        else
        {
            foreach (var card in DisplayCardList)
            {
                if(card.activeSelf && card.GetComponent<DisplayCard>().CardType == "SoulCard")
                {
                    card.SetActive(false);
                }
            }
        }
    }

    //스펠 카드 토글
    public void SpellToggle(bool spell)
    {
        List<GameObject> DisplayCardList = GetComponent<DeckBuildingManager>().DisplayCardList;

        if (spell)
        {
            foreach (var card in DisplayCardList)
            {
                if(!card.activeSelf && card.GetComponent<DisplayCard>().CardType == "SpellCard")
                {
                    card.SetActive(true);
                }
            }
        }
        else
        {
            foreach (var card in DisplayCardList)
            {
                if(card.activeSelf && card.GetComponent<DisplayCard>().CardType == "SpellCard")
                {
                    card.SetActive(false);
                }
            }
        }
    }

    //그리스 지역 토글
    public void GreekToggle(bool greek)
    {
        List<GameObject> DisplayCardList = GetComponent<DeckBuildingManager>().DisplayCardList;

        if (greek)
        {
            foreach (var card in DisplayCardList)
            {
                if(!card.activeSelf && card.GetComponent<DisplayCard>().Reigon == "Greek")
                {
                    card.SetActive(true);
                }
            }
        }
        else
        {
            foreach (var card in DisplayCardList)
            {
                if(card.activeSelf && card.GetComponent<DisplayCard>().Reigon == "Greek")
                {
                    card.SetActive(false);
                }
            }
        }
    }

    //서유럽 지역 토글
    public void WesternToggle(bool western)
    {
        List<GameObject> DisplayCardList = GetComponent<DeckBuildingManager>().DisplayCardList;

        if (western)
        {
            foreach (var card in DisplayCardList)
            {
                if(!card.activeSelf && card.GetComponent<DisplayCard>().Reigon == "Western")
                {
                    card.SetActive(true);
                }
            }
        }
        else
        {
            foreach (var card in DisplayCardList)
            {
                if(card.activeSelf && card.GetComponent<DisplayCard>().Reigon == "Western")
                {
                    card.SetActive(false);
                }
            }
        }
    }

    //북유럽 지역 토글
    public void NorseToggle(bool norse)
    {
        List<GameObject> DisplayCardList = GetComponent<DeckBuildingManager>().DisplayCardList;

        if (norse)
        {
            foreach (var card in DisplayCardList)
            {
                if(!card.activeSelf && card.GetComponent<DisplayCard>().Reigon == "Norse")
                {
                    card.SetActive(true);
                }
            }
        }
        else
        {
            foreach (var card in DisplayCardList)
            {
                if(card.activeSelf && card.GetComponent<DisplayCard>().Reigon == "Norse")
                {
                    card.SetActive(false);
                }
            }
        }
    }
    
    //일반 등급 토글
    public void CommonToggle(bool common)
    {
        List<GameObject> DisplayCardList = GetComponent<DeckBuildingManager>().DisplayCardList;

        if (common)
        {
            foreach (var card in DisplayCardList)
            {
                if(!card.activeSelf && card.GetComponent<DisplayCard>().Rarity == "Common")
                {
                    card.SetActive(true);
                }
            }
        }
        else
        {
            foreach (var card in DisplayCardList)
            {
                if(card.activeSelf && card.GetComponent<DisplayCard>().Rarity == "Common")
                {
                    card.SetActive(false);
                }
            }
        }
    }

    //전설 등급 토글
    public void LegendaryToggle(bool legendary)
    {
        List<GameObject> DisplayCardList = GetComponent<DeckBuildingManager>().DisplayCardList;

        if (legendary)
        {
            foreach (var card in DisplayCardList)
            {
                if(!card.activeSelf && card.GetComponent<DisplayCard>().Rarity == "Legendary")
                {
                    card.SetActive(true);
                }
            }
        }
        else
        {
            foreach (var card in DisplayCardList)
            {
                if(card.activeSelf && card.GetComponent<DisplayCard>().Rarity == "Legendary")
                {
                    card.SetActive(false);
                }
            }
        }
    }

    //신화 등급 토글
    public void MythicalToggle(bool mythical)
    {
        List<GameObject> DisplayCardList = GetComponent<DeckBuildingManager>().DisplayCardList;

        if (mythical)
        {
            foreach (var card in DisplayCardList)
            {
                if(!card.activeSelf && card.GetComponent<DisplayCard>().Rarity == "Mythical")
                {
                    card.SetActive(true);
                }
            }
        }
        else
        {
            foreach (var card in DisplayCardList)
            {
                if(card.activeSelf && card.GetComponent<DisplayCard>().Rarity == "Mythical")
                {
                    card.SetActive(false);
                }
            }
        }
    }
}
