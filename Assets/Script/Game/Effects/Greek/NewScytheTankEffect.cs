using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewScytheTankEffect : Effect
{
    [SerializeField] private ChessPiece.PieceType additionalBuffPieceType;

    public override void EffectAction(PlayerController player)
    {
        NewScytheTank newScytheTankComponent = gameObject.GetComponent<NewScytheTank>();
    
        if (newScytheTankComponent.InfusedPiece.pieceType == additionalBuffPieceType)
        {
            newScytheTankComponent.InfusedPiece.SetKeyword(Keyword.Type.Shield);
            newScytheTankComponent.InfusedPiece.buff.AddBuffByKeyword(newScytheTankComponent.cardName, Buff.BuffType.Shield);
        }
    }
}
