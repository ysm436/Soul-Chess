using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WilhelmTellEffect : TargetingEffect
{
    public int damage;

    public override void EffectAction(PlayerController player)
    {
        ChessPiece infusedPiece = GetComponent<WilhelmTell>().InfusedPiece;
        Vector2Int displacement = targets[0].coordinate - infusedPiece.coordinate;
        Vector2Int targetCoordinate = targets[0].coordinate;
        ChessPiece targetPiece;
        while (GameBoard.instance.gameData.IsValidCoordinate(targetCoordinate))
        {
            targetPiece = GameBoard.instance.gameData.GetPiece(targetCoordinate);
            if (targetPiece != null && targetPiece.soul != null && targetPiece.pieceColor != player.playerColor)
            {
                Debug.Log("WilhelmTell: Soul Effect");
                GameBoard.instance.chessBoard.DamageByCardEffect(effectPrefab, infusedPiece, targetPiece, damage);
                return;
            }
            targetCoordinate += displacement;
        }
    }
}
