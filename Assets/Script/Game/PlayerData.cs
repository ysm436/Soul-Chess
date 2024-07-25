using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[Serializable]
public class PlayerData
{
    public int soulOrbs;
    public int soulEssence;

    public List<Card> deck;
    public List<Card> hand;
    public Action OnGetCard;

    public void GetCard(Card cardInstance)
    {
        hand.Add(cardInstance);
        for (int i = 0; i < hand.Count; i++)
            hand[i].gameObject.GetComponent<SortingGroup>().sortingOrder = i;

        OnGetCard.Invoke();
    }
}
