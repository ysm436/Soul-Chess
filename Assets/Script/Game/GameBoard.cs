using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
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

    [Header("TestSetting")]
    [SerializeField] private RectTransform chessBoardBG;
    public int chessBoardSizeHeight;
    public int chessBoardSizeWidth;
    [SerializeField] private Transform chessPieceParent;
    [SerializeField] private List<GameObject> whiteChessPieceList;
    [SerializeField] private List<GameObject> blackChessPieceList;
    public bool soulSetCanMove;
    

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

    public bool isComputerTurn = false;

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

        //체스 판 크기 세팅
        gameData.BOARD_SIZE_HEIGHT = chessBoardSizeHeight;
        gameData.BOARD_SIZE_WIDTH = chessBoardSizeWidth;
        gameData.boardSquares = new BoardSquare[chessBoardSizeHeight, chessBoardSizeWidth];
        chessBoardBG.sizeDelta = new Vector2((float)(chessBoardSizeHeight + 0.4), (float)(chessBoardSizeWidth + 0.4));

        //체스 판 세팅
        chessBoard.SetBoardSquares(gameData);

        //흰색 체스 말 두기
        for (int h = 0; h < (whiteChessPieceList.Count() / chessBoardSizeWidth); h++) {
            for (int w = 0; w < chessBoardSizeWidth; w++) {
                GameObject pieceObj = Instantiate(whiteChessPieceList[h * chessBoardSizeWidth + w], chessPieceParent);
                ChessPiece pieceComponent = pieceObj.GetComponent<ChessPiece>();

                pieceComponent.coordinate = new Vector2Int(w, h);
                gameData.TryAddPiece(pieceComponent);

                pieceComponent.chessData = gameData;
                pieceComponent.transform.position = chessBoard.GetPositionUsingCoordinate(pieceComponent.coordinate);
            }
        }

        //검은색 체스 말 두기
        for (int h = 0; h < (blackChessPieceList.Count() / chessBoardSizeWidth); h++) {
            for (int w = 0; w < chessBoardSizeWidth; w++) {
                GameObject pieceObj = Instantiate(blackChessPieceList[h * chessBoardSizeWidth + w], chessPieceParent);
                ChessPiece pieceComponent = pieceObj.GetComponent<ChessPiece>();

                pieceComponent.coordinate = new Vector2Int(w, (chessBoardSizeHeight - 1) - h);
                gameData.TryAddPiece(pieceComponent);

                pieceComponent.chessData = gameData;
                pieceComponent.transform.position = chessBoard.GetPositionUsingCoordinate(pieceComponent.coordinate);
            }
        }

        //코스트 초기화(선공이 1, 후공이 0, 턴 종료 시 상대방 코스트 증가)
        gameData.playerBlack.soulOrbs = gameData.playerBlack.soulEssence = 0;
        gameData.playerWhite.soulOrbs = gameData.playerWhite.soulEssence = 1;

        gameData.myPlayerData.playerColor = myController.playerColor;
        gameData.opponentPlayerData.playerColor = opponentController.playerColor;

        synchronizedRandom.Init(GameManager.instance.isHost);
    }

    private void Start()
    {
        //GameManager.instance.soundManager.PlayBgm("GameScene");
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
        gameData.pieceObjects.Remove(targetPiece);
        
        yield return new WaitForSeconds(1.5f);
        if (targetPiece.pieceColor == PlayerColor.White)
        {
            gameData.whiteGraveyard.Add(targetPiece);
            myController.IncreaseGraveyard(PlayerColor.White);
            //targetPiece.transform.GetChild(0).gameObject.SetActive(false);
            //targetPiece.transform.GetChild(1).gameObject.SetActive(false);
            //targetPiece.coordinate = Vector2Int.right * (gameData.whiteGraveyard.Count - 1) + Vector2Int.down;
        }
        else
        {
            gameData.blackGraveyard.Add(targetPiece);
            myController.IncreaseGraveyard(PlayerColor.Black);
            //targetPiece.transform.GetChild(0).gameObject.SetActive(false);
            //targetPiece.transform.GetChild(1).gameObject.SetActive(false);
            //targetPiece.coordinate = Vector2Int.right * (gameData.blackGraveyard.Count - 1) + Vector2Int.up * gameData.BOARD_SIZE_HEIGHT;
        }

        targetPiece.gameObject.SetActive(false);
        //targetPiece.effectIcon.RemoveIcon();
        //targetPiece.DestroyMoveRestrictionIcon();
        //targetPiece.transform.position = chessBoard.GetPositionUsingCoordinate(targetPiece.coordinate);
    }

    Card showedCard = null;
    float cardSize = 1.5f;
    [SerializeField] private CardUI cardUI;
    public void ShowCard(Card card)
    {
        if (cardUI.gameObject.activeSelf)
            HideCard();

        //showedCard = Instantiate(card, cardBoard.position, Quaternion.identity);
        //showedCard.GetComponent<Collider2D>().enabled = false;
        //showedCard.GetComponent<SortingGroup>().sortingOrder = -1;
        //showedCard.transform.localScale = new Vector3(1f, 1f, 0f) * cardSize;

        //showedCard.FlipFront();

        cardUI.gameObject.SetActive(true);
        cardUI.SetCardUI(card);
    }

    public void HideCard()
    {
        if (!cardUI.gameObject.activeSelf)
            return;

        cardUI.gameObject.SetActive(false);
        //Destroy(showedCard.gameObject);
    }

    //기물 정보 표시
    ChessPiece showedPiece = null;
    public bool isShowingPieceInfo = false;
    public void ShowPieceInfo(ChessPiece piece)
    {
        if (isShowingPieceInfo)
            HidePieceInfo();

        showedPiece = piece;
        //showedPieceInfo = Instantiate(pieceInfo, cardBoard.position, Quaternion.identity);
        //showedPieceInfo.EditDescription(piece);
        isShowingPieceInfo = true;

        cardUI.gameObject.SetActive(true);

        if (piece.soul == null)
            cardUI.SetCardUI(piece);
        else
        {
            StartCoroutine(piece.SetFadeAccessory(true));
            cardUI.SetCardUI(piece.soul);
        }
    }
    public void HidePieceInfo()
    {
        if (!isShowingPieceInfo)
            return;

        StartCoroutine(showedPiece.SetFadeAccessory(false));
        showedPiece = null;
        //Destroy(showedPieceInfo.gameObject);
        isShowingPieceInfo = false;

        cardUI.gameObject.SetActive(false);
    }

    [SerializeField] private GameObject explainUI;    
    public void ShowExplainUI(string explain)
    {
        explainUI.GetComponentInChildren<TextMeshProUGUI>().text = explain;
        explainUI.SetActive(true);
    }
    public void HideExplainUI()
    {
        explainUI.SetActive(false);
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

    public PlayerData ComputerData()
    {
        if (isWhiteTurn == isComputerTurn)
            return gameData.playerWhite;
        else
            return gameData.playerBlack;
    }

    public void OnGameOver(ChessPiece killedKing)
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
