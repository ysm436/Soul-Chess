using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewScytheTankEffect : Effect
{
    [SerializeField] private ChessPiece.PieceType additionalBuffPieceType;
    [SerializeField] private int additionalHp;

    public override void EffectAction(PlayerController player)
    {
        NewScytheTank newScytheTankComponent = gameObject.GetComponent<NewScytheTank>();

        newScytheTankComponent.InfusedPiece.SetKeyword(Keyword.Type.Shield);
        newScytheTankComponent.InfusedPiece.buff.AddBuffByKeyword(newScytheTankComponent.cardName, Buff.BuffType.Shield);
    
        if (newScytheTankComponent.InfusedPiece.pieceType == additionalBuffPieceType)
        {
            newScytheTankComponent.InfusedPiece.maxHP += additionalHp;
            newScytheTankComponent.InfusedPiece.buff.AddBuffByValue(gameObject.GetComponent<SoulCard>().cardName, Buff.BuffType.HP, additionalHp, true);
        }
    }
}
