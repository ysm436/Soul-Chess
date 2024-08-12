using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

[Serializable]
public class PlayerData
{
    public int soulOrbs; // 자원 최대치
    public int soulEssence; // 현재 자원량

    public int maxHandCardCount = 8;    //최대 손패
    public int mulliganHandCount = 4;   //첫 손패

    public List<Card> deck;
    public List<Card> hand;
    public Action<Card> OnGetCard;      // Card는 새로 뽑은 카드

    public Vector2 deckPosition;

    // 게임 시작시 호출
    public void Initialize()
    {
        deckPosition = new Vector2(8f, -2.5f);

        foreach (Card card in deck)
        {
            GameBoard.instance.AddCardInDeckObject(card);
        }

        ShuffleDeck();
        Mulligan();
    }

    // 드로우
    public bool DrawCard()
    {
        if (deck.Count <= 0)
        {
            return false;
        }
        else
        {
            Card card = deck[deck.Count - 1];
            deck.RemoveAt(deck.Count - 1);
            card.FlipFront();

            if (IsHandFull())
            {
                DestroyCard(card);
            }
            else
            {
                GetCard(card);
            }
            return true;
        }
    }

    private bool IsHandFull()
    {
        return hand.Count >= maxHandCardCount;
    }

    public void UpdateHandPosition()
    {
        for (int i = 0; i < hand.Count; i++)
        {
            hand[i].transform.position = new Vector3(0.5f * i - 8, -4, -0.1f * i);
        }
    }

    public bool TryAddCardInHand(Card cardInstance)
    {
        if (IsHandFull())
        {
            return false;
        }
        else
        {
            GetCard(cardInstance);
            return true;
        }
    }

    // 핸드에 있는 카드 삭제 (cardInstance: 핸드에서 지정되어야 함, 프리팹 X)
    public bool TryRemoveCardInHand(Card cardInstance)
    {
        int index = hand.IndexOf(cardInstance);

        if (index != -1)
        {
            hand.Remove(cardInstance);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void RemoveHandCards()
    {
        for (int i = hand.Count - 1; i >= 0; i--)
        {
            DestroyCard(hand[i]);
        }
        hand.Clear();
    }

    public void RemoveDeckCards()
    {
        for (int i = deck.Count - 1; i >= 0; i--)
        {
            DestroyCard(deck[i]);
        }
        deck.Clear();
    }

    // 카드 획득시 실행
    public void GetCard(Card cardInstance)
    {
        hand.Add(cardInstance);
        cardInstance.FlipFront();
        cardInstance.GetComponent<SortingGroup>().sortingOrder = hand.Count - 1;

        UpdateHandPosition();

        OnGetCard?.Invoke(cardInstance);
    }

    private void DestroyCard(Card cardInstance)
    {
        cardInstance.Destroy();
    }

    public void ShuffleDeck()
    {
        for (int i = 0; i < deck.Count; i++)
        {
            int j = Random.Range(i, deck.Count);
            Card tmp = deck[i];
            deck[i] = deck[j];
            deck[j] = tmp;
        }
    }

    private void Mulligan()
    {
        for (int i = 0; i < mulliganHandCount; i++)
        {
            DrawCard();
        }

        foreach (Card card in hand)
        {
            //card.isMulligan = true;
        }
    }

    public void ChangeMulligan(Card cardInstance)
    {
        int index = Random.Range(0, deck.Count + 1);
        deck.Insert(index, cardInstance);
        cardInstance.FlipBack();
        cardInstance.transform.position = deckPosition;
        //cardInstance.isMulligan = false;

        TryRemoveCardInHand(cardInstance);

        DrawCard();
    }



    public int spellDamageIncrease = 0;
    public int spellDamageCoefficient = 1;

    public void SpellAttack(ChessPiece targetPiece, int damage)
    {
        targetPiece.SpellAttacked((damage + spellDamageIncrease) * spellDamageCoefficient);
    }
}
