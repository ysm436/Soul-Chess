using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI cardName;
    [SerializeField] private TextMeshProUGUI cost;
    [SerializeField] private TextMeshProUGUI type;
    [SerializeField] private TextMeshProUGUI ADText;
    [SerializeField] private TextMeshProUGUI HPText;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private GameObject AD;
    [SerializeField] private GameObject HP;
    [SerializeField] private Image illustration;
    [SerializeField] private GameObject[] cardFrames;

    public void SetCardUI(Card card)
    {
        if (card is SoulCard)
        {
            type.text = "영혼";
            AD.SetActive(true);
            HP.SetActive(true);
            ADText.text = card.GetComponent<SoulCard>().AD.ToString();
            HPText.text = card.GetComponent<SoulCard>().HP.ToString();
        }
        else
        {
            type.text = "마법";
            AD.SetActive(false);
            HP.SetActive(false);
        }

        foreach (var cardFrame in cardFrames)
            cardFrame.SetActive(false);

        cardFrames[(int)card.rarity].SetActive(true);

        cardName.text = card.cardName;
        cost.text = card.cost.ToString();
        description.text = card.description;
        illustration.sprite = card.illustration;
    }
}
