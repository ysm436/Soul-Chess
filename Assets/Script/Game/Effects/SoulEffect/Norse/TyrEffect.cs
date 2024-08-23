using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TyrEffect : TargetingEffect
{
    ChessPiece restraint_target;
    int defense_quantity = 10;

    public override void EffectAction(PlayerController player)
    {
        Tyr tyr_component = gameObject.GetComponent<Tyr>();

        foreach (var target in targets)
        {
            restraint_target = target as ChessPiece;
        }
        restraint_target.SetKeyword(Keyword.Type.Restraint);
        //버프 관련 변경 머지 후 버프 추가
        restraint_target.buff.AddBuffByKeyword(tyr_component.cardName, Buff.BuffType.Restraint);

        tyr_component.AddEffect();
        tyr_component.InfusedPiece.OnSoulRemoved += tyr_component.RemoveEffect;

        tyr_component.InfusedPiece.SetKeyword(Keyword.Type.Taunt);
        tyr_component.InfusedPiece.SetKeyword(Keyword.Type.Defense, defense_quantity);
        //버프 관련 변경 머지 후 버프 추가
        tyr_component.InfusedPiece.buff.AddBuffByKeyword(tyr_component.cardName, Buff.BuffType.Taunt);
        tyr_component.InfusedPiece.buff.AddBuffByValue(tyr_component.cardName, Buff.BuffType.Defense, defense_quantity, true);
    }
}