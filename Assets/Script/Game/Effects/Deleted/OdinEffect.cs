using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OdinEffect : Effect
{
    [SerializeField] private List<GameObject> selectionCardPrefabList;        //궁니르, 드라우프노르, 감반테인
    private PlayerController Player;

    private List<GameObject> selectionCardInstanceList;
    public override void EffectAction(PlayerController player)
    {
        Player = player;
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
        Destroy(selectionCardInstanceList[selectionNumber]);
        GameObject selectedcard = Instantiate(selectionCardPrefabList[selectionNumber]);
        Card selectedcard_cardcomponent = selectedcard.GetComponent<Card>();
        selectedcard_cardcomponent.isInSelection = false;
        selectedcard_cardcomponent.owner = GetComponent<Card>().owner;

        if (Player.playerColor == GameBoard.PlayerColor.White)
        {
            GameBoard.instance.gameData.playerWhite.TryAddCardInHand(selectedcard_cardcomponent);
        }
        else
        {
            GameBoard.instance.gameData.playerBlack.TryAddCardInHand(selectedcard_cardcomponent);
        }
    }
}
