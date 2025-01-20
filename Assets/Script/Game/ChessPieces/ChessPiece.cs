using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using Unity.Mathematics;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

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

    public bool isAlive;
    [HideInInspector] public bool isRevivable = false; //부활 체크용
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

    public float moveDuration = 0.3f;
    public bool AffectByHades = false;
    public bool AffectByAbel = false;

    // 키워드 우선순위: 면역, 도발, 보호막, 방어력
    // isTauntAttack은 도발이 연쇄적으로 작동하지 않도록 함
    public void MinusHP(int value/* , bool isTauntAttack = false */)
    {
        if (GetKeyword(Keyword.Type.Immunity) == 1)
            return;

        /* if (!isTauntAttack)
        {
            ChessPiece tauntPiece = GetTauntPieceAround();
            if (tauntPiece != null)
            {
                tauntPiece.MinusHP(value, true);
                return;
            }
        } */

        if (GetKeyword(Keyword.Type.Shield) == 1)
        {
            SetKeyword(Keyword.Type.Shield, 0);
            buff.TryRemoveSpecificBuff("", Buff.BuffType.Shield); //보호막 버프 제거
            return;
        }

        /* if (GetKeyword(Keyword.Type.Defense) > 0)
        {
            if (value < GetKeyword(Keyword.Type.Defense))       //피해량보다 방어력이 클 경우
                return;
            value -= keywordDictionary[Keyword.Type.Defense];
        } */

        _currentHP -= value;

        if (AffectByHades) //하데스 능력을 받을 경우
        {
            if (_currentHP <= 0)
            {
                _currentHP = 1;
            }
        }

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
    public int moveCountInThisTurn;
    protected int _moveCount = 0;
    public int moveCount { get => _moveCount; set => _moveCount = value; }

    private PieceObject pieceObject;
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer accessory = null;
    [SerializeField]
    SpriteRenderer accessoryPrefab;
    public PieceEffectIcon effectIcon = null;
    [SerializeField]
    private PieceEffectIcon effectIconPrefab;
    private PieceEffectIcon moveRestrictionIcon = null;

    public Action<ChessPiece> OnKill;
    public Action<ChessPiece> OnKilled; //유언
    public Action<ChessPiece> OnStartAttack;
    public Action<ChessPiece> OnEndAttack;
    public Action<ChessPiece, int> OnAttacked;
    public Action<ChessPiece> OnAttackedAfter;
    public Action<ChessPiece> OnSpellAttacked;
    //public Action OnGetMovableCoordinate;
    public Action<Vector2Int> OnMove;
    public Action OnSoulRemoved;

    public Buff buff = null;
    private Dictionary<Keyword.Type, int> keywordDictionary;    // 1 true, 0 false (방어력은 N)

    private int _tauntNumber;
    public int tauntNumber { get => _tauntNumber; }

    protected bool isSoulSet;

    private Animator animator;
    public AnimationCurve speedCurve;

    public Material pieceMat;

    private void Awake()
    {
        _currentHP = _maxHP;
        baseHP = _maxHP;
        baseAD = attackDamage;

        pieceObject = GetComponent<PieceObject>();
        spriteRenderer = GetComponent<SpriteRenderer>();

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

        if (effectIcon == null)
        {
            effectIcon = Instantiate(effectIconPrefab, transform.position, Quaternion.identity);
            effectIcon.transform.SetParent(transform);
            effectIcon.piece = this;
        }
        animator = GetComponent<Animator>();
        pieceMat = GetComponent<Renderer>().material;
    }
    private void Start()
    {
        GameBoard.instance.myController.OnMyTurnStart += () => moveCountInThisTurn = 0;
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
        animator.SetBool("isAttacking", true);
        OnStartAttack?.Invoke(targetPiece);
        //은신 해제
        SetKeyword(Keyword.Type.Stealth, 0);
        buff.TryRemoveSpecificBuff("", Buff.BuffType.Stealth);

        targetPiece.Attacked(this, attackDamage);

        bool targetIsKilled = !targetPiece.isAlive;
        if (!targetPiece.isAlive)
        {
            OnKill?.Invoke(targetPiece);
        }

        OnEndAttack?.Invoke(targetPiece);

        if (AffectByAbel) // 아벨 능력에 의해 죽었을 경우 위치 조정을 하지 않음
        {
            AffectByAbel = false;
            return false;
        }

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
        OnAttackedAfter?.Invoke(chessPiece);
        return isAlive;
    }
    public bool SpellAttacked(int damage)
    {
        if (pieceType == PieceType.King)
            return isAlive;

        OnSpellAttacked?.Invoke(this);

        MinusHP(damage); //멀린 효과 : 내 마법 피해 2배 구현용

        if (isAlive)
        {
            StartCoroutine(GameBoard.instance.chessBoard.AttackedAnimationC(this));
        }
        else
        {
            GetComponent<Animator>().SetTrigger("killedTrigger");
            MakeAttackedEffect();
        }

        return isAlive;
    }

    public void Kill()
    {
        OnKilled?.Invoke(this);
        if (!isRevivable)
        {
            RemoveSoul();

            isAlive = false;
            pieceObject.HPText.text = "0";
            GameBoard.instance.KillPiece(this);
        }
        else isRevivable = false;
    }

    public void SetSoul(SoulCard targetSoul, Sprite sprite)
    {
        if (soul != null)
            RemoveSoul();

        isSoulSet = true;                                       // 영혼이 부여된 턴에는 이동 불가
        if (moveRestrictionIcon == null) moveRestrictionIcon = Instantiate(effectIconPrefab, transform.position, Quaternion.identity);
        moveRestrictionIcon.MoveRestrictionEffect();
        if (pieceColor == GameBoard.PlayerColor.White)
            GameBoard.instance.whiteController.OnMyTurnEnd += MakeIsSoulSetFalse;
        else GameBoard.instance.blackController.OnMyTurnEnd += MakeIsSoulSetFalse;

        SetKeyword(Keyword.Type.Silence, 0);                    // 영혼 부여 시 침묵 초기화

        soul = targetSoul;
        soul.transform.SetParent(transform);
        soul.transform.localPosition = Vector3.zero;
        soul.gameObject.SetActive(false);

        //악세서리 생성
        if (accessory == null)
        {
            accessory = Instantiate(accessoryPrefab, this.transform.position, Quaternion.identity);
            accessory.transform.SetParent(this.transform);
            accessory.sprite = sprite;
        }

        targetSoul.InfusedPiece = this;

        maxHP += soul.HP;
        AD += soul.AD;

        /* if (GetKeyword(Keyword.Type.Restraint) == 1)            // 구속된 기물의 영혼 교체할 때 구속 유지
        {
            soul.RemoveEffect();
        } */
    }

    public void RemoveSoul()
    {
        if (soul == null)
            return;

        RemoveBuff();

        maxHP -= soul.HP;
        AD -= soul.AD;

        //구버전 코드
        //spriteRenderer.sprite = defaultSprite;


        //악세서리 제거
        if (accessory != null)
        {
            Destroy(accessory);
            accessory = null;
        }

        OnSoulRemoved?.Invoke();
        OnSoulRemoved = null;

        Destroy(soul);
        soul = null;
    }

    // n = 1: 활성화 / n = 0: 비활성화
    // 예외) 방어력 n >= 1: 활성화 및 방어력 수치 나타냄 / n = 0: 비활성화
    public void SetKeyword(Keyword.Type keywordType, int n = 1)
    {
        int oldKeyword = GetKeyword(keywordType);
        /* if (keywordType == Keyword.Type.Defense)
            keywordDictionary[keywordType] += n;
        else */
        keywordDictionary[keywordType] = n;

        //버프 및 디버프 아이콘 스프라이트 설정 (우선순위는 Enum 값이 작을수록 높음)
        effectIcon.SetIconSprite();

        /* if (keywordType == Keyword.Type.Taunt && n == 1)
        {
            _tauntNumber = GameBoard.instance.gameData.tauntNumber;          //도발 부여 순서 저장
        }
        else */ if (keywordType == Keyword.Type.Stun && n == 1)
        {
            //효과 발동 시점의 색깔 턴이 한번 더 돌아와야 스턴 해제
            GameBoard.instance.whiteController.OnMyTurnStart -= Unstun; //이미 있는 스턴 덮어씌우기
            GameBoard.instance.blackController.OnMyTurnStart -= Unstun;
            GameBoard.instance.CurrentPlayerController().OnMyTurnStart += Unstun;
        }
        /* else if (keywordType == Keyword.Type.Restraint && n == 1)
        {
            if (soul != null && oldKeyword != 1) //구속 풀리기 전에 다시 구속되면 RemoveEffect가 2번 실행되는 버그 수정
            {
                if (GetKeyword(Keyword.Type.Silence) != 1) //침묵 상태에서 구속될 때 RemoveEffect가 2번 실행되는 버그 수정
                    soul.RemoveEffect();
            }
        } */
        else if (keywordType == Keyword.Type.Silence && n == 1)
        {
            if (soul != null)
            {
                if (oldKeyword != 1/*  && GetKeyword(Keyword.Type.Restraint) != 1 */) soul.RemoveEffect(); //침묵,구속 풀리기 전에 다시 침묵되면 RemoveEffect 2번 실행 버그 수정
                RemoveBuff();
            }
        }
        /* else if (keywordType == Keyword.Type.Rush && n == 1)
        {
            MakeIsSoulSetFalse();
        } */
    }

    public int GetKeyword(Keyword.Type keywordType) => keywordDictionary[keywordType];

    // 영혼 부여 시 그 턴 이동 제한
    private void MakeIsSoulSetFalse()
    {
        isSoulSet = false;
        DestroyMoveRestrictionIcon();
        GameBoard.instance.myController.OnMyTurnEnd -= MakeIsSoulSetFalse;
    }

    public void DestroyMoveRestrictionIcon()
    {
        if (moveRestrictionIcon != null) 
        { 
            moveRestrictionIcon.DestroyIcon();
            moveRestrictionIcon = null;
        }
    }

    // 스턴 해제 (다시 턴 시작 시 호출)
    public void Unstun()
    {
        SetKeyword(Keyword.Type.Stun, 0);
        buff.TryRemoveSpecificBuff("", Buff.BuffType.Stun); //스턴 정보 제거
        GameBoard.instance.CurrentPlayerController().OnMyTurnStart -= Unstun;
    }

    /* // 구속 해제
    public void Unrestraint()
    {
        SetKeyword(Keyword.Type.Restraint, 0);
        buff.TryRemoveSpecificBuff("", Buff.BuffType.Restraint); //구속 정보 제거
        if (soul != null && GetKeyword(Keyword.Type.Silence) != 1)      // 기물이 침묵 상태가 아니어야 함
        {
            soul.AddEffect();
        }
    } */

    /* private ChessPiece GetTauntPieceAround()
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
    } */

    public void RemoveBuff()        // RemoveSoul과 침묵 키워드에서만 호출
    {
        foreach (Buff.BuffInfo buffInfo in buff.buffList)
        {
            if (!buffInfo.isRemovableByEffect) continue;

            Buff.BuffType buffType = buffInfo.buffType;

            if (buffType == Buff.BuffType.AD)
            {
                attackDamage -= buffInfo.value;                 // 버프 제거 시 공격력이 1 아래로 떨어지지 않음
                if (attackDamage < 0) attackDamage = 0;
            }
            else if (buffType == Buff.BuffType.HP)
            {
                int newMaxHP = _maxHP - buffInfo.value;         // 버프 제거 시 체력이 1 아래로 떨어지지 않음
                if (newMaxHP <= 0) newMaxHP = 1;

                maxHP = newMaxHP;
            }
            else if (buffType == Buff.BuffType.MoveCount)
            {
                moveCount -= buffInfo.value;
                if (moveCount < 1) moveCount = 0;
            }
            /* else if (buffType == Buff.BuffType.Defense)
            {
                SetKeyword(Keyword.Type.Defense, -buffInfo.value);
            } */
            else if (buffType == Buff.BuffType.Immunity)
            {
                SetKeyword(Keyword.Type.Immunity, 0);
            }
            /* else if (buffType == Buff.BuffType.Taunt)
            {
                SetKeyword(Keyword.Type.Taunt, 0);
            } */
            else if (buffType == Buff.BuffType.Shield)
            {
                SetKeyword(Keyword.Type.Shield, 0);
            }
            else if (buffType == Buff.BuffType.Stun)
            {
                SetKeyword(Keyword.Type.Stun, 0);
            }
            /* else if (buffType == Buff.BuffType.Restraint)
            {
                SetKeyword(Keyword.Type.Restraint, 0);
            } */
            else if (buffType == Buff.BuffType.Stealth)
            {
                SetKeyword(Keyword.Type.Stealth, 0);
            }
            else if (buffType == Buff.BuffType.Silence)
            {
                SetKeyword(Keyword.Type.Silence, 0);
            }
            /* else if (buffType == Buff.BuffType.Rush)
            {
                SetKeyword(Keyword.Type.Rush, 0);
            } */

            // buffType == Buff.BuffType.Description인 경우는 OnSoulRemoved 통해 구현
        }

        pieceObject.HPText.text = _currentHP.ToString();
        pieceObject.ADText.text = attackDamage.ToString();

        buff.ClearBuffList();
    }

    public void MakeAttackedEffect()
    {
        //피격 이펙트
        PieceEffectIcon attackedEffect = Instantiate(effectIconPrefab, transform.position, Quaternion.identity);
        attackedEffect.AttackedEffect();
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

