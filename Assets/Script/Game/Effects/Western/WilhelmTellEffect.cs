using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WilhelmTellEffect : TargetingEffect
{
    public int damage;

    public override void EffectAction(PlayerController player)
    {
        Vector2Int displacement = targets[0].coordinate - GetComponent<SoulCard>().InfusedPiece.coordinate;
        Vector2Int targetCoordinate = targets[0].coordinate;
        ChessPiece targetPiece;
        while (GameBoard.instance.gameData.IsValidCoordinate(targetCoordinate))
        {
            targetPiece = GameBoard.instance.gameData.GetPiece(targetCoordinate);
            if (targetPiece != null && targetPiece.soul != null && targetPiece.pieceColor != player.playerColor)
            {
                targetPiece.SpellAttacked(damage);
                return;
            }
            targetCoordinate += displacement;
        }
    }
}
