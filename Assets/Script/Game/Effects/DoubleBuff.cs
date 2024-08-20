using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiBuff : TargetingEffect
{
    public int buffHP;
    public int buffAD;
    public override void EffectAction(PlayerController player)
    {
        foreach (var t in targets)
        {
            (t as ChessPiece).maxHP += buffHP;
            (t as ChessPiece).AD += buffAD;
        }
    }
}
