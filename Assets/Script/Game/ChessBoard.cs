using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessBoard : MonoBehaviour
{
    public Vector2 basePosition;
    public GameObject boardSquareSample;


    public List<Sprite> blackBoardSquareSprites = new List<Sprite>();
    public List<Sprite> whiteBoardSquareSprites = new List<Sprite>();


    public void SetBoardSquares(GameData gameData)
    {
        for (int i = 0; i < GameData.BOARD_SIZE; i++)
        {
            for (int j = 0; j < GameData.BOARD_SIZE; j++)
            {
                gameData.boardSquares[j, i] = Instantiate<GameObject>(boardSquareSample, new Vector2(j, i) + basePosition, Quaternion.identity, transform).GetComponent<BoardSquare>();

                if ((i + j) % 2 == 0)
                    gameData.boardSquares[j, i].SetBoardSquare(j, i, blackBoardSquareSprites[Random.Range(0, blackBoardSquareSprites.Count)]);
                else
                    gameData.boardSquares[j, i].SetBoardSquare(j, i, whiteBoardSquareSprites[Random.Range(0, whiteBoardSquareSprites.Count)]);
            }
        }
    }

    public void SetPiecePositionByCoordinate(ChessPiece chessPiece)
    {
        chessPiece.transform.position = (Vector2)chessPiece.coordinate + basePosition;
    }
}