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
    public PieceInfo pieceInfo; //기물 정보 프리팹

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

        //코스트 초기화(선공이 1, 후공이 0, 턴 종료 시 상대방 코스트 증가)
        gameData.playerBlack.soulOrbs = gameData.playerBlack.soulEssence = 0;
        gameData.playerWhite.soulOrbs = gameData.playerWhite.soulEssence = 1;
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

    //기물 정보 표시
    PieceInfo showedPieceInfo = null;
    public bool isShowingPieceInfo = false;
    public void ShowPieceInfo(ChessPiece piece)
    {
        if (showedPieceInfo != null)
            HidePieceInfo();

        showedPieceInfo = Instantiate(pieceInfo, cardBoard.transform.position, Quaternion.identity);
        showedPieceInfo.EditDescription(piece);
        isShowingPieceInfo = true;
    }
    public void HidePieceInfo()
    {
        if (showedPieceInfo == null)
            return;

        Destroy(showedPieceInfo.gameObject);
        isShowingPieceInfo = false;
    }

    //현재 턴 진행 중인 Enabled 플레이어 데이터 접근
    public PlayerData CurrentPlayerData()
    {
        if (whiteController.enabled) return gameData.playerWhite;
        else return gameData.playerBlack;
    }
    public PlayerController CurrentPlayerController()
    {
        if (whiteController.enabled) return whiteController;
        else return null; //blackController
    }

    [System.Serializable]
    public enum PlayerColor
    {
        White, Black
    }
}
