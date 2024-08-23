using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewScytheTankEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        NewScytheTank soul = gameObject.GetComponent<NewScytheTank>();

        soul.InfusedPiece.SetKeyword(Keyword.Type.Defense, 10);
        soul.InfusedPiece.SetKeyword(Keyword.Type.Rush);

        soul.InfusedPiece.buff.AddBuffByValue(soul.cardName, Buff.BuffType.Defense, 10, true);
        soul.InfusedPiece.buff.AddBuffByKeyword(soul.cardName, Buff.BuffType.Rush);
    }
}
