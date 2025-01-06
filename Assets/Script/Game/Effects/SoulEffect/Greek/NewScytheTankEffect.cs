using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewScytheTankEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        NewScytheTank newScytheTankComponent = gameObject.GetComponent<NewScytheTank>();

        newScytheTankComponent.InfusedPiece.SetKeyword(Keyword.Type.Defense, newScytheTankComponent.defenseAmount);
        newScytheTankComponent.InfusedPiece.SetKeyword(Keyword.Type.Rush);

        newScytheTankComponent.InfusedPiece.buff.AddBuffByValue(newScytheTankComponent.cardName, Buff.BuffType.Defense, newScytheTankComponent.defenseAmount, true);
        newScytheTankComponent.InfusedPiece.buff.AddBuffByKeyword(newScytheTankComponent.cardName, Buff.BuffType.Rush);
    }
}
