using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class DeckBuildingManager : MonoBehaviour
{
    private List<GameObject> AllCardList = new List<GameObject>();
    public List<GameObject> DisplayCardList = new List<GameObject>();

    public Transform DynamicDisplay;
    public Transform DisplayStorage;
    public Transform TrashCan;
    public GameObject display_prefab;

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

    // 카드들을 화면에 나타냅니다.
    private void MakeDisplayCard()
    {
        int index = 0;
        foreach (var card in AllCardList)
        {
            AddDisplayCard(index);
            index++;
        }
    }

    public void AddDisplayCard(int card_index)
    {
        GameObject card = AllCardList[card_index];

        GameObject newDisplay = Instantiate(display_prefab, DynamicDisplay);
        newDisplay.name = card.name + "_display";
            
        TextMeshProUGUI[] texts = newDisplay.GetComponentsInChildren<TextMeshProUGUI>();

        Card cardinfo = card.GetComponent<Card>();
        DisplayCard DisplayCard = newDisplay.GetComponent<DisplayCard>();

        DisplayCard.cardindex = card_index;
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
    
    public void ReloadDisplayCard()
    {
        foreach (var display_card in DisplayCardList)
        {
            display_card.GetComponent<DisplayCard>().quantity = 2; // 나중에 플레이어 정보랑 연결해야 함
        }

        for(int i = DisplayStorage.childCount; i > 0; i--)
        {
            Transform card = DisplayStorage.GetChild(i - 1);
            card.GetComponent<DisplayCard>().quantity = 2;
            card.SetParent(DynamicDisplay);
            card.gameObject.SetActive(true);
        }
    }
}