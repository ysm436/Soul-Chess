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
    private int additionalMoveCount = 0;

    private bool isMoved;
    public bool TurnEndPossible
    {
        get
        {
            return isMoved || !(GameBoard.instance.gameData.pieceObjects.Any(obj => (obj.GetMovableCoordinates().Count >= 1 && obj.pieceColor == playerColor)));
        }
    }

    private Card usingCard = null;
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
        OnOpponentTurnEnd += () => { isMoved = false; };
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
            if (targetableObjects.Any(obj => obj.coordinate == coordinate))
            {
                ClearTargetableObjects();

                TargetableObject target;
                if (targetingEffect.GetTargetType().targetType == TargetingEffect.TargetType.Piece)
                    target = targetPiece;
                else // if (targetingEffect.GetTargetType().targetType == TargetingEffect.TargetType.Tile)
                    target = GameBoard.instance.gameData.boardSquares[coordinate.x, coordinate.y];

                if (targetingEffect.SetTarget(target))
                {
                    UseCardEffect();
                }
                else
                {
                    targetableObjects = targetingEffect.GetTargetType().GetTargetList(playerColor);

                    //타겟 효과가 부정적인지 파라미터 전달
                    SetTargetableObjects(true, targetingEffect.IsNegativeEffect);
                }
            }
        }
        else //이동 관련 코드
        {
            if (chosenPiece == null)//선택된 (아군)기물이 없을 때
            {
                if (targetPiece != null)
                {
                    if (IsMyPiece(targetPiece))//고른 기물이 아군일때
                        if (!isMoved || (targetPiece.moveCountInThisTurn > 0 && targetPiece.moveCountInThisTurn <= targetPiece.moveCount))
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

        srcPiece.moveCountInThisTurn++;
        isMoved = true;

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
            if (isTargetable)
            {
                //타겟 효과가 부정적인지 체크
                if (isNegativeEffect)
                    GameBoard.instance.GetBoardSquare(obj.coordinate).isNegativeTargetable = true;
                else
                    GameBoard.instance.GetBoardSquare(obj.coordinate).isPositiveTargetable = true;
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

        //타겟 효과가 부정적인지 파라미터 전달
        SetTargetableObjects(true, targetingEffect.IsNegativeEffect);
    }
    public void UseCardEffect()
    {
        if (isInfusing)
        {
            (usingCard as SoulCard).infusion.EffectAction(this);
            if (usingCard.EffectOnCardUsed != null)
            {
                targetingEffect = null;
                UseCard(usingCard);
                return;
            }
            isInfusing = false;
        }

        if (usingCard is SoulCard)
        {
            if (usingCard.EffectOnCardUsed is TargetingEffect)
                photonView.RPC("UseCardRemote", RpcTarget.Others, usingCard.handIndex, (usingCard as SoulCard).infusion.targetCoordinates[0], (usingCard.EffectOnCardUsed as TargetingEffect).targetCoordinates);
            else
                photonView.RPC("UseCardRemote", RpcTarget.Others, usingCard.handIndex, (usingCard as SoulCard).infusion.targetCoordinates[0], null);
        }
        else
        {
            if (usingCard.EffectOnCardUsed is TargetingEffect)
                photonView.RPC("UseCardRemote", RpcTarget.Others, usingCard.handIndex, new Vector2(-1, -1), (usingCard.EffectOnCardUsed as TargetingEffect).targetCoordinates);
            else
                photonView.RPC("UseCardRemote", RpcTarget.Others, usingCard.handIndex, new Vector2(-1, -1), null);
        }

        usingCard.EffectOnCardUsed?.EffectAction(this);


        GameBoard.instance.CurrentPlayerData().soulEssence -= usingCard.cost;

        if (!(usingCard is SoulCard))
            usingCard.Destroy();
        usingCard = null;
        isUsingCard = false;
        targetingEffect = null;

        GameBoard.instance.HideCard();

    }

    [PunRPC]
    public void UseCardRemote(int cardIndex, Vector2 infusionTarget, Vector2[] targetArray = null)
    {
        Card card = gameBoard.gameData.opponentPlayerData.hand[cardIndex];
        GameBoard.instance.CurrentPlayerData().soulEssence -= card.cost;

        gameBoard.ShowCard(card);
        GameBoard.instance.gameData.opponentPlayerData.TryRemoveCardInHand(card);

        if (card is SoulCard)
        {

            Vector2Int infusionTargetCoordinate = Vector2Int.RoundToInt(infusionTarget);

            Debug.Log(infusionTargetCoordinate);

            (card as SoulCard).infusion.SetTargetsByCoordinate(new Vector2Int[] { infusionTargetCoordinate });
            gameBoard.gameData.boardSquares[infusionTargetCoordinate.x, infusionTargetCoordinate.y].outline.changeOutline(BoardSquareOutline.TargetableStates.movable);

            (card as SoulCard).infusion.EffectAction(gameBoard.opponentController);
        }

        if (card.EffectOnCardUsed is TargetingEffect)
        {
            Vector2Int[] targetCoordinateArray = targetArray.Select(coordinate => Vector2Int.RoundToInt(coordinate)).ToArray();
            TargetingEffect targetingEffect = card.EffectOnCardUsed as TargetingEffect;
            targetingEffect.SetTargetsByCoordinate(targetCoordinateArray);

            if (targetingEffect.IsPositiveEffect)
            {
                foreach (Vector2Int targetCoordinate in targetCoordinateArray)
                {
                    gameBoard.gameData.boardSquares[targetCoordinate.x, targetCoordinate.y].outline.changeOutline(BoardSquareOutline.TargetableStates.positive);
                }
            }
            else if (targetingEffect.IsNegativeEffect)
            {
                foreach (Vector2Int targetCoordinate in targetCoordinateArray)
                {
                    gameBoard.gameData.boardSquares[targetCoordinate.x, targetCoordinate.y].outline.changeOutline(BoardSquareOutline.TargetableStates.negative);
                }
            }

            targetingEffect.EffectAction(gameBoard.opponentController);
        }
        else if (card.EffectOnCardUsed != null)
        {

            card.EffectOnCardUsed.EffectAction(gameBoard.opponentController);
        }

        if (!(card is SoulCard))
            card.Destroy();

        Invoke("HideRemoteUsedCard", 1f);
    }
    private void HideRemoteUsedCard()
    {
        gameBoard.HideCard();
        foreach (BoardSquare bs in gameBoard.gameData.boardSquares)
        {
            bs.outline.changeOutline(BoardSquareOutline.TargetableStates.none);
        }
        GameBoard.instance.gameData.opponentPlayerData.UpdateHandPosition();
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
        photonView.RPC("LocalDraw", RpcTarget.All);
    }
    [PunRPC]
    public void LocalDraw()
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
