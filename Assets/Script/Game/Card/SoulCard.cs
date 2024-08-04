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
    public int AD
    {
        set
        {
            if (InfusedPiece != null)
                InfusedPiece.AD += value - _AD;
            _AD = value;
        }
        get { return _AD; }
    }
    [SerializeField]
    private int _AD;

    public int HP
    {
        set
        {
            if (InfusedPiece != null)
                InfusedPiece.maxHP += value - _HP;
            _HP = value;
        }
        get { return _HP; }
    }
    [SerializeField]
    private int _HP;

    //영혼카드 프리팹마다 대응하는 악세서리 프리팹 인스펙터에 지정 필요
    public PieceAccessory accessory;

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
