using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

// TODO: PlayerColor GameData로 옮기기

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    [HideInInspector]
    public GameData gameData;
    public GameManager.PlayerColor playerColor;
    public ChessBoard chessBoard;
    public GameObject cardBoard;

    public PlayerController whiteController;

    private void Awake()
    {

        //singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }


        foreach (ChessPiece piece in chessBoard.GetComponentsInChildren<ChessPiece>())
        {
            gameData.TryAddPiece(piece);

            piece.chessData = gameData;
            chessBoard.SetPiecePositionByCoordinate(piece);
        }

        chessBoard.SetBoardSquares(gameData);
    }

    public BoardSquare GetBoardSquare(Vector2Int coordinate)
    {
        return gameData.boardSquares[coordinate.x, coordinate.y];
    }
    public void KillPiece(ChessPiece targetPiece)
    {
        gameData.graveyard.Add(targetPiece);
        gameData.pieceObjects.Remove(targetPiece);

        targetPiece.coordinate = Vector2Int.right * (gameData.graveyard.Count - 1) + Vector2Int.down;
        chessBoard.SetPiecePositionByCoordinate(targetPiece);
    }

    Card showedCard = null;
    float cardSize = 1.5f;
    public void ShowCard(Card card)
    {
        if (showedCard != null)
            HideCard();

        showedCard = Instantiate(card, cardBoard.transform.position, Quaternion.identity);
        showedCard.GetComponent<Collider2D>().enabled = false;
        showedCard.GetComponent<SortingGroup>().sortingOrder = -1;
        showedCard.transform.localScale = new Vector3(1f, 1f, 0f) * cardSize;
    }
    public void HideCard()
    {
        if (showedCard == null)
            return;

        Destroy(showedCard.gameObject);
    }

    [System.Serializable]
    public enum PlayerColor
    {
        White, Black
    }
}
