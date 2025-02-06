using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UtgardaLokiEffect : TargetingEffect
{
    [SerializeField] private ChessPiece.PieceType additionalBuffPieceType;

    public override void EffectAction(PlayerController player)
    {
        foreach (var target in targets)
        {
            (target as ChessPiece).SetKeyword(Keyword.Type.Silence);
            (target as ChessPiece).buff.AddBuffByKeyword(gameObject.GetComponent<SoulCard>().cardName, Buff.BuffType.Silence);
            
            UtgardaLoki utgardaLokiComponent = gameObject.GetComponent<UtgardaLoki>();

            if (utgardaLokiComponent.InfusedPiece.pieceType == additionalBuffPieceType)
            {
                (target as ChessPiece).SetKeyword(Keyword.Type.Stun);
                (target as ChessPiece).buff.AddBuffByKeyword(gameObject.GetComponent<SoulCard>().cardName, Buff.BuffType.Stun);
            }
        }
    }
}
