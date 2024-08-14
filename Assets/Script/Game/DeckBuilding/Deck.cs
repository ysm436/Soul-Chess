using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public class Deck
{
    static readonly int CARD_LIMIT = 30;
    //public int index;
    public string deckname;
    public List<int> cards = new List<int>();
}
