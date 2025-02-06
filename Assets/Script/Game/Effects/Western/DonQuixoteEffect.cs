using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonQuixoteEffect : Effect
{
    [SerializeField] private ChessPiece.PieceType additionalBuffPieceType;
    [SerializeField] private int additionalAD;

    public override void EffectAction(PlayerController player)
    {
        DonQuixote donQuixoteComponent = gameObject.GetComponent<DonQuixote>();

        donQuixoteComponent.AddEffect();
        donQuixoteComponent.InfusedPiece.OnSoulRemoved += donQuixoteComponent.RemoveEffect;
        
        if (donQuixoteComponent.InfusedPiece.pieceType == additionalBuffPieceType)
        {
            donQuixoteComponent.InfusedPiece.maxHP += additionalAD;
            donQuixoteComponent.InfusedPiece.buff.AddBuffByValue(gameObject.GetComponent<SoulCard>().cardName, Buff.BuffType.AD, additionalAD, true);
        }
    }
}
