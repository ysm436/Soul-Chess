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
        if (GetKeyword(Keyword.Type.Immunity) == 1)
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

        if (GetKeyword(Keyword.Type.Shield) == 1)
        {
            keywordDictionary[Keyword.Type.Shield] = 0;
            return;
        }

        if (GetKeyword(Keyword.Type.Defense) > 0)
        {
            if (value < GetKeyword(Keyword.Type.Defense))       //피해량보다 방어력이 클 경우
                return;
            value -= keywordDictionary[Keyword.Type.Defense];
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

    protected int _moveCount;
    public int moveCount { get => _moveCount; set => _moveCount = value; }

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
    public Action OnSoulRemoved;

    public Buff buff = null;
    private Dictionary<Keyword.Type, int> keywordDictionary;    // 1 true, 0 false (방어력은 N)

    private int _tauntNumber;
    public int tauntNumber { get => _tauntNumber; }

    protected bool isSoulSet;


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
        foreach (Keyword.Type keywordType in Keyword.AllKeywords)
        {
            keywordDictionary.Add(keywordType, 0);
        }

        isSoulSet = false;

        if (buff == null)
            buff = new Buff();
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

        SetKeyword(Keyword.Type.Stealth, 0);
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
        RemoveSoul();

        isAlive = false;
        pieceObject.HPText.text = "0";
        GameBoard.instance.KillPiece(this);
    }

    public void SetSoul(SoulCard targetSoul, Sprite sprite)
    {
        if (soul != null)
            RemoveSoul();

        isSoulSet = true;              // 영혼이 부여된 턴에는 이동 불가
        GameBoard.instance.myController.OnMyTurnEnd += MakeIsSoulSetFalse;


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

        RemoveBuff();

        spriteRenderer.sprite = defaultSprite;

        OnSoulRemoved?.Invoke();
        OnSoulRemoved = null;

        Destroy(soul);
        soul = null;
    }

    //n은 방어력 지정할 때만 사용, 방어력 수치 나타냄
    public void SetKeyword(Keyword.Type keywordType, int n = 1)
    {
        keywordDictionary[keywordType] = n;

        if (keywordType == Keyword.Type.Taunt)
        {
            _tauntNumber = GameBoard.instance.gameData.tauntNumber;          //도발 부여 순서 저장
        }
        else if (keywordType == Keyword.Type.Stun)
        {
            GameBoard.instance.myController.OnMyTurnEnd += Unstun;
        }
        else if (keywordType == Keyword.Type.Restraint)
        {
            if (soul != null)
            {
                soul.RemoveEffect();
            }
        }
        else if (keywordType == Keyword.Type.Silence)
        {
            if (soul != null)
            {
                soul.RemoveEffect();
                RemoveBuff();
            }
        }
        else if (keywordType == Keyword.Type.Rush)
        {
            MakeIsSoulSetFalse();
        }
    }

    public int GetKeyword(Keyword.Type keywordType) => keywordDictionary[keywordType];

    // 영혼 부여 시 그 턴 이동 제한
    private void MakeIsSoulSetFalse()
    {
        isSoulSet = false;
        GameBoard.instance.myController.OnMyTurnEnd -= MakeIsSoulSetFalse;
    }

    // 스턴 해제 (턴 종료 시 호출)
    public void Unstun()
    {
        SetKeyword(Keyword.Type.Stun, 0);
        GameBoard.instance.myController.OnMyTurnEnd -= Unstun;
    }

    // 구속 해제
    public void Unrestraint()
    {
        SetKeyword(Keyword.Type.Restraint, 0);

        if (soul != null)
        {
            soul.AddEffect();
        }
    }

    private ChessPiece GetTauntPieceAround()
    {
        List<Vector2Int> aroundCoordinates = new()
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

        for (int i = aroundCoordinates.Count - 1; i >= 0; i--)
        {
            Vector2Int currentCoordinate = aroundCoordinates[i];


            if (!_chessData.IsValidCoordinate(currentCoordinate))
            {
                aroundCoordinates.RemoveAt(i);
                continue;
            }
            if (_chessData.GetPiece(currentCoordinate) == null)
            {
                aroundCoordinates.RemoveAt(i);
                continue;
            }
            else
            {
                if (_chessData.GetPiece(currentCoordinate).pieceColor != pieceColor)
                {
                    aroundCoordinates.RemoveAt(i);
                    continue;
                }
            }
            if (_chessData.GetPiece(currentCoordinate).GetKeyword(Keyword.Type.Taunt) != 1)
            {
                aroundCoordinates.RemoveAt(i);
                continue;
            }
        }

        if (aroundCoordinates.Count == 0)   // 주변에 도발 기물이 없는 경우
            return null;
        else                                // 주변에 도발 기물이 1개 이상인 경우 가장 마지막으로 도발 부여받은 기물 선택
        {
            ChessPiece tauntPiece = _chessData.GetPiece(aroundCoordinates[0]);
            int maxTauntNumber = tauntPiece.tauntNumber;

            foreach (Vector2Int coordinate in aroundCoordinates)
            {
                ChessPiece currentPiece = _chessData.GetPiece(coordinate);

                if (currentPiece.tauntNumber > maxTauntNumber)
                {
                    tauntPiece = currentPiece;
                    maxTauntNumber = currentPiece.tauntNumber;
                }
            }

            return tauntPiece;
        }
    }

    public void RemoveBuff()        // RemoveSoul과 침묵 키워드에서만 호출
    {
        foreach (Buff.BuffInfo buffInfo in buff.buffList)
        {
            if (!buffInfo.isRemovableByEffect) continue;

            Buff.BuffType buffType = buffInfo.buffType;

            if (buffType == Buff.BuffType.AD)
            {
                attackDamage -= buffInfo.value;
                if (attackDamage < 0) attackDamage = 0;
            }
            else if (buffType == Buff.BuffType.HP)
            {
                _currentHP -= buffInfo.value;
                if (_currentHP <= 0) _currentHP = 1;
            }
            else if (buffType == Buff.BuffType.MoveCount)
            {
                moveCount -= buffInfo.value;
                if (moveCount < 1) moveCount = 1;
            }

            // buffType == Buff.BuffType.Description인 경우는 OnSoulRemoved 통해 구현
        }

        pieceObject.HPText.text = _currentHP.ToString();
        pieceObject.ADText.text = attackDamage.ToString();

        buff.ClearBuffList();
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

