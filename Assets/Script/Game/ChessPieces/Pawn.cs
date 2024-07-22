using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Pawn : ChessPiece
{
    bool isMoved = false;
    public override List<Vector2Int> GetMovableCoordinates()
    {
        List<Vector2Int> movableCoordinates = new List<Vector2Int>();

        ChessPiece blockingPiece;
        Vector2Int targetCoordinate;

        Vector2Int direction;


        if (pieceColor == PieceColor.White)
            direction = Vector2Int.up;
        else
            direction = Vector2Int.down;

        //현재 좌표에서 이동 가능한 좌표로 targetCoordinate 값 설정
        targetCoordinate = coordinate + direction;

        //첫 이동일 시 한번 더 반복
        for (int i = 0; i < 2; i++)
        {
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
            }

            //첫 이동이 아닐 경우 break
            if (isMoved)
                break;

            targetCoordinate += direction;
        }

        //로컬 함수 
        void CheckAttackable(Vector2Int targetCoordinate)
        {
            // targetCoordinate가 체스판 안인지 체크
            if (_chessData.IsValidCoordinate(targetCoordinate))
            {
                //해당 칸에 위치한 기물의 정보를 받아옴
                blockingPiece = _chessData.GetPiece(targetCoordinate);

                //해당 칸에 위치한 기물이 상대 기물 경우 이동 가능
                if (blockingPiece != null)
                    if (blockingPiece.pieceColor != pieceColor)
                        movableCoordinates.Add(targetCoordinate);
            }
        }

        CheckAttackable(coordinate + direction + Vector2Int.left);
        CheckAttackable(coordinate + direction + Vector2Int.right);

        return movableCoordinates;
    }

    override public void Move(Vector2Int targetCoordinate)
    {
        base.Move(targetCoordinate);
        isMoved = true;
    }
}
