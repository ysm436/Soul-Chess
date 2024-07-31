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

    [HideInInspector]
    public ChessPiece InfusedPiece;

    public Action<ChessPiece> OnInfuse; //강림

    override protected void Awake()
    {
        base.Awake();
        soulCardObject = GetComponent<SoulCardObject>();

        soulCardObject.ADText.text = AD.ToString();
        soulCardObject.HPText.text = HP.ToString();
        soulCardObject.PieceRestrictionText.text = pieceRestriction.ToString();

        if (EffectOnCardUsed is Infusion)
        {
            (EffectOnCardUsed as Infusion).infuse += Infuse;
            (EffectOnCardUsed as Infusion).targetTypes[0].targetPieceType = pieceRestriction;
        }
    }


    public void Infuse(ChessPiece targetPiece)
    {
        targetPiece.SetSoul(this);

        OnInfuse?.Invoke(targetPiece);
    }
}
