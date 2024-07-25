using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
public class SoulCard : Card
{
    SoulCardObject soulCardObject;
    /// <summary>
    /// presented by sum of ChessPiece.PieceType
    /// </summary>
    /// 
    [Header("SoulData")]
    public ChessPiece.PieceType pieceRestriction;
    public int AD;
    public int HP;
    override protected void Awake()
    {
        base.Awake();
        soulCardObject = GetComponent<SoulCardObject>();

        soulCardObject.ADText.text = AD.ToString();
        soulCardObject.HPText.text = HP.ToString();
        soulCardObject.PieceRestrictionText.text = pieceRestriction.ToString();

        if (effect is Infusion)
        {
            (effect as Infusion).infuse += Infuse;
            (effect as Infusion).targetTypes[0].targetPieceType = pieceRestriction;
        }
    }


    public void Infuse(ChessPiece targetPiece)
    {
        targetPiece.SetSoul(this);
        //targetPiece
    }
}
