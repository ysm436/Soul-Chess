using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulCardPieceRestriction : MonoBehaviour
{
    //기물 제한 아이콘에 미리 sprite들과 PieceType을 매핑
    [SerializeField]
    private List<Sprite> sprites;
    SpriteRenderer spriteRenderer;

    void Awake() { spriteRenderer = gameObject.GetComponent<SpriteRenderer>(); }

    public void SetIconSprite(ChessPiece.PieceType pieceType)
    {
        switch (pieceType)
        {
            case ChessPiece.PieceType.Pawn:
                spriteRenderer.sprite = sprites[0]; break;
            case ChessPiece.PieceType.Knight:
                spriteRenderer.sprite = sprites[1]; break;
            case ChessPiece.PieceType.Bishop:
                spriteRenderer.sprite = sprites[2]; break;
            case ChessPiece.PieceType.Rook:
                spriteRenderer.sprite = sprites[3]; break;
            case ChessPiece.PieceType.Quene:
                spriteRenderer.sprite = sprites[4]; break;
            case ChessPiece.PieceType.King:
                spriteRenderer.sprite = sprites[5]; break;
            default:
                spriteRenderer.sprite = null; break;
        }
    }
}
