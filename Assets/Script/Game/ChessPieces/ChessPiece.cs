using System;
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
    public bool isAlive;

    public enum PieceType
    {
        King,
        Quene,
        Bishop,
        Knight,
        Rook,
        Pawn
    }
    [Serializable]
    public enum PieceColor
    {
        White, Black
    }


    public PieceType pieceType;
    public PieceColor pieceColor;

    public int attackDamage = 10;

    public int HP
    {
        set { current_HP = value; isAlive = current_HP > 0; }
        get { return current_HP; }
    }
    private int current_HP = 10;
    public int max_HP = 10;

    private void Awake()
    {
        current_HP = max_HP;
    }

    /// <summary>
    ///     해당 기물이 이동할 수 있는 좌표를 반환하는 함수 
    /// </summary>
    /// <returns>
    ///     Vector2Int 리스트 형태로 이동할 수 있는 모든 좌표를 반환
    /// </returns>
    abstract public List<Vector2Int> GetMovableCoordinates();
    virtual public void Move(Vector2Int targetCoordinate)
    {
        coordinate = targetCoordinate;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="targetPiece"></param>
    /// <returns>target is killed</returns>
    public bool Attack(ChessPiece targetPiece)
    {
        targetPiece.HP -= attackDamage;
        if (!targetPiece.isAlive)
            return true;

        HP -= targetPiece.attackDamage;
        return false;
    }
    public void OnDead()
    {

    }

}

