using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : ChessPiece
{
    public override List<Vector2Int> GetMovableCoordinates()
    {
        List<Vector2Int> movableCoordinates = new List<Vector2Int>();

        if (GetKeyword(Keyword.Type.Stun) == 1 || GetKeyword(Keyword.Type.Restraint) == 1 || isSoulSet) return movableCoordinates;

        ChessPiece blockingPiece;
        Vector2Int targetCoordinate;

        //나이트의 현재 위치로부터 이동 가능한 좌표
        Vector2Int[] possibleCoordinates = {
            Vector2Int.up*2 + Vector2Int.left,
            Vector2Int.up*2 + Vector2Int.right,
            Vector2Int.right*2 + Vector2Int.up,
            Vector2Int.right*2 + Vector2Int.down,
            Vector2Int.down*2 + Vector2Int.right,
            Vector2Int.down*2 + Vector2Int.left,
            Vector2Int.left*2 + Vector2Int.down,
            Vector2Int.left*2 + Vector2Int.up
            };

        //각 좌표가 이동할 수 있는 칸인지 검사
        foreach (Vector2Int possibleCoordinate in possibleCoordinates)
        {
            //현재 좌표에서 이동 가능한 좌표로 targetCoordinate 값 설정
            targetCoordinate = coordinate + possibleCoordinate;

            // targetCoordinate가 체스판 안인지 체크
            if (_chessData.IsValidCoordinate(targetCoordinate))
            {
                //해당 칸에 위치한 기물의 정보를 받아옴
                blockingPiece = _chessData.GetPiece(targetCoordinate);

                //해당 칸에 위치한 기물이 없을 경우 이동 가능
                if (blockingPiece == null)
                {
                    movableCoordinates.Add(targetCoordinate);
                }
                else
                {
                    //해당 칸에 적 기물이 있을 경우 이동 가능
                    if (blockingPiece.pieceColor != this.pieceColor)
                    {
                        movableCoordinates.Add(targetCoordinate);
                    }
                }
            }
        }

        return movableCoordinates;
    }
}
