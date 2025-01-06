using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TyrEffect : TargetingEffect
{
    public override void EffectAction(PlayerController player)
    {
        Tyr tyrComponent = gameObject.GetComponent<Tyr>();

        foreach (var target in targets)
        {
            tyrComponent.restraint_target = target as ChessPiece;
        }
        tyrComponent.restraint_target.SetKeyword(Keyword.Type.Restraint);
        //버프 관련 변경 머지 후 버프 추가
        tyrComponent.restraint_target.buff.AddBuffByKeyword(tyrComponent.cardName, Buff.BuffType.Restraint);

        tyrComponent.AddEffect();
        tyrComponent.InfusedPiece.OnSoulRemoved += tyrComponent.RemoveEffect;

        tyrComponent.InfusedPiece.SetKeyword(Keyword.Type.Taunt);
        tyrComponent.InfusedPiece.SetKeyword(Keyword.Type.Defense, tyrComponent.defenseQuantity);
        //버프 관련 변경 머지 후 버프 추가
        tyrComponent.InfusedPiece.buff.AddBuffByKeyword(tyrComponent.cardName, Buff.BuffType.Taunt);
        tyrComponent.InfusedPiece.buff.AddBuffByValue(tyrComponent.cardName, Buff.BuffType.Defense, tyrComponent.defenseQuantity, true);
    }
}