using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulCard : Card
{
    /// <summary>
    /// presented by sum of ChessPiece.PieceType
    /// </summary>
    public ChessPiece.PieceType pieceRestriction;
    public int AD;
    public int HP;

    public void Infusion(ChessPiece targetPiece)
    {
        targetPiece.soul = this;
    }
}
