using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     체스 기물의 기본 클래스
/// </summary>
abstract public class ChessPiece : MonoBehaviour
{
    protected ChessData _chessData;
    public ChessData chessData { set { _chessData = value; } }
    public Vector2Int coordinate;
    protected bool isAlive;

    public enum PieceType
    {
        King,
        Quene,
        Bishop,
        Knight,
        Rook,
        Pawn
    }
    public enum PieceColor
    {
        White, Black
    }


    public PieceType pieceType;
    public PieceColor pieceColor;

    public int attackDamage = 5;
    public int max_HP = 5;
    public int current_HP = 5;

    private void Start()
    {
        foreach (var v in GetMovableCoordinates())
        {
            Debug.Log(v);
        }
    }

    /// <summary>
    ///     해당 기물이 이동할 수 있는 좌표를 반환하는 함수 
    /// </summary>
    /// <returns>
    ///     Vector2Int 리스트 형태로 이동할 수 있는 모든 좌표를 반환
    /// </returns>
    abstract public List<Vector2Int> GetMovableCoordinates();
    public void Move(Vector2Int targetCoordinate)
    {
        coordinate = targetCoordinate;
    }
    /*
    abstract public bool TryMove(float targetCoordinate);
    */

}

