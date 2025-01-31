using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardInDeck : MonoBehaviour, IPointerClickHandler
{
    public int cardDisplayIndex;
    public int cardIndex;
    private int _quantity;
    public int Quantity
    {
        get {return _quantity;}
        set
        {
            _quantity = value;
            quantityText.text = value.ToString();
        }
    }
    public TextMeshProUGUI cardNameText;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI quantityText;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        DeckBuildingManager dbm = GetComponentInParent<DeckBuildingManager>();

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            DisplayInfo targetDisplayInfo = dbm.cardDisplayList[cardDisplayIndex].GetComponent<DisplayInfo>();
            targetDisplayInfo.Quantity += 1;
            dbm.deckManager.tempDeck.Remove(cardIndex);
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
