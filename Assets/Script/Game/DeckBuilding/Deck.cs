using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public class Deck
{
    public int index;
    public string deckname;
    public int card_count = 0;
    public int[] costs = new int[8]{0, 0, 0, 0, 0, 0, 0, 0}; // {0, 1, 2, 3, 4, 5, 6, 7+}
    public int[] chesspieces = new int[6]{0, 0, 0, 0, 0, 0}; // {Pawn, Knight, Bishop, Rook, Quene, King}
    public int[] extra_chesspieces = new int[6]{0, 0, 0, 0, 0, 0}; // 다중 기물 카드의 경우 이용되는 chesspiece
    public int[] Rarities = new int[3]{0, 0, 0}; // {Common, Legendary, Rarity}
    public List<int> cards = new List<int>();
}
