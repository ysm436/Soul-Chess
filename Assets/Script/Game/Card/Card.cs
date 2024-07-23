using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card : TargetableObject
{
    public string cardName;
    public int cost;
    public Sprite illustration;
    [Multiline]
    public string description;

    //public abstract void Use();
}
