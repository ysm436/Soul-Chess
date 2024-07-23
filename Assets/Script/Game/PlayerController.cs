using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public ChessPiece.PieceColor playerColor;
    public GameBoard gameBoard;

    ChessPiece chosenPiece = null;
    List<Vector2Int> movableCoordinates = new List<Vector2Int>();
    bool isUsingCard = false;

    private void OnEnable()
    {
        foreach (var s in gameBoard.gameData.boardSquares)
        {
            s.OnClick = OnClickBoardSquare;
        }
    }
    private void OnDisnable()
    {
        foreach (var s in gameBoard.gameData.boardSquares)
        {
            s.OnClick = null;
        }
    }

    public void OnClickBoardSquare(Vector2Int coordinate)
    {
        ChessPiece targetPiece = gameBoard.gameData.GetPiece(coordinate);

        if (isUsingCard)
        {

        }
        else
        {
            if (chosenPiece == null)//선택된 (아군)기물이 없을 때
            {
                if (targetPiece != null)
                {
                    if (IsMyPiece(targetPiece))//고른 기물이 아군일때
                    {
                        SetChosenPiece(targetPiece);
                    }
                }
            }
            else//선택된 (아군)기물이 있을 때
            {
                if (targetPiece != null)
                {
                    if (IsMyPiece(targetPiece))//고른 기물이 아군일때
                    {
                        SetChosenPiece(targetPiece);
                    }
                    else//고른 기물이 적일 때
                    {
                        if (IsMovableCoordniate(coordinate))
                        {
                            if (chosenPiece.Attack(targetPiece))
                            {
                                gameBoard.KillPiece(targetPiece);
                                chosenPiece.Move(coordinate);
                                gameBoard.chessBoard.SetPiecePositionByCoordinate(chosenPiece);
                            }
                            else if (!chosenPiece.isAlive)
                            {
                                gameBoard.KillPiece(chosenPiece);
                            }

                            chosenPiece = null;
                            ClearMovableCoordniates();
                        }
                    }
                }
                else //고른 칸이 빈칸일때
                {
                    if (IsMovableCoordniate(coordinate))
                    {
                        chosenPiece.Move(coordinate);
                        gameBoard.chessBoard.SetPiecePositionByCoordinate(chosenPiece);
                    }
                    chosenPiece = null;
                    ClearMovableCoordniates();
                }
            }
        }
    }

    void SetChosenPiece(ChessPiece targetPiece)
    {
        ClearMovableCoordniates();

        movableCoordinates.AddRange(targetPiece.GetMovableCoordinates());
        foreach (var c in movableCoordinates)
        {
            gameBoard.GetBoardSquare(c).ismMovable = true;
        }

        chosenPiece = targetPiece;
    }
    void ClearMovableCoordniates()
    {
        movableCoordinates.Clear();
        foreach (var sq in gameBoard.gameData.boardSquares)
        {
            sq.ismMovable = false;
        }
    }
    bool IsMovableCoordniate(Vector2Int coordinate)
    {
        return movableCoordinates.Contains(coordinate);
    }
    bool IsMyPiece(ChessPiece chessPiece)
    {
        return chessPiece.pieceColor == playerColor;
    }
}
