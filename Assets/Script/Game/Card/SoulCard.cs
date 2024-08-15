using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System.Linq;
using Unity.Mathematics;
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
        
        //기물 제한 아이콘 생성 후 위치 정렬
        List<SoulCardPieceRestriction> icons = new List<SoulCardPieceRestriction>();
        foreach(ChessPiece.PieceType value in Enum.GetValues(typeof(ChessPiece.PieceType)))
        {
            if (pieceRestriction.HasFlag(value) && (value != ChessPiece.PieceType.None))
            {
                SoulCardPieceRestriction temp = Instantiate(soulCardObject.PieceRestrictionIcon, transform.position, Quaternion.identity);
                temp.gameObject.SetActive(true);
                temp.transform.SetParent(transform);
                temp.transform.localScale *= 0.8f;
                temp.SetIconSprite(value);
                icons.Add(temp);
            }
        }
        if (icons.Count > 0)
        {
            if (icons.Count % 2 == 0) //아이콘이 짝수 개
            {
                int temp = icons.Count / 2;
                for (int i = temp - 1; i >= 0; i--)
                {
                    icons[i].transform.localPosition = new Vector3(-0.08f - i*0.16f, -1.175f, 0);
                    icons[icons.Count-1 - i].transform.localPosition = new Vector3(0.08f + i*0.16f, -1.175f, 0);
                }
            }
            else //아이콘이 홀수 개
            {
                int temp = icons.Count / 2;
                icons[temp].transform.localPosition = new Vector3(0,  -1.175f, 0);
                for (int i = temp - 1; i >= 0; i--)
                {
                    icons[i].transform.localPosition = new Vector3(-0.16f*i, -1.175f, 0);
                    icons[icons.Count-1 - i].transform.localPosition = new Vector3(i*0.16f, -1.175f, 0);
                }
            }
        }

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

}
