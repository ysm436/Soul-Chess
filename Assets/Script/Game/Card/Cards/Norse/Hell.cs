using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hell : SoulCard
{
    protected override int CardID => cardIdDict["헬"];

    [HideInInspector] public SoulCard targetSoul = null;

    [SerializeField] private ChessPiece.PieceType additionalBuffPieceType;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void AddEffect()
    {
        if (targetSoul != null)
        {
            int multiplier = 1;
            if (InfusedPiece.pieceType == additionalBuffPieceType)
            {
                multiplier = 2;
            }

            InfusedPiece.AD += targetSoul.AD * multiplier; ;
            InfusedPiece.maxHP += targetSoul.HP * multiplier;

            InfusedPiece.buff.AddBuffByValue(cardName, Buff.BuffType.AD, targetSoul.AD * multiplier, true);
            InfusedPiece.buff.AddBuffByValue(cardName, Buff.BuffType.HP, targetSoul.HP * multiplier, true);
            InfusedPiece.OnKilled += GetTargetSoulCard;
            InfusedPiece.OnSoulRemoved += RemoveEffect;
        }
    }
    public override void RemoveEffect()
    {
        for(int i = InfusedPiece.buff.buffList.Count-1; i >= 0; i--)
        {
            Buff.BuffInfo buffInfo = InfusedPiece.buff.buffList[i];
            if (buffInfo.sourceName == cardName)
            {
                if (buffInfo.buffType == Buff.BuffType.AD)
                {
                    InfusedPiece.AD -= buffInfo.value;
                    InfusedPiece.buff.TryRemoveSpecificBuff(cardName, Buff.BuffType.AD);
                }
                else if (buffInfo.buffType == Buff.BuffType.HP)
                {
                    InfusedPiece.maxHP = (InfusedPiece.maxHP-buffInfo.value) > 0 ? InfusedPiece.maxHP-buffInfo.value : 1;
                    InfusedPiece.buff.TryRemoveSpecificBuff(cardName, Buff.BuffType.HP);
                }
            }
        }
        if (targetSoul != null) Destroy(targetSoul);
        targetSoul = null;
    }

    public void GetTargetSoulCard(ChessPiece chessPiece)
    {
        if (targetSoul != null)
        {
            if (InfusedPiece.pieceColor == GameBoard.PlayerColor.White)
            {
                targetSoul.gameObject.SetActive(true);
                if (!GameBoard.instance.gameData.playerWhite.TryAddCardInHand(targetSoul))
                {
                    Debug.Log("헬 : Hand is Full");
                    targetSoul.Destroy();
                    targetSoul = null;
                    return;
                }
            }
            else
            {
                targetSoul.gameObject.SetActive(true);
                if (!GameBoard.instance.gameData.playerBlack.TryAddCardInHand(targetSoul))
                {
                    Debug.Log("헬 : Hand is Full");
                    targetSoul.Destroy();
                    targetSoul = null;
                    return;
                }
            }
            targetSoul = null;
        }
        else
        {
            Debug.Log("헬 유언 실행 불가(card 없음)");
        }
    }
}
