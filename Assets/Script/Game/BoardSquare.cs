using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using Unity.Collections;
using UnityEngine.EventSystems;

public class BoardSquare : TargetableObject, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public BoardSquareOutline outline; //외곽선 프리팹

    public bool isMovable
    {
        set
        {
            if (value)
            {
                outline.changeOutline(BoardSquareOutline.TargetableStates.movable);
            }
            else
            {
                outline.changeOutline(BoardSquareOutline.TargetableStates.none);
            }
        }
    }
    public bool isNegativeTargetable //부정적 효과 타겟은 빨간색 외곽선
    {
        set
        {
            if (value)
            {
                outline.changeOutline(BoardSquareOutline.TargetableStates.negative);
            }
            else
            {
                outline.changeOutline(BoardSquareOutline.TargetableStates.none);
            }
        }
    }
    public bool isPositiveTargetable //긍정적 효과 타겟은 초록색 외곽선
    {
        set
        {
            if (value)
            {
                outline.changeOutline(BoardSquareOutline.TargetableStates.positive);
            }
            else
            {
                outline.changeOutline(BoardSquareOutline.TargetableStates.none);
            }
        }
    }

    public Action<Vector2Int> OnClick
    {
        set
        {
            _onClick = null;
            _onClick += value;
        }
    }
    Action<Vector2Int> _onClick;
    SpriteRenderer spriteRenderer;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        _onClick.Invoke(coordinate);
        if (GameBoard.instance.gameData.GetPiece(coordinate))
            GameBoard.instance.ShowPieceInfo(GameBoard.instance.gameData.GetPiece(coordinate));
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        //카드 사용 중인지 체크해서 그 때는 기물 정보 표시 X
        if (GameBoard.instance.gameData.GetPiece(coordinate) && (!GameBoard.instance.CurrentPlayerController().isUsingCard))
            GameBoard.instance.ShowPieceInfo(GameBoard.instance.gameData.GetPiece(coordinate));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (GameBoard.instance.isShowingPieceInfo)
            GameBoard.instance.HidePieceInfo();
    }

    public BoardSquare(Vector2Int coordinate, Sprite sprite)
    {
        this.spriteRenderer.sprite = sprite;
        this.coordinate = coordinate;
    }
    public void SetBoardSquare(int x, int y, Sprite sprite)
    {
        this.coordinate = new Vector2Int(x, y);
        this.spriteRenderer.sprite = sprite;
    }
}
