using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System.Linq;
public class SoulCard : Card
{
    [Serializable]      // 하나의 PieceType에 하나의 Sprite가 선택되도록 설정되어야 함
    private struct AccessorySprite
    {
        public ChessPiece.PieceType pieceType;
        public GameBoard.PlayerColor pieceColor;
        public Sprite sprite;
    }

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

    [SerializeField]
    private List<AccessorySprite> accessorySpriteList;

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
        Sprite accessorySprite = accessorySpriteList.FirstOrDefault(data => (data.pieceType == targetPiece.pieceType) && (data.pieceColor == targetPiece.pieceColor)).sprite;

        if (accessorySprite == null)
        {
            Debug.Log(cardName + " has no sprite correspond to piece type " + targetPiece.pieceType);
        }

        targetPiece.SetSoul(this, accessorySprite);

        OnInfuse?.Invoke(targetPiece);
    }

    public virtual void AddEffect()
    {

    }

    public virtual void RemoveEffect()
    {

    }
}
