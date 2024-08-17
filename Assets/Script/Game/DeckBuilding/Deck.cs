using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public class Deck
{
    static readonly int CARD_LIMIT = 30;
    //public int index;
    public string deckname;
    public int card_count = 0;
    public int[] chesspieces = new int[6]{0, 0, 0, 0, 0, 0}; // {Pawn, Knight, Bishop, Rook, Quene, King}
    public int[] Rarities = new int[3]{0, 0, 0}; // {Common, Legendary, Rarity}
    public List<int> cards = new List<int>();
}
