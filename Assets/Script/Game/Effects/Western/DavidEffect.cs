using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DavidEffect : TargetingEffect
{
    [SerializeField] private int standardAD = 7;
    ChessPiece.PieceType targetPieceRestriction =
        ChessPiece.PieceType.Pawn |
        ChessPiece.PieceType.Knight |
        ChessPiece.PieceType.Bishop |
        ChessPiece.PieceType.Rook |
        ChessPiece.PieceType.Quene |
        ChessPiece.PieceType.King;

    void Awake()
    {
        Predicate<ChessPiece> condition = (ChessPiece piece) => piece.AD >= standardAD;
        EffectTarget effectTarget = new EffectTarget(TargetType.Piece, targetPieceRestriction, true, false, condition);
        targetTypes.Add(effectTarget);
    }

    public override void EffectAction(PlayerController player)
    {
        David davidComponent = GetComponent<David>();

        foreach (var target in targets)
        {
            Debug.Log("David Effect");
            GameBoard.instance.chessBoard.KillByCardEffect(effectPrefab, davidComponent.InfusedPiece, target as ChessPiece);
        }
    }

    /* public override void EffectAction(PlayerController player)
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
    } */
}
