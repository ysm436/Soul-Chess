using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class ThunderEffect : TargetingEffect
{
    [SerializeField] private int thunderDamage = 3;
    private void Awake()
    {
        if (GameBoard.instance.playerColor == GameBoard.PlayerColor.White)
        {
            targetTypes[0].tileCoordinate = new Vector2Int(0, 7);
        }
        else
        {
            targetTypes[0].tileCoordinate = new Vector2Int(7, 0);
        }
    }
    public override void EffectAction(PlayerController player)
    {
        ChessPiece targetPiece;
        List<ChessPiece> targetList = new List<ChessPiece>();

        foreach (var sq in GameBoard.instance.gameData.boardSquares)
        {
            if (sq.coordinate.x == targets[0].coordinate.x)
            {
                targetPiece = GameBoard.instance.gameData.GetPiece(sq.coordinate);
                if (targetPiece != null)
                    if (targetPiece.soul != null && targetPiece.pieceColor != player.playerColor)
                        targetList.Add(targetPiece);
            }
        }

        if (targetList.Count != 0)
        {
            GameBoard.instance.chessBoard.DamageByThunderEffect(effectPrefab, targetList, thunderDamage);
        }
        else
        {
            Debug.Log("Thunder: No Target");
        }
    }
}