using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OdinEffect : Effect
{
    [SerializeField] private List<GameObject> selectionCardPrefabList;        //궁니르, 드라우프노르, 감반테인

    private List<GameObject> selectionCardInstanceList;
    public override void EffectAction(PlayerController player)
    {
        InstantiateSelectionCard(gameObject.GetComponent<SoulCard>().InfusedPiece);
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
}
