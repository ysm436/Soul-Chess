using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobinHood : SoulCard
{
    protected override int CardID => cardIdDict["로빈 후드"];

    SoulCard targetSoul = null;

    protected override void Awake()
    {
        base.Awake();
    }

    public void EnableEffect(ChessPiece targetPiece)
    {
        if (targetSoul != null)
        {
            targetSoul.Destroy();
            targetSoul = null;
        }

        if (targetPiece.soul != null && targetPiece.pieceColor != InfusedPiece.pieceColor)
        {
            targetSoul = Instantiate(targetPiece.soul);
            targetSoul.owner = owner;
            targetSoul.gameObject.SetActive(false);
            targetPiece.OnKilled += GetTargetSoulCard;
        }
        InfusedPiece.OnEndAttack += DisableEffect;
    }

    public void DisableEffect(ChessPiece targetPiece)
    {
        if (targetSoul != null) targetSoul.Destroy();
        targetSoul = null;
        targetPiece.OnKilled -= GetTargetSoulCard;
    }

    public override void AddEffect()
    {
        //OnKill이 상대 기물이 죽어서 RemoveSoul()된 이후 호출되어 OnKill로는 구현 불가
        InfusedPiece.OnStartAttack += EnableEffect;
    }
    public override void RemoveEffect()
    {
        InfusedPiece.OnStartAttack -= EnableEffect;
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
                    Debug.Log("로빈 후드 : Hand is Full");
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
                    Debug.Log("로빈 후드 : Hand is Full");
                    targetSoul.Destroy();
                    targetSoul = null;
                    return;
                }
            }
            targetSoul = null;
        }
        else Debug.Log("로빈 후드 동작 실패");
    }
}
