using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessBoard : MonoBehaviour
{
    [SerializeField]
    public ChessData chessData;
    [SerializeField]
    ChessPiece.PieceColor bottomPlayerColor;

    public Vector2 basePosition;

    public GameObject boardSquareSample;
    public List<Sprite> blackBoardSquareSprites = new List<Sprite>();
    public List<Sprite> whiteBoardSquareSprites = new List<Sprite>();


    private void Awake()
    {
        foreach (ChessPiece piece in GetComponentsInChildren<ChessPiece>())
        {
            chessData.TryAddPiece(piece);

            piece.chessData = chessData;
            SetPiecePositionByCoordinate(piece);
        }

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                chessData.boardSquares[j, i] = Instantiate<GameObject>(boardSquareSample, new Vector2(j, i) + basePosition, Quaternion.identity, transform).GetComponent<BoardSquare>();

                if ((i + j) % 2 == 0)
                    chessData.boardSquares[j, i].SetBoardSquare(j, i, blackBoardSquareSprites[Random.Range(0, blackBoardSquareSprites.Count)]);
                else
                    chessData.boardSquares[j, i].SetBoardSquare(j, i, whiteBoardSquareSprites[Random.Range(0, whiteBoardSquareSprites.Count)]);
            }
        }
    }

    public BoardSquare GetBoardSquare(Vector2Int coordinate)
    {
        return chessData.boardSquares[coordinate.x, coordinate.y];
    }

    public void SetPiecePositionByCoordinate(ChessPiece chessPiece)
    {
        chessPiece.transform.position = (Vector2)chessPiece.coordinate + basePosition;
    }
    public void KillPiece(ChessPiece targetPiece)
    {
        chessData.graveyard.Add(targetPiece);
        chessData.pieceObjects.Remove(targetPiece);

        targetPiece.coordinate = Vector2Int.right * (chessData.graveyard.Count - 1) + Vector2Int.down;
        SetPiecePositionByCoordinate(targetPiece);
    }

}
