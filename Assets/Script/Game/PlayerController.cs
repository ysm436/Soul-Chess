using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using System;
using Photon.Pun;
using Photon.Pun.Demo.Cockpit;

public class PlayerController : MonoBehaviour
{
    private PhotonView photonView;

    public GameBoard.PlayerColor playerColor;
    public GameBoard gameBoard;
    public SoulOrb soulOrb; //코스트 프리팹

    ChessPiece chosenPiece = null;
    List<Vector2Int> movableCoordinates = new List<Vector2Int>();

    private int movableCount = 1;
    public bool TurnEndPossible
    {
        get
        {
            return (movableCount <= 0) || !(GameBoard.instance.gameData.pieceObjects.Any(obj => (obj.GetMovableCoordinates().Count >= 1 && obj.pieceColor == playerColor)));
        }
    }

    private Card UsingCard = null;
    private TargetingEffect targetingEffect;
    public bool isUsingCard = false;
    private bool isInfusing = false;
    List<TargetableObject> targetableObjects = new List<TargetableObject>();

    public Action OnMyTurnStart;
    public Action OnOpponentTurnStart;

    public Action OnMyDraw;
    public Action OnOpponentDraw;

    public Action OnMyTurnEnd;
    public Action OnOpponentTurnEnd;

    [SerializeField] private bool _isMyTurn;
    public bool isMyTurn { get => _isMyTurn; }

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }
    private void Start()
    {
        OnOpponentTurnEnd += () => movableCount = 1;
    }
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
        if (!GameBoard.instance.isActivePlayer && !GameBoard.instance.isDebugMode)
            return;

        ChessPiece targetPiece = gameBoard.gameData.GetPiece(coordinate);

        if (isUsingCard)
        {
            if (targetableObjects.Contains(targetPiece))
            {
                ClearTargetableObjects();
                if (targetingEffect.SetTarget(targetPiece))
                {
                    UseCardEffect();
                }
                else
                {
                    targetableObjects = targetingEffect.GetTargetType().GetTargetList(playerColor);

                    if (targetingEffect.GetTargetType().targetType == TargetingEffect.TargetType.Piece)
                    {
                        //타겟 효과가 부정적인지 파라미터 전달
                        SetTargetableObjects(true, targetingEffect.IsNegativeEffect);
                    }
                }
            }
        }
        else if (movableCount > 0)
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
                            photonView.RPC("MovePiece", RpcTarget.All, chosenPiece.coordinate.x, chosenPiece.coordinate.y, coordinate.x, coordinate.y, true);

                            chosenPiece = null;
                            ClearMovableCoordniates();
                        }
                    }
                }
                else //고른 칸이 빈칸일때
                {
                    if (IsMovableCoordniate(coordinate))
                    {
                        photonView.RPC("MovePiece", RpcTarget.All, chosenPiece.coordinate.x, chosenPiece.coordinate.y, coordinate.x, coordinate.y, false);
                    }
                    chosenPiece = null;
                    ClearMovableCoordniates();
                }
            }
        }
    }

    [PunRPC]
    private void MovePiece(int src_x, int src_y, int dst_x, int dst_y, bool isAttack)
    {
        Vector2Int dst_coordinate = new Vector2Int(dst_x, dst_y);
        Vector2Int src_coordinate = new Vector2Int(src_x, src_y);

        ChessPiece srcPiece = GameBoard.instance.gameData.GetPiece(src_coordinate);

        if (isAttack)
        {
            ChessPiece dstPiece = GameBoard.instance.gameData.GetPiece(dst_coordinate);
            if (srcPiece.Attack(dstPiece))
            {
                srcPiece.Move(dst_coordinate);
                gameBoard.chessBoard.SetPiecePositionByCoordinate(srcPiece);
            }
            else if (!srcPiece.isAlive)
            {
                //이벤트 메커니즘 수정하면서 다시 체크해볼게요
                //gameBoard.KillPiece(srcPiece);
            }
        }
        else
        {
            srcPiece.Move(dst_coordinate);
            gameBoard.chessBoard.SetPiecePositionByCoordinate(srcPiece);
        }

        movableCount--;
    }
    void SetChosenPiece(ChessPiece targetPiece)
    {
        ClearMovableCoordniates();

        movableCoordinates.AddRange(targetPiece.GetMovableCoordinates());
        foreach (var c in movableCoordinates)
        {
            gameBoard.GetBoardSquare(c).isMovable = true;
        }

        chosenPiece = targetPiece;
    }
    void ClearMovableCoordniates()
    {
        movableCoordinates.Clear();
        foreach (var sq in gameBoard.gameData.boardSquares)
        {
            sq.isNegativeTargetable = false;
            sq.isPositiveTargetable = false;
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

    void ClearTargetableObjects()
    {
        targetableObjects.Clear();
        foreach (var sq in gameBoard.gameData.boardSquares)
        {
            sq.isMovable = false;
        }
    }
    void SetTargetableObjects(bool isTargetable, bool isNegativeEffect)
    {
        foreach (var obj in targetableObjects)
            if (obj is ChessPiece && isTargetable)
            {
                //타겟 효과가 부정적인지 체크
                if (isNegativeEffect) GameBoard.instance.GetBoardSquare((obj as ChessPiece).coordinate).isNegativeTargetable = true;
                else GameBoard.instance.GetBoardSquare((obj as ChessPiece).coordinate).isPositiveTargetable = true;
            }
    }
    public bool UseCard(Card card)
    {
        if (GameBoard.instance.CurrentPlayerData().soulEssence < card.cost)
            return false;

        usingCard = card;
        isUsingCard = true;

        if (usingCard is SoulCard)
        {
            if (!isInfusing)
            {
                if (!(usingCard as SoulCard).infusion.isAvailable(playerColor))
                {
                    usingCard = null;
                    isUsingCard = false;
                    return false;
                }
                else if (usingCard.EffectOnCardUsed is TargetingEffect)
                {
                    if (!(usingCard.EffectOnCardUsed as TargetingEffect).isAvailable(playerColor))
                    {
                        usingCard = null;
                        isUsingCard = false;
                        return false;
                    }
                }

                isInfusing = true;

                targetingEffect = (usingCard as SoulCard).infusion;
                ActiveTargeting();
            }
            else
            {
                isInfusing = false;

                if (usingCard.EffectOnCardUsed is TargetingEffect)
                {
                    if (!(usingCard.EffectOnCardUsed as TargetingEffect).isAvailable(playerColor))
                    {
                        usingCard = null;
                        isUsingCard = false;
                        return false;
                    }
                    targetingEffect = usingCard.EffectOnCardUsed as TargetingEffect;
                    ActiveTargeting();
                }
                else
                {
                    UseCardEffect();
                }
            }
        }
        else
        {
            if (card.EffectOnCardUsed is TargetingEffect)
            {
                targetingEffect = usingCard.EffectOnCardUsed as TargetingEffect;
                ActiveTargeting();
            }
            else
            {
                UseCardEffect();
            }
        }


        return true;
    }
    private void ActiveTargeting()
    {
        ClearMovableCoordniates();

        targetableObjects = targetingEffect.GetTargetType().GetTargetList(playerColor);

        if (targetingEffect.GetTargetType().targetType == TargetingEffect.TargetType.Piece)
        {
            //타겟 효과가 부정적인지 파라미터 전달
            SetTargetableObjects(true, targetingEffect.IsNegativeEffect);
        }
    }
    public void UseCardEffect()
    {
        if (isInfusing)
        {
            targetingEffect.EffectAction();// same as (usingCard as SoulCard).infusion.EffectAction();
            if (usingCard.EffectOnCardUsed != null)
            {
                targetingEffect = null;
                UseCard(usingCard);
                return;
            }
            isInfusing = false;
        }
        else
        {
            usingCard.EffectOnCardUsed.EffectAction();
        }

        GameBoard.instance.CurrentPlayerData().soulEssence -= usingCard.cost;

        if (!(usingCard is SoulCard))
            usingCard.Destroy();
        usingCard = null;
        isUsingCard = false;

        targetingEffect = null;
        GameBoard.instance.HideCard();
    }

    public void TurnStart()
    {
        _isMyTurn = true;
        OnMyTurnStart?.Invoke();
    }

    public void OpponentTurnStart()
    {
        OnOpponentTurnStart?.Invoke();
    }

    public void Draw()
    {
        if (playerColor == GameBoard.PlayerColor.White)
        {
            GameBoard.instance.gameData.playerWhite.DrawCard();
        }
        else
        {
            GameBoard.instance.gameData.playerBlack.DrawCard();
        }
        OnMyDraw?.Invoke();
    }

    public void OpponentDraw()
    {
        OnOpponentDraw?.Invoke();
    }

    public void TurnEnd()
    {
        OnMyTurnEnd?.Invoke();
        //턴 종료 시 상대 코스트 회복
        if (playerColor == GameBoard.PlayerColor.White)
        {
            if (GameBoard.instance.gameData.playerBlack.soulOrbs < 10)
                GameBoard.instance.gameData.playerBlack.soulOrbs++;
            GameBoard.instance.gameData.playerBlack.soulEssence = GameBoard.instance.gameData.playerBlack.soulOrbs;
        }
        else
        {
            if (GameBoard.instance.gameData.playerWhite.soulOrbs < 10)
                GameBoard.instance.gameData.playerWhite.soulOrbs++;
            GameBoard.instance.gameData.playerWhite.soulEssence = GameBoard.instance.gameData.playerWhite.soulOrbs;
        }
    }

    public void OpponentTurnEnd()
    {
        OnOpponentTurnEnd?.Invoke();
    }
}
