using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Odin : SoulCard
{
    [SerializeField] private List<GameObject> selectionCardPrefabList;        //�ôϸ�, �������ϸ�, �������� ������ �־�� �� (ī�� ������)

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
        GameBoard.instance.gameData.playerWhite.TryAddCardInHand(selectedCard);
    }

    public override void AddEffect()
    {

    }

    public override void RemoveEffect()
    {

    }
}
