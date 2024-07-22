using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : ChessPiece
{

    public override List<Vector2Int> GetMovableCoordinates()
    {
        List<Vector2Int> movableCoordinates = new List<Vector2Int>();

        ChessPiece blockingPiece;
        Vector2Int targetCoordinate;

        //이동방향
        Vector2Int[] directions = {
            Vector2Int.up, // 상단 방향
            Vector2Int.right, // 우측 방향
            Vector2Int.down, // 하단 방향
            Vector2Int.left, // 좌측 방향
        };

        //각 방향으로 한칸씩 이동할 수 있는 칸인지 검사
        foreach (Vector2Int direction in directions)
        {
            //현재 좌표에서 direction 방향으로 한칸 이동한 좌표로 targetCoordinate 값 설정
            targetCoordinate = coordinate + direction;

            // targetCoordinate를 direction 방향으로 한칸씩 이동하며 체스판 안인지 체크
            while (_chessData.IsValidCoordinate(targetCoordinate))
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
                    break;
                }
                //targetCoordinate를 direction 방향으로 한칸씩 이동
                targetCoordinate += direction;
            }
        }

        return movableCoordinates;
    }
}
