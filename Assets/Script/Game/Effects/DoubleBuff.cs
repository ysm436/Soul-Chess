using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiBuff : Effect
{
    public int buffHP;
    public int buffAD;
    public override void EffectAction()
    {
        foreach (var t in targets)
        {
            (t as ChessPiece).maxHP += buffHP;
            (t as ChessPiece).AD += buffAD;
        }
    }
}
