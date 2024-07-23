using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    [HideInInspector]
    public GameData gameData;
    public ChessPiece.PieceColor bottomPlayerColor;
    public ChessBoard chessBoard;

    private void Awake()
    {
        foreach (ChessPiece piece in GetComponentsInChildren<ChessPiece>())
        {
            gameData.TryAddPiece(piece);

            piece.chessData = gameData;
            chessBoard.SetPiecePositionByCoordinate(piece);
        }

        chessBoard.SetBoardSquares(gameData);
    }

    public BoardSquare GetBoardSquare(Vector2Int coordinate)
    {
        return gameData.boardSquares[coordinate.x, coordinate.y];
    }
    public void KillPiece(ChessPiece targetPiece)
    {
        gameData.graveyard.Add(targetPiece);
        gameData.pieceObjects.Remove(targetPiece);

        targetPiece.coordinate = Vector2Int.right * (gameData.graveyard.Count - 1) + Vector2Int.down;
        chessBoard.SetPiecePositionByCoordinate(targetPiece);
    }

}
