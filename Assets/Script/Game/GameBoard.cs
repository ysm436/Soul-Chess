using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

// TODO: PlayerColor GameData로 옮기기

public class GameBoard : MonoBehaviour
{
    public static GameBoard instance = null;

    [Header("DevOption")]
    public bool isDebugMode;
    public SynchronizedRandom synchronizedRandom;


    [HideInInspector]
    public GameData gameData;

    [Header("GameData")]
    public GameBoard.PlayerColor playerColor;
    public ChessBoard chessBoard;
    public GameObject cardBoard;
    public PieceInfo pieceInfo; //기물 정보 프리팹

    public GameOverUI gameOverUI;
    public PlayerController whiteController;
    public PlayerController blackController;

    public PlayerController myController { get => playerColor == PlayerColor.White ? whiteController : blackController; }
    public PlayerController opponentController { get => playerColor == PlayerColor.White ? blackController : whiteController; }

    public Action<ChessPiece> OnPieceKilled;

    public bool isActivePlayer
    {
        get => myController.enabled;
    }

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

        //플레이어 색상(선공) 지정
        if (GameManager.instance.isHost)
            playerColor = PlayerColor.White;
        else
            playerColor = PlayerColor.Black;

        //덱 초기화
        //gameData.myPlayerData.deck = GameManager.instance.currentDeck;

        //체스 판 세팅
        chessBoard.SetBoardSquares(gameData);

        //체스 말 두기
        foreach (ChessPiece piece in chessBoard.GetComponentsInChildren<ChessPiece>())
        {
            gameData.TryAddPiece(piece);

            piece.chessData = gameData;
            chessBoard.SetPiecePositionByCoordinate(piece);
        }


        //코스트 초기화(선공이 1, 후공이 0, 턴 종료 시 상대방 코스트 증가)
        gameData.playerBlack.soulOrbs = gameData.playerBlack.soulEssence = 3;
        gameData.playerWhite.soulOrbs = gameData.playerWhite.soulEssence = 4;

        gameData.myPlayerData.playerColor = myController.playerColor;
        gameData.opponentPlayerData.playerColor = opponentController.playerColor;

        foreach (ChessPiece king in gameData.pieceObjects.Where(piece => piece.pieceType == ChessPiece.PieceType.King))
            king.OnKilled += OnGameOver;

        synchronizedRandom.Init(GameManager.instance.isHost);
    }

    private void Start()
    {
        GameManager.instance.soundManager.PlayBgm("GameScene");
    }
    public BoardSquare GetBoardSquare(Vector2Int coordinate)
    {
        return gameData.boardSquares[coordinate.x, coordinate.y];
    }
    public void KillPiece(ChessPiece targetPiece)
    {
        OnPieceKilled?.Invoke(targetPiece);

        if (targetPiece.pieceColor == PlayerColor.White)
        {
            gameData.whiteGraveyard.Add(targetPiece);
            targetPiece.coordinate = Vector2Int.right * (gameData.whiteGraveyard.Count - 1) + Vector2Int.down;
        }
        else
        {
            gameData.blackGraveyard.Add(targetPiece);
            targetPiece.coordinate = Vector2Int.right * (gameData.blackGraveyard.Count - 1) + Vector2Int.up * GameData.BOARD_SIZE;
        }

        targetPiece.effectIcon.RemoveIcon();
        targetPiece.DestroyMoveRestrictionIcon();
        gameData.pieceObjects.Remove(targetPiece);

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

        showedCard.FlipFront();
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
        if (isActivePlayer)
            return playerColor == PlayerColor.White ? gameData.playerWhite : gameData.playerBlack;
        else
            return playerColor == PlayerColor.White ? gameData.playerBlack : gameData.playerWhite;
    }
    public PlayerController CurrentPlayerController()
    {
        if (isActivePlayer)
            return myController;
        else
            return opponentController;
    }

    private void OnGameOver(ChessPiece killedKing)
    {
        if (killedKing.pieceColor == playerColor)
            gameOverUI.OnDefeated();
        else
            gameOverUI.OnWin();
    }

    [System.Serializable]
    public enum PlayerColor
    {
        White, Black
    }
}
