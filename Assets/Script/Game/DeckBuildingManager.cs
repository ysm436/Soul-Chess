using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class DeckBuildingManager : MonoBehaviour
{
    private List<GameObject> AllCardList = new List<GameObject>();
    private List<GameObject> DisplayCardList = new List<GameObject>();

    public Transform DynamicDisplay;
    public GameObject display_prefab;

    bool soul = true;
    bool spell = true;

    private void Awake()
    {
        FindAllCard();
        MakeDisplayCard();
    }

    // Greek, Norse, Western 폴더 내 존재하는 모든 카드를 찾습니다.
    private void FindAllCard()
    {
        string[] GUIDs = AssetDatabase.FindAssets("t: prefab", new[] {"Assets/Prefabs/Game/Cards/Greek"});
        
        for (int i = 0; i < GUIDs.Length; i++)
        {
            string guid = GUIDs[i];
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            GameObject asset = AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject)) as GameObject;
            AllCardList.Add(asset);
        }

        GUIDs = AssetDatabase.FindAssets("t: prefab", new[] {"Assets/Prefabs/Game/Cards/Norse"});
        
        for (int i = 0; i < GUIDs.Length; i++)
        {
            string guid = GUIDs[i];
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            GameObject asset = AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject)) as GameObject;
            AllCardList.Add(asset);
        }

        GUIDs = AssetDatabase.FindAssets("t: prefab", new[] {"Assets/Prefabs/Game/Cards/Western"});
        
        for (int i = 0; i < GUIDs.Length; i++)
        {
            string guid = GUIDs[i];
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            GameObject asset = AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject)) as GameObject;
            AllCardList.Add(asset);
        }
    }

    // 필터에 맞게 카드들을 분류합니다.
    public void SpellToggle(bool spell)
    {
        if (spell)
        {
            foreach (var card in DisplayCardList)
            {
                if(!card.activeSelf && card.GetComponent<CardInfoForDeckBuilding>().CardType == "SpellCard")
                {
                    card.SetActive(true);
                }
            }
        }
        else
        {
            foreach (var card in DisplayCardList)
            {
                if(card.activeSelf && card.GetComponent<CardInfoForDeckBuilding>().CardType == "SpellCard")
                {
                    card.SetActive(false);
                }
            }
        }
    }

    public void SoulToggle(bool soul)
    {
        if (soul)
        {
            foreach (var card in DisplayCardList)
            {
                if(!card.activeSelf && card.GetComponent<CardInfoForDeckBuilding>().CardType == "SoulCard")
                {
                    card.SetActive(true);
                }
            }
        }
        else
        {
            foreach (var card in DisplayCardList)
            {
                if(card.activeSelf && card.GetComponent<CardInfoForDeckBuilding>().CardType == "SoulCard")
                {
                    card.SetActive(false);
                }
            }
        }
    }

    public void GreekToggle(bool greek)
    {
        if (greek)
        {
            foreach (var card in DisplayCardList)
            {
                if(!card.activeSelf && card.GetComponent<CardInfoForDeckBuilding>().Reigon == "Greek")
                {
                    card.SetActive(true);
                }
            }
        }
        else
        {
            foreach (var card in DisplayCardList)
            {
                if(card.activeSelf && card.GetComponent<CardInfoForDeckBuilding>().Reigon == "Greek")
                {
                    card.SetActive(false);
                }
            }
        }
    }

    public void WesternToggle(bool western)
    {
        if (western)
        {
            foreach (var card in DisplayCardList)
            {
                if(!card.activeSelf && card.GetComponent<CardInfoForDeckBuilding>().Reigon == "Western")
                {
                    card.SetActive(true);
                }
            }
        }
        else
        {
            foreach (var card in DisplayCardList)
            {
                if(card.activeSelf && card.GetComponent<CardInfoForDeckBuilding>().Reigon == "Western")
                {
                    card.SetActive(false);
                }
            }
        }
    }

    public void NorseToggle(bool norse)
    {
        if (norse)
        {
            foreach (var card in DisplayCardList)
            {
                if(!card.activeSelf && card.GetComponent<CardInfoForDeckBuilding>().Reigon == "Norse")
                {
                    card.SetActive(true);
                }
            }
        }
        else
        {
            foreach (var card in DisplayCardList)
            {
                if(card.activeSelf && card.GetComponent<CardInfoForDeckBuilding>().Reigon == "Norse")
                {
                    card.SetActive(false);
                }
            }
        }
    }
    public void CommonToggle(bool common)
    {
        if (common)
        {
            foreach (var card in DisplayCardList)
            {
                if(!card.activeSelf && card.GetComponent<CardInfoForDeckBuilding>().Rarity == "Common")
                {
                    card.SetActive(true);
                }
            }
        }
        else
        {
            foreach (var card in DisplayCardList)
            {
                if(card.activeSelf && card.GetComponent<CardInfoForDeckBuilding>().Rarity == "Common")
                {
                    card.SetActive(false);
                }
            }
        }
    }
    public void LegendaryToggle(bool legendary)
    {
        if (legendary)
        {
            foreach (var card in DisplayCardList)
            {
                if(!card.activeSelf && card.GetComponent<CardInfoForDeckBuilding>().Rarity == "Legendary")
                {
                    card.SetActive(true);
                }
            }
        }
        else
        {
            foreach (var card in DisplayCardList)
            {
                if(card.activeSelf && card.GetComponent<CardInfoForDeckBuilding>().Rarity == "Legendary")
                {
                    card.SetActive(false);
                }
            }
        }
    }
    public void MythicalToggle(bool mythical)
    {
        if (mythical)
        {
            foreach (var card in DisplayCardList)
            {
                if(!card.activeSelf && card.GetComponent<CardInfoForDeckBuilding>().Rarity == "Mythical")
                {
                    card.SetActive(true);
                }
            }
        }
        else
        {
            foreach (var card in DisplayCardList)
            {
                if(card.activeSelf && card.GetComponent<CardInfoForDeckBuilding>().Rarity == "Mythical")
                {
                    card.SetActive(false);
                }
            }
        }
    }

    // 카드들을 화면에 나타냅니다.
    private void MakeDisplayCard()
    {
        foreach (var card in AllCardList)
        {
            GameObject newDisplay = Instantiate(display_prefab, DynamicDisplay);
            newDisplay.name = card.name + "_display";
            
            TextMeshProUGUI[] texts = newDisplay.GetComponentsInChildren<TextMeshProUGUI>();

            Card cardinfo = card.GetComponent<Card>();
            CardInfoForDeckBuilding DisplayCard = newDisplay.GetComponent<CardInfoForDeckBuilding>();

            DisplayCard.CardName = cardinfo.cardName;
            DisplayCard.Cost = cardinfo.cost;
            DisplayCard.Reigon = cardinfo.reigon.ToString();
            //DisplayCard.CardName = cardinfo.cardName;
            DisplayCard.Rarity = cardinfo.rarity.ToString();

            if (card.GetComponent<SpellCard>())
            {
                DisplayCard.CardType = "SpellCard";
            }
            else
            {
                DisplayCard.CardType = "SoulCard";
            }
            
            texts[0].text = DisplayCard.CardName;
            texts[1].text = DisplayCard.Cost.ToString();
            texts[2].text = cardinfo.description;

            DisplayCardList.Add(newDisplay);
        }
    }

}