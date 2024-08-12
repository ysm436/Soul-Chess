using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    public GameBoard.PlayerColor playerColor;
    public GameBoard gameBoard;
    public SoulOrb soulOrb; //코스트 프리팹

    ChessPiece chosenPiece = null;
    List<Vector2Int> movableCoordinates = new List<Vector2Int>();

    private Card UsingCard = null;
    private TargetingEffect targetingEffect;
    public bool isUsingCard = false;
    List<TargetableObject> targetableObjects = new List<TargetableObject>();

    public Action OnMyTurnStart;
    public Action OnOpponentTurnStart;

    public Action OnMyDraw;
    public Action OnOpponentDraw;

    public Action OnMyTurnEnd;
    public Action OnOpponentTurnEnd;

    [SerializeField] private bool _isMyTurn;
    public bool isMyTurn { get => _isMyTurn; }

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
        if (!GameBoard.instance.isActivePlayer)
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

                        SetTargetableObjects(true);
                    }
                }
            }
        }
        else
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
                            if (chosenPiece.Attack(targetPiece))
                            {
                                chosenPiece.Move(coordinate);
                                gameBoard.chessBoard.SetPiecePositionByCoordinate(chosenPiece);
                            }
                            else if (!chosenPiece.isAlive)
                            {
                                //이벤트 메커니즘 수정하면서 다시 체크해볼게요
                                //gameBoard.KillPiece(chosenPiece);
                            }

                            chosenPiece = null;
                            ClearMovableCoordniates();
                        }
                    }
                }
                else //고른 칸이 빈칸일때
                {
                    if (IsMovableCoordniate(coordinate))
                    {
                        chosenPiece.Move(coordinate);
                        gameBoard.chessBoard.SetPiecePositionByCoordinate(chosenPiece);
                    }
                    chosenPiece = null;
                    ClearMovableCoordniates();
                }
            }
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
            sq.isTargetable = false;
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
    void SetTargetableObjects(bool isTargetable)
    {
        foreach (var obj in targetableObjects)
            if (obj is ChessPiece)
                GameBoard.instance.GetBoardSquare((obj as ChessPiece).coordinate).isTargetable = isTargetable;
    }
    public void UseCard(Card card)
    {
        UsingCard = card;
        isUsingCard = true;

        if (!(card.EffectOnCardUsed is TargetingEffect))
        {
            UseCardEffect();
            return;
        }
        else
        {
            targetingEffect = card.EffectOnCardUsed as TargetingEffect;
        }


        ClearMovableCoordniates();

        if (!targetingEffect.isAvailable(playerColor))
        {
            UsingCard = null;
            isUsingCard = false;
            return;
        }

        targetableObjects = targetingEffect.GetTargetType().GetTargetList(playerColor);

        if (targetingEffect.GetTargetType().targetType == TargetingEffect.TargetType.Piece)
        {
            SetTargetableObjects(true);
        }

        GameBoard.instance.ShowCard(card);
    }
    public void UseCardEffect()
    {
        UsingCard.EffectOnCardUsed.EffectAction();
        GameBoard.instance.CurrentPlayerData().soulEssence -= UsingCard.cost;

        if (!(UsingCard is SoulCard))
            UsingCard.Destroy();
        UsingCard = null;
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
