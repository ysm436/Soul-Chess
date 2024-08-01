using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    public GameManager.PlayerColor playerColor;
    public GameManager gameBoard;

    ChessPiece chosenPiece = null;
    List<Vector2Int> movableCoordinates = new List<Vector2Int>();

    private Card UsingCard = null;
    private TargetingEffect targetingEffect;
    public bool isUsingCard = false;
    List<TargetableObject> targetableObjects = new List<TargetableObject>();

    public Action OnMyDraw;
    //    public Action OnOpponentDraw;
    public Action OnMyTurnEnd;
    public Action OnOpponentTurnEnd;

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
                                gameBoard.KillPiece(targetPiece);
                                chosenPiece.Move(coordinate);
                                gameBoard.chessBoard.SetPiecePositionByCoordinate(chosenPiece);
                            }
                            else if (!chosenPiece.isAlive)
                            {
                                gameBoard.KillPiece(chosenPiece);
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
                GameManager.instance.GetBoardSquare((obj as ChessPiece).coordinate).isTargetable = isTargetable;
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

        GameManager.instance.ShowCard(card);
    }
    public void UseCardEffect()
    {
        UsingCard.EffectOnCardUsed.EffectAction();

        if (!(UsingCard is SoulCard))
            UsingCard.Destroy();
        UsingCard = null;
        isUsingCard = false;

        targetingEffect = null;

        GameManager.instance.HideCard();
    }
    /// <summary>
    ///     아직 구현 안됐습니다.
    /// </summary>
    public void Draw()
    {
        OnMyDraw?.Invoke();
    }
    public void TurnEnd()
    {
        OnMyTurnEnd?.Invoke();
    }
}
