using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Infusion : TargetingEffect
{
    public Infusion(ChessPiece.PieceType pieceRestriction)
    {
        targetTypes.Add(new EffectTarget(TargetType.Piece, pieceRestriction, false, true));
    }
    public Action<ChessPiece> infuse;
    public override void EffectAction()
    {
        infuse.Invoke(targets[0] as ChessPiece);
    }
}
