using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using Unity.Collections;

public class BoardSquare : MonoBehaviour
{
    static readonly Color movableColor = Color.red;
    static readonly Color targetableColor = Color.red;

    public Vector2Int coordinate;

    public bool isMovable
    {
        set
        {
            if (value)
            {
                spriteRenderer.color = movableColor;
            }
            else
            {
                spriteRenderer.color = Color.white;
            }
        }
    }
    public bool isTargetable
    {
        set
        {
            if (value)
            {
                spriteRenderer.color = targetableColor;
            }
            else
            {
                spriteRenderer.color = Color.white;
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
    private void OnMouseUp()
    {
        _onClick.Invoke(coordinate);
        if (GameBoard.instance.gameData.GetPiece(coordinate))
            GameBoard.instance.ShowPieceInfo(GameBoard.instance.gameData.GetPiece(coordinate));
    }
    private void OnMouseEnter()
    {
        //카드 사용 중인지 체크해서 그 때는 기물 정보 표시 X
        //BlackController 없어서 검정 턴에는 기물 위에 올리면 오류
        if (GameBoard.instance.gameData.GetPiece(coordinate) && (!GameBoard.instance.CurrentPlayerController().isUsingCard))
            GameBoard.instance.ShowPieceInfo(GameBoard.instance.gameData.GetPiece(coordinate));
    }
    private void OnMouseExit()
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
