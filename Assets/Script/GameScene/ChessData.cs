using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
///     체스 판 및 체스 기물들에 대한 정보를 다룸,
///     좌표는 백색 측 기준
/// </summary>
[System.Serializable]
public class ChessData
{
    static readonly int BOARD_SIZE = 8;

    public BoardSquare[,] boardSquares = new BoardSquare[8, 8];
    [SerializeField]
    public List<ChessPiece> pieceObjects = new List<ChessPiece>();
    public List<ChessPiece> graveyard = new List<ChessPiece>();

    public bool TryAddPiece(ChessPiece piece)
    {
        if (pieceObjects.Any(obj => obj.coordinate == piece.coordinate))
            return false;

        pieceObjects.Add(piece);
        return true;
    }
    /// <summary>
    ///     targetCoordinate가 체스판 안에 있는 유효한 좌표인지 확인
    /// </summary>
    /// <param name="targetCoordinate"></param>
    /// <returns></returns>
    public bool IsValidCoordinate(Vector2Int targetCoordinate)
    {
        if (targetCoordinate.x < 0 || targetCoordinate.x >= BOARD_SIZE)
            return false;
        if (targetCoordinate.y < 0 || targetCoordinate.y >= BOARD_SIZE)
            return false;

        return true;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="targetCoordinate"></param>
    /// <returns>
    ///     ChessPiece in coordniate (nullable)
    /// </returns>
    public ChessPiece GetPiece(Vector2Int targetCoordinate)
    {
        return pieceObjects.FirstOrDefault(obj => obj.coordinate == targetCoordinate);
    }
    public void MoveFormTo(Vector2Int startCoordinate, Vector2Int targetCoordinate)
    {

    }
}