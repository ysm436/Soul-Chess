using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using Unity.Collections;

public class BoardSquare : MonoBehaviour
{
    static readonly Color movableColor = Color.yellow;

    public Vector2Int coordinate;

    public bool ismMovable
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
