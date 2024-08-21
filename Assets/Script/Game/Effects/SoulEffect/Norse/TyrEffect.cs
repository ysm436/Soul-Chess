using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TyrEffect : TargetingEffect
{
    ChessPiece restraint_target;

    public override void EffectAction(PlayerController player)
    {
        Tyr tyr_component = gameObject.GetComponent<Tyr>();

        foreach (var target in targets)
        {
            restraint_target = target as ChessPiece;
        }
        restraint_target.SetKeyword(Keyword.Type.Restraint);
        //버프 관련 변경 머지 후 버프 추가

        tyr_component.AddEffect();
        tyr_component.InfusedPiece.OnKilled += restraint_remove;
        tyr_component.InfusedPiece.OnSoulRemoved += tyr_component.RemoveEffect;
    }

    public void restraint_remove(ChessPiece chessPiece)
    {
        restraint_target.Unrestraint();
        //버프 관련 변경 머지 후 버프 추가
    }

}