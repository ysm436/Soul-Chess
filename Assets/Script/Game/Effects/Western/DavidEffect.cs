using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DavidEffect : Effect
{
    [SerializeField] private int standardAD = 7;
    private List<ChessPiece> targetList = new List<ChessPiece>();
    private David davidComponent;

    public override void EffectAction(PlayerController player)
    {
        davidComponent = GetComponent<David>();

        foreach (var sq in GameBoard.instance.gameData.boardSquares)
        {
            sq.OnClick = null;
        }

        foreach (var chesspiece in GameBoard.instance.gameData.pieceObjects)
        {
            if (chesspiece.pieceType != ChessPiece.PieceType.King && chesspiece.pieceColor != davidComponent.InfusedPiece.pieceColor && chesspiece.isAlive)
            {
                targetList.Add(chesspiece);
            }
        }

        if (davidComponent.InfusedPiece.pieceType != ChessPiece.PieceType.King)
        {
            foreach (var target in targetList.ToList())
            {
                if (target.AD < standardAD)
                {
                    targetList.Remove(target);
                }
            }
        }

        if (targetList.Count == 0)
        {
            foreach (var sq in GameBoard.instance.gameData.boardSquares)
            {
                sq.isNegativeTargetable = false;
                sq.OnClick = GameBoard.instance.myController.OnClickBoardSquare;
            }
        }
        else
        {
            foreach (var target in targetList)
            {
                BoardSquare targetBoardSquare = GameBoard.instance.GetBoardSquare(target.coordinate);
                targetBoardSquare.isNegativeTargetable = true;
                targetBoardSquare.OnClick = AttackEffect;
            }
        }
    }

    public void AttackEffect(Vector2Int targetCoordinate)
    {
        ChessPiece targetPiece = GameBoard.instance.gameData.GetPiece(targetCoordinate);

        GameBoard.instance.chessBoard.KillByCardEffect(effectPrefab, davidComponent.InfusedPiece, targetPiece);

        foreach (var sq in GameBoard.instance.gameData.boardSquares)
        {
            sq.isNegativeTargetable = false;
            sq.OnClick = GameBoard.instance.myController.OnClickBoardSquare;
        }
    }
}
