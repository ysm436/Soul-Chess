using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardFilter : MonoBehaviour
{
    private bool Soul = true;
    private bool Spell = true;
    private bool Greek = true;
    private bool Western = true;
    private bool Norse = true;
    private bool Common = true;
    private bool Legendary = true;
    private bool Mythical = true;

    // 소울 카드 토글
    public void SoulToggle(bool soul)
    {
        List<GameObject> DisplayCardList = GetComponent<DeckBuildingManager>().DisplayCardList;

        if (soul)
        {
            Soul = true;
            ForOnToggle();
        }
        else
        {
            Soul = false;
            foreach (var card in DisplayCardList)
            {
                if (card.activeSelf && card.GetComponent<DisplayCard>().CardType == Card.Type.Soul)
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
            Spell = true;
            ForOnToggle();
        }
        else
        {
            Spell = false;
            foreach (var card in DisplayCardList)
            {
                if (card.activeSelf && card.GetComponent<DisplayCard>().CardType == Card.Type.Spell)
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
            Greek = true;
            ForOnToggle();
        }
        else
        {
            Greek = false;
            foreach (var card in DisplayCardList)
            {
                if (card.activeSelf && card.GetComponent<DisplayCard>().Reigon == "Greek")
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
            Western = true;
            ForOnToggle();
        }
        else
        {
            Western = false;
            foreach (var card in DisplayCardList)
            {
                if (card.activeSelf && card.GetComponent<DisplayCard>().Reigon == "Western")
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
            Norse = true;
            ForOnToggle();
        }
        else
        {
            Norse = false;
            foreach (var card in DisplayCardList)
            {
                if (card.activeSelf && card.GetComponent<DisplayCard>().Reigon == "Norse")
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
            Common = true;
            ForOnToggle();
        }
        else
        {
            Common = false;
            foreach (var card in DisplayCardList)
            {
                if (card.activeSelf && card.GetComponent<DisplayCard>().Rarity == "Common")
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
            Legendary = true;
            ForOnToggle();
        }
        else
        {
            Legendary = false;
            foreach (var card in DisplayCardList)
            {
                if (card.activeSelf && card.GetComponent<DisplayCard>().Rarity == "Legendary")
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
            Mythical = true;
            ForOnToggle();
        }
        else
        {
            Mythical = false;
            foreach (var card in DisplayCardList)
            {
                if (card.activeSelf && card.GetComponent<DisplayCard>().Rarity == "Mythical")
                {
                    card.SetActive(false);
                }
            }
        }
    }

    void ForOnToggle()
    {
        List<GameObject> DisplayCardList = GetComponent<DeckBuildingManager>().DisplayCardList;
        
        foreach (var card in DisplayCardList)
        {
            DisplayCard cardinfo = card.GetComponent<DisplayCard>();

            if (!card.activeSelf)
            {   
                if (cardinfo.CardType == Card.Type.Soul)
                {
                    if (!Soul) continue; 
                }
                else
                {
                    if (!Spell) continue;
                }

                if (cardinfo.Reigon == "Greek")
                {
                    if (!Greek) continue;
                }
                else if (cardinfo.Reigon == "Western")
                {
                    if (!Western) continue;
                }
                else
                {
                    if (!Norse) continue;
                }

                if (cardinfo.Rarity == "Common")
                {
                    if (!Common) continue;
                }
                else if (cardinfo.Rarity == "Legendary")
                {
                    if (!Legendary) continue;
                }
                else
                {
                    if (!Mythical) continue;
                }

                card.SetActive(true);
            }
        }
    }
}