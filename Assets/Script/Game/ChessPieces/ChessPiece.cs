using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
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
    public SoulCard soul
    {
        set { _soul = value; }
        get { return _soul; }
    }

    private SoulCard _soul = null;
    public PieceType pieceType;
    public GameManager.PlayerColor pieceColor;

    public int AD
    {
        set
        {
            if (value < 0)
                attackDamage = 0;
            else
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
                Kill();
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

    //악세서리
    private PieceAccessory accessory = null;

    private PieceObject pieceObject;

    public Action<ChessPiece> OnKill;
    public Action<ChessPiece> OnKilled; //유언
    public Action<ChessPiece> OnStartAttack;
    public Action<ChessPiece> OnEndAttack;
    public Action<ChessPiece, int> OnAttacked;
    public Action OnSpellAttacked;
    //public Action OnGetMovableCoordinate;
    public Action<Vector2Int> OnMove;

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
        OnMove?.Invoke(targetCoordinate);

        coordinate = targetCoordinate;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="targetPiece"></param>
    /// <returns>target is killed</returns>
    public bool Attack(ChessPiece targetPiece)
    {
        OnStartAttack?.Invoke(targetPiece);

        targetPiece.Attacked(this, attackDamage);

        bool targetIsKilled = !targetPiece.isAlive;
        if (!targetPiece.isAlive)
        {
            OnKill?.Invoke(targetPiece);
        }
        else
        {
            HP -= targetPiece.attackDamage;
        }


        OnEndAttack?.Invoke(targetPiece);

        return targetIsKilled;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns> isAlive </returns>
    public bool Attacked(ChessPiece chessPiece, int damage)
    {
        OnAttacked?.Invoke(chessPiece, damage);

        HP -= damage;
        return isAlive;
    }
    public bool SpellAttacked(int damage)
    {
        OnSpellAttacked?.Invoke();

        HP -= damage;
        return isAlive;
    }

    public void Kill()
    {
        OnKilled?.Invoke(this);

        isAlive = false;
        pieceObject.HPText.text = "0";
        GameManager.instance.KillPiece(this);
    }

    public void SetSoul(SoulCard targetSoul)
    {
        if (soul != null)
            RemoveSoul();

        soul = targetSoul;
        soul.transform.SetParent(transform);
        soul.transform.localPosition = Vector3.zero;
        soul.gameObject.SetActive(false);

        //악세서리 오브젝트를 기물에 자식으로 생성
        if (soul.accessory != null)
        {
            accessory = Instantiate(soul.accessory, transform.position, Quaternion.identity);
            accessory.transform.SetParent(transform);
        }
        targetSoul.InfusedPiece = this;

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

