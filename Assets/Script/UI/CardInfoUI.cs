using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardInfoUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI cardNameText;
    [SerializeField] private GameObject[] costImages;
    [SerializeField] private TextMeshProUGUI cardCostText;

    public void SetCardInfoUI(Card card)
    {
        cardNameText.text = card.cardName;
        cardCostText.text = card.cost.ToString();

        if (card is SoulCard)
        {
            costImages[0].SetActive(true);
            costImages[1].SetActive(false);
            cardCostText.color = Color.black;
        }
        else
        {
            costImages[0].SetActive(false);
            costImages[1].SetActive(true);
            cardCostText.color = Color.white;
        }
    }
}
