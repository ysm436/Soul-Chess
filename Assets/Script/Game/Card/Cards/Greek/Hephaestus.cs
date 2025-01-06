using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hephaestus : SoulCard
{
    protected override int CardID => cardIdDict["헤파이스토스"];

    public int rangeDamage = 20;

    protected override void Awake()
    {
        base.Awake();
    }

    public void SoulEffect(Vector2Int coordinate)
    {
        List<Vector2Int> aroundCoordinates = new()
        {
            coordinate + Vector2Int.up,
            coordinate + Vector2Int.up + Vector2Int.right,
            coordinate + Vector2Int.up + Vector2Int.left,
            coordinate + Vector2Int.down,
            coordinate + Vector2Int.down + Vector2Int.right,
            coordinate + Vector2Int.down + Vector2Int.left,
            coordinate + Vector2Int.right,
            coordinate + Vector2Int.left,
        };

        GameData _chessData = GameBoard.instance.gameData;

        for(int i = aroundCoordinates.Count - 1; i >= 0; i--)
        {
            Vector2Int currentCoordinate = aroundCoordinates[i];

            if (!_chessData.IsValidCoordinate(currentCoordinate)) //잘못된 좌표면 제거
            {
                aroundCoordinates.RemoveAt(i);
                continue;
            }
            if (_chessData.GetPiece(currentCoordinate) != null && _chessData.GetPiece(currentCoordinate) != InfusedPiece &&
                _chessData.GetPiece(currentCoordinate).soul != null) //영혼에만 피해
            {
                _chessData.GetPiece(currentCoordinate).MinusHP(rangeDamage);
            }
        }
    }

    public override void AddEffect()
    {
        InfusedPiece.OnMove += SoulEffect;
        InfusedPiece.OnSoulRemoved += RemoveEffect;
    }
    public override void RemoveEffect()
    {
        InfusedPiece.OnMove -= SoulEffect;
    }
}
