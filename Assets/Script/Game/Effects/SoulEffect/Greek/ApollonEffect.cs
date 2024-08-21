using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApollonEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        Apollon apollon_component = gameObject.GetComponent<Apollon>();
        Vector2Int StandardPosition = apollon_component.InfusedPiece.coordinate;
        List<Vector2Int> row = new List<Vector2Int>();

        Vector2Int TempPosition = StandardPosition;
        TempPosition += Vector2Int.left;
        while (GameBoard.instance.gameData.IsValidCoordinate(TempPosition))
        {
            row.Add(TempPosition);
            TempPosition += Vector2Int.left;
        }

        TempPosition = StandardPosition;
        TempPosition += Vector2Int.right;
        while (GameBoard.instance.gameData.IsValidCoordinate(TempPosition))
        {
            row.Add(TempPosition);
            TempPosition += Vector2Int.right;
        }

        foreach (var position in row) //열의 모든 내 기물에게 보호막
        {
            ChessPiece obj = GameBoard.instance.gameData.GetPiece(position);
            if (obj != null && obj.pieceColor == player.playerColor)
            {
                obj.SetKeyword(Keyword.Type.Shield);
                //버프 관련 변경 머지 후 버프 추가
            }
        }
        apollon_component.AddEffect(); //자신에게 보호막
    }
}
