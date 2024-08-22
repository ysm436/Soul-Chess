using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderEffect : Effect
{
    List<BoardSquare> sqList = new List<BoardSquare>(); //행 기준 BoardSquare
    List<BoardSquare> others = new List<BoardSquare>(); //나머지
    GameBoard.PlayerColor playercolor = GameBoard.PlayerColor.White; //EffectAction에 변수로 player 받으면 추가하겠습니다.
    public override void EffectAction(PlayerController player)
    {
        playercolor = player.playerColor; //추가함
        Vector2Int[] row_standards = {
            Vector2Int.zero,
            Vector2Int.up,
            Vector2Int.up * 2,
            Vector2Int.up * 3,
            Vector2Int.up * 4,
            Vector2Int.up * 5,
            Vector2Int.up * 6,
            Vector2Int.up * 7
        };

        //모든 게임 타일의 OnClick 삭제(PlayerController에서 타일 클릭 시 아무것도 안됨)
        foreach (var sq in GameBoard.instance.gameData.boardSquares)
        {
            others.Add(sq);
            sq.OnClick = null;
        };

        foreach (var row_standard in row_standards)
        {
            Vector2Int targetCoordinate = row_standard;
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
        foreach (var sq in GameBoard.instance.gameData.boardSquares)
        {
            sq.isNegativeTargetable = false;
            sq.OnClick = GameBoard.instance.myController.OnClickBoardSquare;
        }

        while (GameBoard.instance.gameData.IsValidCoordinate(targetCoordinate))
        {
            ChessPiece target = GameBoard.instance.gameData.GetPiece(targetCoordinate);
            if (target.soul != null && target.pieceColor != playercolor) //영혼만 공격
            {
                target.SpellAttacked(35);
            }
            targetCoordinate += Vector2Int.right;
        }
    }
}