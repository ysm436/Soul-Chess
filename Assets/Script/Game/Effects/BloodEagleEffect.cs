using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodEagleEffect : TargetingEffect
{
    public override void EffectAction()
    {
        foreach (var target in targets)
        {
            (target as ChessPiece).Kill();
        }

        GameBoard.instance.myController.Draw();
    }
}
