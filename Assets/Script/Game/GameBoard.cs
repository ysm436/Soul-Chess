using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

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
    public Transform cardBoard;
    public SpriteRenderer myHand;
    public SpriteRenderer trashCan;
    public CancelButton cancelButton;
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

    public bool isWhiteTurn = true;

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
        {
            playerColor = PlayerColor.White;
            if (SceneManager.GetActiveScene().name == "PvEGameScene")
                blackController.GetComponent<PvEPlayerController>().isComputer = true;
        }
        else
        {
            playerColor = PlayerColor.Black;
            if (SceneManager.GetActiveScene().name == "PvEGameScene")
                whiteController.GetComponent<PvEPlayerController>().isComputer = true;
        }

        //덱 초기화
        //gameData.myPlayerData.deck = GameManager.instance.currentDeck;

        //체스 판 세팅
        chessBoard.SetBoardSquares(gameData);

        //체스 말 두기
        foreach (ChessPiece piece in chessBoard.GetComponentsInChildren<ChessPiece>())
        {
            gameData.TryAddPiece(piece);

            piece.chessData = gameData;
            piece.transform.position = chessBoard.GetPositionUsingCoordinate(piece.coordinate);
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
    public bool isCardUsed(Vector3 cardPosition)
    {
        return Mathf.Abs(cardPosition.x - myHand.transform.position.x) > myHand.bounds.size.x / 2
            || Mathf.Abs(cardPosition.y - myHand.transform.position.y) > myHand.bounds.size.y / 2;
    }
    public bool isCardDiscarded(Vector3 cardPosition)
    {
        return Mathf.Sqrt(Mathf.Pow(cardPosition.x - trashCan.transform.position.x, 2) + Mathf.Pow(cardPosition.y - trashCan.transform.position.y, 2)) < trashCan.bounds.size.x / 2;
    }
    public BoardSquare GetBoardSquare(Vector2Int coordinate)
    {
        return gameData.boardSquares[coordinate.x, coordinate.y];
    }
    public void KillPiece(ChessPiece targetPiece)
    {
        OnPieceKilled?.Invoke(targetPiece); // 이거 의미 없는 것 같은데..
        StartCoroutine(KillPieceAnimationC(targetPiece));
    }

    IEnumerator KillPieceAnimationC(ChessPiece targetPiece)
    {
        yield return new WaitForSeconds(1.5f);
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

        targetPiece.transform.position = chessBoard.GetPositionUsingCoordinate(targetPiece.coordinate);
    }

    Card showedCard = null;
    float cardSize = 1.5f;
    public void ShowCard(Card card)
    {
        if (showedCard != null)
            HideCard();

        showedCard = Instantiate(card, cardBoard.position, Quaternion.identity);
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

        showedPieceInfo = Instantiate(pieceInfo, cardBoard.position, Quaternion.identity);
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
        {
            if (SceneManager.GetActiveScene().name == "TutorialScene")
            {
                var tutorialManager = FindObjectOfType<TutorialManager>();
                tutorialManager.gameOverUI.OnWin();
                return;
            }
            gameOverUI.OnWin();
        }
    }

    [System.Serializable]
    public enum PlayerColor
    {
        White, Black
    }
}
