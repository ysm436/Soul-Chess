using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Infusion : Effect
{
    public Action<ChessPiece> infuse;
    public override void EffectAction()
    {
        infuse.Invoke(targets[0] as ChessPiece);
    }
}