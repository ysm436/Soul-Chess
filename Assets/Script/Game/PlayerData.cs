using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[Serializable]
public class PlayerData
{
    public int soulOrbs; // 자원 최대치
    public int soulEssence; // 현재 자원량

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

    public int spellDamageIncrease = 0;
    public int spellDamageCoefficient = 1;

    public void SpellAttack(ChessPiece targetPiece, int damage)
    {
        targetPiece.SpellAttacked((damage + spellDamageIncrease) * spellDamageCoefficient);
    }
}
