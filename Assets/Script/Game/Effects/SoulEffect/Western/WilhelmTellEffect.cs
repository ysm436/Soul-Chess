using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WilhelmTellEffect : Effect
{
    List<BoardSquare> sqList = new List<BoardSquare>(); //방향 지정용 BoardSquare
    List<BoardSquare> others = new List<BoardSquare>(); //나머지

    public override void EffectAction(PlayerController player)
    {
        Vector2Int coordinate = gameObject.GetComponent<SoulCard>().InfusedPiece.coordinate;
        GameData _chessData = GameBoard.instance.gameData;

        Vector2Int[] directions = {
            Vector2Int.up, // 상단 방향
            Vector2Int.up + Vector2Int.right, // 우상단 방향
            Vector2Int.right, // 우측 방향
            Vector2Int.right + Vector2Int.down, // 우하단 방향
            Vector2Int.down, // 하단 방향
            Vector2Int.down + Vector2Int.left, // 좌하단 방향
            Vector2Int.left, // 좌측 방향
            Vector2Int.left + Vector2Int.up // 좌상단 방향
            };

        //모든 게임 타일의 OnClick 삭제(PlayerController에서 타일 클릭 시 아무것도 안됨)
        foreach (var sq in GameBoard.instance.gameData.boardSquares)
        {
            others.Add(sq);
            sq.OnClick = null;
        };

        foreach (var direction in directions)
        {
            Vector2Int targetCoordinate = coordinate + direction;
            if (!GameBoard.instance.gameData.IsValidCoordinate(targetCoordinate)) continue;
            //방향 지정용 타일의 OnClick 액션에 AttackEffect() 추가
            BoardSquare sq = GameBoard.instance.GetBoardSquare(targetCoordinate);
            sq.isNegativeTargetable = true;
            sq.OnClick = AttackEffect;
            sqList.Add(sq);
            others.Remove(sq); //나머지 타일 리스트에서 제거
        }
    }

    public void AttackEffect(Vector2Int targetCoordinate)
    {
        Vector2Int coordinate = gameObject.GetComponent<SoulCard>().InfusedPiece.coordinate;
        Vector2Int direction = targetCoordinate - coordinate;

        foreach (var sq in GameBoard.instance.gameData.boardSquares)
        {
            sq.isNegativeTargetable = false;
            sq.OnClick = GameBoard.instance.myController.OnClickBoardSquare;
        }

        while(GameBoard.instance.gameData.IsValidCoordinate(targetCoordinate))
        {
            ChessPiece piece = GameBoard.instance.gameData.GetPiece(targetCoordinate);
            if (piece == null || piece.pieceColor == gameObject.GetComponent<SoulCard>().InfusedPiece.pieceColor)
            {
                targetCoordinate += direction;
            }
            else
            {
                piece.MinusHP(60);
                Debug.Log("빌헬름 텔 효과 발동");
                break;
            }
        }
    }
}
