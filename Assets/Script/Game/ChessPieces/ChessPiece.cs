using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
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
    public GameBoard.PlayerColor pieceColor;

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

    // 키워드 우선순위: 면역, 도발, 보호막, 방어력
    // isTauntAttack은 도발이 연쇄적으로 작동하지 않도록 함
    public void MinusHP(int value, bool isTauntAttack = false)
    {
        if (GetKeyword(Keyword.type.Immunity) == 1)
            return;

        if (!isTauntAttack)
        {
            ChessPiece tauntPiece = GetTauntPieceAround();
            if (tauntPiece != null)
            {
                tauntPiece.MinusHP(value, true);
                return;
            }
        }

        if (GetKeyword(Keyword.type.Shield) == 1)
        {
            keywordDictionary[Keyword.type.Shield] = 0;
            return;
        }

        if (GetKeyword(Keyword.type.Defense) > 0)
        {
            if (value < GetKeyword(Keyword.type.Defense))       //피해량보다 방어력이 클 경우
                return;
            value -= keywordDictionary[Keyword.type.Defense];
        }

        _currentHP -= value;

        if (_currentHP > 0)
            pieceObject.HPText.text = _currentHP.ToString();
        else
            Kill();
    }

    public void AddHP(int value)
    {
        _currentHP += value;
        if (_currentHP > maxHP) _currentHP = maxHP;

        pieceObject.HPText.text = _currentHP.ToString();
    }

    public int GetHP { get => _currentHP; }
    /*public int HP 
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
    }*/
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
    private SpriteRenderer spriteRenderer;

    private Sprite defaultSprite;

    public Action<ChessPiece> OnKill;
    public Action<ChessPiece> OnKilled; //유언
    public Action<ChessPiece> OnStartAttack;
    public Action<ChessPiece> OnEndAttack;
    public Action<ChessPiece, int> OnAttacked;
    public Action OnSpellAttacked;
    //public Action OnGetMovableCoordinate;
    public Action<Vector2Int> OnMove;

    private Dictionary<Keyword.type, int> keywordDictionary;    // 1 true, 0 false (방어력은 N)

    private void Awake()
    {
        _currentHP = _maxHP;
        baseHP = _maxHP;
        baseAD = attackDamage;

        pieceObject = GetComponent<PieceObject>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        defaultSprite = spriteRenderer.sprite;

        pieceObject.HPText.text = _currentHP.ToString();
        pieceObject.ADText.text = attackDamage.ToString();

        keywordDictionary = new();
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
            MinusHP(targetPiece.attackDamage);
        }


        OnEndAttack?.Invoke(targetPiece);

        return targetIsKilled;
    }

    public bool Attacked(int damage, bool isTauntAttack)
    {
        //OnAttacked?.Invoke(damage);

        MinusHP(damage);
        return isAlive;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns> isAlive </returns>
    public bool Attacked(ChessPiece chessPiece, int damage)
    {
        OnAttacked?.Invoke(chessPiece, damage);

        MinusHP(damage);
        return isAlive;
    }
    public bool SpellAttacked(int damage)
    {
        OnSpellAttacked?.Invoke();

        MinusHP(damage);
        return isAlive;
    }

    public void Kill()
    {
        OnKilled?.Invoke(this);

        isAlive = false;
        pieceObject.HPText.text = "0";
        GameBoard.instance.KillPiece(this);
    }

    public void SetSoul(SoulCard targetSoul, Sprite sprite)
    {
        if (soul != null)
            RemoveSoul();

        soul = targetSoul;
        soul.transform.SetParent(transform);
        soul.transform.localPosition = Vector3.zero;
        soul.gameObject.SetActive(false);

        spriteRenderer.sprite = sprite;

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

        spriteRenderer.sprite = defaultSprite;

        Destroy(soul);
        soul = null;
    }

    //n은 방어력 지정할 때만 사용, 방어력 수치 나타냄
    public void SetKeyword(Keyword.type keywordType, int n = 1)
    {
        if (keywordDictionary.ContainsKey(keywordType))
        {
            keywordDictionary[keywordType] = n;
        }
        else
        {
            keywordDictionary.Add(keywordType, n);
        }
    }

    public int GetKeyword(Keyword.type keywordType)
    {
        if (keywordDictionary.ContainsKey(keywordType))
        {
            return keywordDictionary[keywordType];
        }
        else
        {
            return 0;
        }
    }

    // 주변에 도발 기물이 2개 이상일 경우 가장 마지막으로 도발 부여받은 기물 선택
    private ChessPiece GetTauntPieceAround()
    {
        List<Vector2Int> resultCoordinates = new()
        {
            coordinate + Vector2Int.up,
            coordinate + Vector2Int.up + Vector2Int.right,
            coordinate + Vector2Int.up + Vector2Int.left,
            coordinate + Vector2Int.down,
            coordinate + Vector2Int.down + Vector2Int.right,
            coordinate + Vector2Int.down + Vector2Int.left,
            coordinate + Vector2Int.right,
            coordinate + Vector2Int.left,
        };

        for (int i = resultCoordinates.Count - 1; i >= 0; i--)
        {
            Vector2Int currentCoordinate = resultCoordinates[i];

            if (!_chessData.IsValidCoordinate(currentCoordinate))
            {
                resultCoordinates.RemoveAt(i);
                continue;
            }

            if (_chessData.GetPiece(currentCoordinate) == null)
            {
                resultCoordinates.RemoveAt(i);
            }
            else
            {
                if (_chessData.GetPiece(resultCoordinates[i]).pieceColor != pieceColor)
                {
                    resultCoordinates.RemoveAt(i);
                }
            }
        }

        foreach (Vector2Int coordinate in resultCoordinates)
        {
            if (_chessData.GetPiece(coordinate).GetKeyword(Keyword.type.Taunt) == 1)
            {
                return _chessData.GetPiece(coordinate);
            }
        }

        // 주변에 도발 기물이 없는 경우 null 반환
        return null;
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

