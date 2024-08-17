using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SimpleCard : MonoBehaviour, IPointerClickHandler
{
    public int cardindex;
    private int _quantity;
    public int quantity
    {
        get {return _quantity;}
        set
        {
            _quantity = value;
            quantityText.text = value.ToString();
        }
    }
    public TextMeshProUGUI cardNameText;
    public TextMeshProUGUI cost;
    public TextMeshProUGUI quantityText;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        DeckBuildingManager dbm = GetComponentInParent<DeckBuildingManager>();
        int find_index = -1;

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            DeckManager deckmanager = GetComponentInParent<DeckManager>();

            for (int i = dbm.DisplayStorage.childCount; i > 0; i--)
            {
                Transform find_card = dbm.DisplayStorage.GetChild(i - 1);
                if (find_card.GetComponent<DisplayCard>().cardindex == cardindex)
                {
                    find_index = i - 1;
                    break;
                }
            }

            if (find_index != -1) // 디스플레이에 남아있는 카드가 0개일 경우
            {
                Transform find_card = dbm.DisplayStorage.GetChild(find_index);
                dbm.AddDisplayCard(cardindex, 1);

                Transform displayslot = dbm.DynamicDisplay; // 원래 자리로 돌아가도록 -> 여기서는 index 순
                Transform added_display = displayslot.GetChild(displayslot.childCount - 1);
                for (int card_order = 0; card_order < displayslot.childCount - 1; card_order++)
                {
                    if (displayslot.GetChild(card_order).GetComponent<DisplayCard>().cardindex > added_display.GetComponent<DisplayCard>().cardindex)
                    {
                        added_display.SetSiblingIndex(card_order);
                        break;
                    }
                }

                find_card.SetParent(dbm.TrashCan);
                find_card.gameObject.SetActive(false);
                deckmanager.RemoveCardInfoInDeck(added_display.GetComponent<DisplayCard>());
            }
            else // 디스플레이에 카드가 남아있을 경우
            {
                foreach (var display in dbm.DisplayCardList)
                {
                    DisplayCard displaycardinfo = display.GetComponent<DisplayCard>();
                    if (displaycardinfo.cardindex == cardindex)
                    {
                        displaycardinfo.quantity += 1;
                        deckmanager.RemoveCardInfoInDeck(displaycardinfo);
                        break;
                    }
                }
            }

            deckmanager.TempDeck.Remove(gameObject);
            transform.SetParent(dbm.TrashCan);
            gameObject.SetActive(false);
        }
    }
}
