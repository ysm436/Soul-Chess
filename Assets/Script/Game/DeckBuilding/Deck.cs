using System;
using System.Collections.Generic;

[Serializable] public class Deck
{
    public int index;
    public string deckName;
    public int cardCount = 0;
    public int[] costs = new int[10] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0}; // {0, 1, 2, 3, 4, 5, 6, 7+}
    public int[] types = new int[2] {0, 0}; // {Soul, Spell}
    public int[] rarities = new int[3] {0, 0, 0}; // {Common, Legendary, Rarity}
    public List<int> cards = new List<int>();
}
