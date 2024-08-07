using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SimpleCard : MonoBehaviour, IPointerClickHandler
{
    public int cardindex;
    public TextMeshProUGUI cardNameText;
    public TextMeshProUGUI cost;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        DeckBuildingManager dbm = GetComponentInParent<DeckBuildingManager>();
        int find_index = -1;

        if(eventData.button == PointerEventData.InputButton.Right)
        {
            for (int i = dbm.DisplayStorage.childCount; i > 0; i--)
            {
                Transform find_card = dbm.DisplayStorage.GetChild(i - 1);
                if (find_card.GetComponent<DisplayCard>().cardindex == cardindex)
                {
                    find_index = i - 1;
                    break;
                }
            }

            if (find_index != -1) // display에 남아있는 카드가 0개
            {
                Transform find_card = dbm.DisplayStorage.GetChild(find_index);
                dbm.AddDisplayCard(cardindex, 1);
                find_card.SetParent(dbm.TrashCan);
                find_card.gameObject.SetActive(false);
            }
            else // display에 카드가 남아있음
            {
                foreach (var display in dbm.DisplayCardList)
                {
                    DisplayCard displaycardinfo = display.GetComponent<DisplayCard>();
                    if(displaycardinfo.cardindex == cardindex)
                    {
                        displaycardinfo.quantity += 1;
                        break;
                    }
                }
            }

            GetComponentInParent<DeckManager>().TempDeck.Remove(gameObject);
            transform.SetParent(dbm.TrashCan);
            gameObject.SetActive(false);
        }
    }
}
