using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// TODO: RemoveSoul에 영혼 묘지 적용?


/// <summary>
///     체스 기물의 기본 클래스
/// </summary>
abstract public class ChessPiece : TargetableObject
{

    private int baseAD = 10;
    private int baseHP = 10;


    protected GameData _chessData;
    public GameData chessData { set { _chessData = value; } }

    public Vector2Int coordinate;
    public bool isAlive;
    private SoulCard soul = null;
    public PieceType pieceType;
    public GameManager.PlayerColor pieceColor;

    public int AD
    {
        set
        {
            attackDamage = value;
            pieceObject.ADText.text = attackDamage.ToString();
        }
        get { return attackDamage; }
    }
    [SerializeField]
    private int attackDamage;

    public int HP
    {
        set
        {
            _currentHP = value < _maxHP ? value : _maxHP;
            if (_currentHP > 0)
                pieceObject.HPText.text = _currentHP.ToString();
            else
                OnKilled();
        }
        get { return _currentHP; }
    }
    private int _currentHP;
    public int maxHP
    {
        set
        {
            if (value > _maxHP)
                _currentHP += (value - _maxHP);
            else
                _currentHP = value < _currentHP ? value : _currentHP;

            _maxHP = value;

            pieceObject.HPText.text = _currentHP.ToString();
        }
        get
        {
            return _maxHP;
        }
    }
    [SerializeField]
    private int _maxHP;


    private PieceObject pieceObject;

    private void Awake()
    {
        _currentHP = _maxHP;
        baseHP = _maxHP;
        baseAD = attackDamage;

        pieceObject = GetComponent<PieceObject>();

        pieceObject.HPText.text = _currentHP.ToString();
        pieceObject.ADText.text = attackDamage.ToString();
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

    public void OnKilled()
    {
        isAlive = false;
        pieceObject.HPText.text = "0";
        GameManager.instance.KillPiece(this);
    }

    public void SetSoul(SoulCard targetSoul)
    {
        if (soul != null)
            RemoveSoul();

        soul = targetSoul;

        maxHP += soul.HP;
        AD += soul.AD;
    }
    public void RemoveSoul()
    {
        if (soul == null)
            return;

        maxHP -= soul.HP;
        AD -= soul.AD;

        Destroy(soul);
        soul = null;
    }

    [Flags]
    public enum PieceType
    {
        None = 0b00_0000,
        Pawn = 0b00_0001,
        Knight = 0b00_0010,
        Bishop = 0b00_0100,
        Rook = 0b00_1000,
        Quene = 0b01_0000,
        King = 0b10_0000
    }
}

