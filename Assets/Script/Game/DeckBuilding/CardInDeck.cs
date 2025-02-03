using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardInDeck : MonoBehaviour, IPointerClickHandler
{
    public int cardDisplayIndex;
    public int cardIndex;
    private int _quantity;
    public int Quantity
    {
        get { return _quantity; }
        set
        {
            _quantity = value;
            quantityText.text = "X " + value.ToString();
        }
    }

    public Card.Type cardType
    {
        set
        {
            if (value == Card.Type.Soul)
                cardTypeImage.sprite = typeSpriteList[0];
            else
                cardTypeImage.sprite = typeSpriteList[1];
        }
    }

    public Card.Rarity cardRarity
    {
        set
        {
            if (value == Card.Rarity.Legendary)
                rankGameObjectList[1].SetActive(true);
            else if (value == Card.Rarity.Mythical)
            {
                rankGameObjectList[1].SetActive(true);
                rankGameObjectList[2].SetActive(true);
            }    
        }
    }

    [SerializeField] private List<Sprite> typeSpriteList;
    [SerializeField] private List<GameObject> rankGameObjectList;

    public TextMeshProUGUI cardNameText;
    public TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI quantityText;

    [SerializeField] private Image cardTypeImage;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        DeckBuildingManager dbm = GetComponentInParent<DeckBuildingManager>();

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            DisplayInfo targetDisplayInfo = dbm.cardDisplayList[cardDisplayIndex].GetComponent<DisplayInfo>();
            targetDisplayInfo.Quantity += 1;
            dbm.deckManager.tempDeck.Remove(cardDisplayIndex);
            Quantity -= 1;

            if (Quantity == 0)
            {
                transform.SetParent(dbm.trashCan);
                gameObject.SetActive(false);
            }

            dbm.deckManager.RemoveCardInfoInDeck(targetDisplayInfo);
        }

    }
}
