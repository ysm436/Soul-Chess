using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Odin : SoulCard
{
    [SerializeField] private List<GameObject> selectionCardPrefabList;        //궁니르, 드라우프니르, 감반테인 순으로 있어야 함 (카드 프리팹)

    private List<GameObject> selectionCardInstanceList;

    protected override void Awake()
    {
        base.Awake();
        OnInfuse += InstantiateSelectionCard;
    }

    private void InstantiateSelectionCard(ChessPiece chessPiece)
    {
        selectionCardInstanceList = new();
        foreach (GameObject selectionCard in selectionCardPrefabList)
        {
            GameObject selectionCardInstance = Instantiate(selectionCard);
            CardSelectionDisplay cardSelectionDisplay = selectionCardInstance.AddComponent<CardSelectionDisplay>();
            cardSelectionDisplay.selectionNumber = selectionCardPrefabList.IndexOf(selectionCard);
            cardSelectionDisplay.OnSelected += AddCardToHand;

            cardSelectionDisplay.Initialize();

            selectionCardInstanceList.Add(selectionCardInstance);
        }
        foreach (GameObject selectionCardInstance in selectionCardInstanceList)
        {
            selectionCardInstance.GetComponent<CardSelectionDisplay>().selectionObjectList = selectionCardInstanceList;
            selectionCardInstance.GetComponent<Card>().isInSelection = true;
        }
    }

    private void AddCardToHand(int selectionNumber)
    {
        Card selectedCard = selectionCardInstanceList[selectionNumber].GetComponent<Card>();
        selectedCard.isInSelection = false;
        GameManager.instance.gameData.playerWhite.TryAddCardInHand(selectedCard);
    }
}
