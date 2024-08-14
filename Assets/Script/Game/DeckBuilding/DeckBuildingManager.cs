using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class DeckBuildingManager : MonoBehaviour
{
    public List<GameObject> AllCardList = new List<GameObject>();
    public List<GameObject> DisplayCardList = new List<GameObject>();

    [SerializeField] private int max_card_index;
    [SerializeField] private int quantity_setting;

    public Transform DynamicDisplay;
    public Transform DisplayStorage; // 디스플레이 카드가 덱에 들어가 남아있는 카드의 개수가 0개가 되었을 때 저장되는 창고
    public Transform TrashCan; // 더 이상 쓰지 않는 object를 넣어두는 쓰레기통
    public GameObject display_prefab;

    private void Awake()
    {
        FindAllCard();
        MakeDisplayCard();
    }

    // Greek, Norse, Western 폴더 내 존재하는 모든 카드를 찾습니다.
    private void FindAllCard()
    {
        while (AllCardList.Count <= max_card_index)
        {
            AllCardList.Add(null);
        }

        string[] GUIDs = AssetDatabase.FindAssets("t: prefab", new[] { "Assets/Prefabs/Game/Cards/Greek" });

        for (int i = 0; i < GUIDs.Length; i++)
        {
            string guid = GUIDs[i];
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            GameObject asset = AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject)) as GameObject;
            AllCardList[Card.cardIdDict[asset.GetComponent<Card>().cardName]] = asset;
        }

        GUIDs = AssetDatabase.FindAssets("t: prefab", new[] { "Assets/Prefabs/Game/Cards/Norse" });

        for (int i = 0; i < GUIDs.Length; i++)
        {
            string guid = GUIDs[i];
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            GameObject asset = AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject)) as GameObject;
            AllCardList[Card.cardIdDict[asset.GetComponent<Card>().cardName]] = asset;
        }

        GUIDs = AssetDatabase.FindAssets("t: prefab", new[] { "Assets/Prefabs/Game/Cards/Western" });

        for (int i = 0; i < GUIDs.Length; i++)
        {
            string guid = GUIDs[i];
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            GameObject asset = AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject)) as GameObject;
            AllCardList[Card.cardIdDict[asset.GetComponent<Card>().cardName]] = asset;
        }
    }

    // 카드들을 화면에 나타냅니다.
    private void MakeDisplayCard()
    {
        for (int i = 0; i < AllCardList.Count; i++)
        {
            if (AllCardList[i])
            {
                AddDisplayCard(i, quantity_setting);
            }
        }
    }

    //디스플레이 object를 생성합니다.
    public void AddDisplayCard(int card_index, int quantity)
    {
        GameObject card = AllCardList[card_index];

        GameObject newDisplay = Instantiate(display_prefab, DynamicDisplay);
        newDisplay.name = card.name + "_display";

        //TextMeshProUGUI[] texts = newDisplay.GetComponentsInChildren<TextMeshProUGUI>();

        Card cardinfo = card.GetComponent<Card>();
        DisplayCard DisplayCard = newDisplay.GetComponent<DisplayCard>();

        DisplayCard.cardindex = card_index;
        DisplayCard.CardName = cardinfo.cardName;
        DisplayCard.Cost = cardinfo.cost;
        DisplayCard.Description = cardinfo.description;
        DisplayCard.Reigon = cardinfo.reigon.ToString();
        DisplayCard.Rarity = cardinfo.rarity.ToString();
        DisplayCard.quantity = quantity;

        if (cardinfo is SoulCard)
        {
            DisplayCard.HP = (cardinfo as SoulCard).HP;
            DisplayCard.AD = (cardinfo as SoulCard).AD;
        }

        if (card.GetComponent<SpellCard>())
        {
            DisplayCard.CardType = Card.Type.Spell;
        }
        else
        {
            DisplayCard.CardType = Card.Type.Soul;
        }

        //texts[0].text = DisplayCard.CardName;
        //texts[1].text = DisplayCard.Cost.ToString();
        //texts[2].text = cardinfo.description;

        DisplayCardList.Add(newDisplay);
    }

    //디스플레이될 카드들을 재설정 합니다.
    public void ReloadDisplayCard()
    {
        foreach (var display_card in DisplayCardList)
        {
            display_card.GetComponent<DisplayCard>().quantity = quantity_setting;
        }

        for (int i = DisplayStorage.childCount; i > 0; i--)
        {
            Transform card = DisplayStorage.GetChild(i - 1);
            AddDisplayCard(card.gameObject.GetComponent<DisplayCard>().cardindex, quantity_setting);
            card.SetParent(TrashCan);
            card.gameObject.SetActive(false);
        }
    }
}