using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static GameBoard;

public class TutorialManager : MonoBehaviour
{
    public CutoutMaskUI cutoutMaskUI;
    public Transform myDeckTransform;
    public Transform myHandTransform;
    public Transform opponentDeckTransform;
    public Transform opponentHandTransform;

    public TutorialController tutorialController;

    private PlayerData player;
    readonly float CARD_DISTANCE_IN_HAND = 0.5f;

    private Card usingCard = null;
    private TargetingEffect targetingEffect;
    public bool isUsingCard = false;
    private bool isInfusing = false;
    ChessPiece chosenPiece = null;
    List<Vector2Int> movableCoordinates = new List<Vector2Int>();
    List<TargetableObject> targetableObjects = new List<TargetableObject>();

    public bool isMoved;

    // Tutorial
    private bool isInitalized;

    public PlayerController playerController;
    private PlayerColor playerColor { get => playerController.playerColor; }

    private int step;

    private int shadowStep;

    private bool isFirstSoulCardUsed;

    private int cardClickedCount;

    private Vector2 textAnchorMin;
    private Vector2 textAnchorMax;

    public RectTransform transparent;
    public GameObject shadow;
    public GameObject blockCardUse;
    public RectTransform arrow;

    public RectTransform textTransform;
    public TextMeshProUGUI descriptionText;
    public GameObject nextButton;

    public TutorialGameOverUI gameOverUI;

    public Card viking;
    public Card bard;

    private Card ShowingCard;

    private bool isCardClicked;

    public bool isAllowingTurnEnd;

    // Showing Card Info Anchors
    public List<Vector2> cardMinAnchors;
    public List<Vector2> cardMaxAnchors;

    private void Awake()
    {
        isInitalized = false;
        cutoutMaskUI.SetToCanvasSize();
        step = 0;
        shadowStep = 0;
        isFirstSoulCardUsed = false;
        cardClickedCount = 0;
        isCardClicked = false;
        isAllowingTurnEnd = false;
    }

    private void Start()
    {

    }

    private void Update()
    {
        if (!isInitalized)
        {
            blockCardUse.SetActive(true);
            InitializeMyCards();
            InitializeOpponentCards();
            SetShadow(4, 1);

            foreach (var s in GameBoard.instance.gameData.boardSquares)
            {
                if (s.coordinate == new Vector2Int(4, 1))
                {
                    s.OnClick = ProcessStep1;
                }
                else if (s.coordinate == new Vector2Int(4, 3))
                {
                    s.OnClick = ProcessStep2;
                }
                else
                {
                    s.OnClick = DoNothing;
                }
            }

            foreach (ChessPiece king in GameBoard.instance.gameData.pieceObjects.Where(piece => piece.pieceType == ChessPiece.PieceType.King))
                king.OnKilled += OnEndTutorial;

            isInitalized = true;
        }
    }

    private bool IsClickingTouchable()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D[] rayCastHits = Physics2D.RaycastAll(ray.origin, ray.direction);

        foreach (RaycastHit2D raycastHit in rayCastHits)
        {
            if (raycastHit.transform.name == "Touchable")
            {
                Debug.Log(raycastHit.transform.name);
            }
        }
        return false;
    }

    private void InitializeMyCards()
    {
        player = GameBoard.instance.gameData.myPlayerData;

        var deckAnchor = myDeckTransform;
        var handAnchor = myHandTransform;
        foreach (Card card in GameManager.instance.GetCardListFrom(GetIntList(16, 46, 13)))
        {
            card.owner = GameBoard.instance.gameData.myPlayerData;
            var instantiatedCard = Instantiate(card, deckAnchor);
            instantiatedCard.FlipBack();
            player.deck.Add(instantiatedCard);
            instantiatedCard.GetComponent<SortingGroup>().sortingOrder = -1;
            var check = instantiatedCard.AddComponent<CheckCardClicked>();
            check.tutorialManager = this;
        }

        player.deckPosition = new Vector2(7.6f, -2.3f); //UI에 맞게 좌표수정
    }

    private void InitializeOpponentCards()
    {
        var deckAnchor = opponentDeckTransform;

        foreach (Card card in GameManager.instance.GetCardListFrom(GetIntList(1, 13, 1, 14)))
        {
            card.owner = GameBoard.instance.gameData.opponentPlayerData;
            var instantiatedCard = Instantiate(card, deckAnchor);
            instantiatedCard.FlipBack();
            GameBoard.instance.gameData.opponentPlayerData.deck.Add(instantiatedCard);
            instantiatedCard.GetComponent<SortingGroup>().sortingOrder = -1;
        }
    }

    private List<int> GetIntList(params int[] index)
    {
        return new List<int>(index);
    }

    private void SetShadow(int x, int y, bool isTurnButton = false,
        bool isCost = false, bool isCard = false, bool isSpecific = false,
        params Vector2[] anchors)
    {
        float minX = 0f, minY = 0f, maxX = 0f, maxY = 0f;
        if (x != -1 && y != -1)
        {
            float startX = 0.355f, startY = 0.107f;
            float distX = 0.056f, distY = 0.1f;

            startX += distX * x;
            startY += distY * y;

            float endX = startX + distX;
            float endY = startY + distY;

            minX = startX;
            minY = startY;
            maxX = endX;
            maxY = endY;

            arrow.anchoredPosition = new Vector2(0f, 62f);
        }
        else if (isTurnButton)
        {
            minX = 0.895f;
            minY = 0.45f;
            maxX = 0.99f;
            maxY = 0.55f;
        }
        else if (isCost)
        {
            minX = 0.82f;
            minY = 0.37f;
            maxX = 0.9f;
            maxY = 0.44f;
        }
        else if (isCard)
        {
            minX = 0.1f;
            minY = 0f;
            maxX = 0.21f;
            maxY = 0.23f;

            arrow.anchoredPosition = new Vector2(0f, 92f);
        }
        else if (isSpecific)
        {
            minX = anchors[0].x;
            minY = anchors[0].y;
            maxX = anchors[1].x;
            maxY = anchors[1].y;

            arrow.anchoredPosition = new Vector2(0f, 62f);
        }
        transparent.anchorMin = new Vector2(minX, minY);
        transparent.anchorMax = new Vector2(maxX, maxY);
        transparent.offsetMin = Vector2.zero;
        transparent.offsetMax = Vector2.zero;

    }

    public void RemoveShadow()
    {
        shadow.SetActive(false);
        RemoveArrow();
    }

    public void EnableShadow()
    {
        shadow.SetActive(true);
        EnableArrow();
    }

    public void CardClicked()
    {
        if (cardClickedCount == 0)
        {
            if (step != 9) return;
            ProcessStep10();
            cardClickedCount++;
        }
        else if (cardClickedCount == 1)
        {
            if (step != 18) return;
            ProcessStep19();
            cardClickedCount++;
        }
        else if (cardClickedCount == 2)
        {
            if (step != 25) return;
            ProcessStep26();
            cardClickedCount++;
        }

    }

    private void ProcessStep1(Vector2Int coordinate)
    {
        foreach (var s in GameBoard.instance.gameData.boardSquares)
        {
            if (s.coordinate == coordinate)
            {
                s.OnClick = DoNothing;
            }
        }
        SetShadow(4, 3);

        descriptionText.text =
            "자신의 기물을 선택 시\n" +
            "이동할 수 있는 범위가 나타납니다.\n" +
            "타일을 클릭해 기물을 이동하세요.";
        SetTextSize(3);

        OnClickBoardSquare(coordinate);
    }

    private void ProcessStep2(Vector2Int coordinate)
    {
        foreach (var s in GameBoard.instance.gameData.boardSquares)
        {
            if (s.coordinate == coordinate)
            {
                s.OnClick = DoNothing;
            }
        }
        SetShadow(-1, -1, isTurnButton: true);

        descriptionText.text =
            "기물을 이동하여\n" +
            "턴 종료 버튼이 활성화되었습니다.\n" +
            "턴 종료 버튼을 눌러\n" +
            "당신의 턴을 끝내세요.";
        SetTextSize(4);

        OnClickBoardSquare(coordinate);

        if (GameBoard.instance.isActivePlayer)
        {
            tutorialController.EnableTurnChangeButton();
        }
    }

    public void ProcessStep3()
    {
        EnableShadow();
        SetShadow(-1, -1, isCost: true);

        ShowText();
        descriptionText.text =
            "상대방의 턴이 종료되었습니다.\n" +
            "내 턴 시작 시 내 덱에서\n" +
            "카드를 1장 뽑아 손패로 가져오고,\n" +
            "영혼 정수 최대치가\n" +
            "1 늘어나며 충전됩니다.\n" +
            "영혼 정수는\n" +
            "카드를 사용하는데 소모됩니다.";
        SetTextSize(7);
        nextButton.SetActive(true);
        nextButton.GetComponent<Button>().onClick.RemoveAllListeners();
        nextButton.GetComponent<Button>().onClick.AddListener(ProcessStep4);
    }

    private void ProcessStep4()
    {
        SetShadow(-1, -1, isCard: true);

        descriptionText.text =
            "카드의 정보를 보려면\n" +
            "마우스를 카드 위에 올리세요.";
        SetTextSize(2);

        textTransform.anchorMin = new Vector2(0.5f, 0.5f);
        textTransform.anchorMax = new Vector2(0.5f, 0.5f);
        textTransform.anchoredPosition = Vector2.zero;

        nextButton.GetComponent<Button>().onClick.RemoveAllListeners();
        nextButton.GetComponent<Button>().onClick.AddListener(ProcessStep5);
    }

    private void ProcessStep5()
    {
        // 전체 카드 강조
        /*Vector2[] anchors = new Vector2[2];
        anchors[0] = new Vector2(0.08f, 0.25f);
        anchors[1] = new Vector2(0.27f, 0.73f);
        SetShadow(-1, -1, isSpecific: true, anchors: anchors);*/

        Vector2[] anchors = new Vector2[2];
        anchors[0] = new Vector2(0.215f, 0.66f);
        anchors[1] = new Vector2(0.262f, 0.75f);
        SetShadow(-1, -1, isSpecific: true, anchors: anchors);

        ShowCard(viking);
        descriptionText.text =
            "이 카드는 영혼 카드입니다.\n" +
            "영혼 카드를 사용해\n" +
            "영혼을 부여하면\n" +
            "아군 기물을 강화할 수 있습니다.";
        SetTextSize(4);

        nextButton.GetComponent<Button>().onClick.RemoveAllListeners();
        nextButton.GetComponent<Button>().onClick.AddListener(ProcessStep6);
    }

    private void ProcessStep6()
    {
        Vector2[] anchors = new Vector2[2];
        anchors[0] = new Vector2(0.085f, 0.66f);
        anchors[1] = new Vector2(0.14f, 0.75f);
        SetShadow(-1, -1, isSpecific: true, anchors: anchors);

        descriptionText.text =
            "비용: 이 카드를 사용하기 위해\n" +
            "필요한 영혼 정수의 양입니다.";
        SetTextSize(2);

        nextButton.GetComponent<Button>().onClick.RemoveAllListeners();
        nextButton.GetComponent<Button>().onClick.AddListener(ProcessStep7);
    }

    private void ProcessStep7()
    {
        Vector2[] anchors = new Vector2[2];
        anchors[0] = new Vector2(0.080f, 0.22f);
        anchors[1] = new Vector2(0.13f, 0.32f);
        SetShadow(-1, -1, isSpecific: true, anchors: anchors);

        descriptionText.text =
            "공격력: 이 영혼이 부여될 때\n" +
            "추가되는 공격력을 의미합니다.\n" +
            "다른 기물을 공격할 때\n" +
            "공격력만큼 피해를 줍니다.";
        SetTextSize(4);

        nextButton.GetComponent<Button>().onClick.RemoveAllListeners();
        nextButton.GetComponent<Button>().onClick.AddListener(ProcessStep8);
    }

    private void ProcessStep8()
    {
        Vector2[] anchors = new Vector2[2];
        anchors[0] = new Vector2(0.215f, 0.22f);
        anchors[1] = new Vector2(0.267f, 0.32f);
        SetShadow(-1, -1, isSpecific: true, anchors: anchors);

        descriptionText.text =
            "체력: 이 영혼이 부여될 때\n" +
            "추가되는 체력을 의미합니다.\n" +
            "기물의 체력이 0이 될 경우\n" +
            "해당 기물은 파괴됩니다.";
        SetTextSize(4);

        nextButton.GetComponent<Button>().onClick.RemoveAllListeners();
        nextButton.GetComponent<Button>().onClick.AddListener(ProcessStep9);
    }

    private void ProcessStep9()
    {
        blockCardUse.SetActive(false);
        step = 9;

        SetShadow(-1, -1, isCard: true);

        RemoveShowCard();
        descriptionText.text =
            "카드를 클릭해\n" +
            "사용할 수 있습니다.";
        SetTextSize(2);
        nextButton.gameObject.SetActive(false);
    }

    private void ProcessStep10()
    {
        blockCardUse.SetActive(true);
        GameObject.Find("CancelButton").SetActive(false);

        SetShadow(4, 3);
        descriptionText.text =
            "영혼 카드 사용 시,\n" +
            "기물을 눌러 영혼을\n" +
            "부여할 수 있습니다.\n" +
            "기물을 눌러 영혼을 부여하세요.";
        SetTextSize(4);

        textTransform.anchorMin = new Vector2(0.677f, 0.78f);
        textTransform.anchorMax = new Vector2(0.677f, 0.78f);
        textTransform.anchoredPosition = Vector2.zero;

        foreach (var s in GameBoard.instance.gameData.boardSquares)
        {
            if (s.coordinate == new Vector2Int(4, 3))
            {
                s.OnClick = ProcessStep11;
            }
            else
            {
                s.OnClick = DoNothing;
            }
        }
    }

    private void ProcessStep11(Vector2Int coordinate)
    {
        SetShadow(5, 1);

        foreach (var s in GameBoard.instance.gameData.boardSquares)
        {
            if (s.coordinate == coordinate)
            {
                s.OnClick = DoNothing;
            }
        }
        descriptionText.text =
            "당신의 기물에\n" +
            "영혼을 부여했습니다.\n" +
            "해당 기물은 공격력이 2,\n" +
            "체력이 4 증가했습니다.\n" +
            "다른 기물을 선택해 이동시키세요.";
        SetTextSize(6);
        isMoved = false;
        //OnClickBoardSquare(coordinate);

        ClearMovableCoordniates();

        // Set Active false Viking
        VikingWarrior[] vikings = FindObjectsOfType<VikingWarrior>();

        for (int i = 0; i < vikings.Length; i++)
        {
            if (vikings[i].transform.parent == null)
            {
                vikings[i].gameObject.SetActive(false);
            }
        }

        SoulCard vikingInstance = Instantiate(viking) as SoulCard;

        vikingInstance.Infuse(GameBoard.instance.gameData.GetPiece(coordinate));

        ProcessStep12();
    }

    private void ProcessStep12()
    {
        foreach (var s in GameBoard.instance.gameData.boardSquares)
        {
            if (s.coordinate == new Vector2Int(5, 1))
            {
                s.OnClick = ProcessStep12_1;
            }
            else if (s.coordinate == new Vector2Int(5, 3))
            {
                s.OnClick = ProcessStep13;
            }
            else
            {
                s.OnClick = DoNothing;
            }
        }
    }

    private void ProcessStep12_1(Vector2Int coordinate)
    {
        SetShadow(5, 3);

        OnClickBoardSquare(coordinate);
    }

    private void ProcessStep13(Vector2Int coordinate)
    {
        SetShadow(-1, -1, isTurnButton: true);

        foreach (var s in GameBoard.instance.gameData.boardSquares)
        {
            if (s.coordinate == coordinate)
            {
                s.OnClick = DoNothing;
            }
        }
        OnClickBoardSquare(coordinate);

        if (GameBoard.instance.isActivePlayer)
        {
            tutorialController.EnableTurnChangeButton();
        }

        descriptionText.text =
            "기물을 이동하여 턴 종료 버튼이\n" +
            "활성화되었습니다.\n" +
            "턴 종료 버튼을 눌러\n" +
            "당신의 턴을 끝내세요.";
        SetTextSize(4);
    }

    public void ProcessStep14()
    {
        EnableShadow();
        SetShadow(5, 3);

        ShowText();

        descriptionText.text =
            "아군 기물이 적 기물에게 공격 받아\n" +
            "처치 당했습니다.";
        SetTextSize(2);

        nextButton.gameObject.SetActive(true);
        nextButton.GetComponent<Button>().onClick.RemoveAllListeners();
        nextButton.GetComponent<Button>().onClick.AddListener(ProcessStep15);
    }


    private void ProcessStep15()
    {
        RemoveText();

        tutorialController.StartEnemySecondRoutine2();
        nextButton.gameObject.SetActive(false);
    }

    public void ProcessStep16()
    {
        EnableShadow();

        SetShadow(5, 3);

        ShowText();
        descriptionText.text =
            "적이 자신의 기물에\n" +
            "영혼을 부여했습니다.\n" +
            "체력이 높아 당장은 처치하기\n" +
            "어려울 듯 합니다.";
        SetTextSize(4);
        nextButton.gameObject.SetActive(true);
        nextButton.GetComponent<Button>().onClick.RemoveAllListeners();
        nextButton.GetComponent<Button>().onClick.AddListener(tutorialController.OnTurnEndClicked);
    }

    public void ProcessStep17()
    {
        UpdateMyHandPosition();

        SetShadow(5, 0);

        descriptionText.text =
            "기물을 이동시키세요.";
        SetTextSize(1);
        nextButton.gameObject.SetActive(false);

        isMoved = false;

        foreach (var s in GameBoard.instance.gameData.boardSquares)
        {
            if (s.coordinate == new Vector2Int(5, 0))
            {
                s.OnClick = ProcessStep17_1;
            }
            else if (s.coordinate == new Vector2Int(1, 4))
            {
                s.OnClick = ProcessStep18;
            }
            else
            {
                s.OnClick = DoNothing;
            }
        }
    }

    private void ProcessStep17_1(Vector2Int coordinate)
    {
        SetShadow(1, 4);

        OnClickBoardSquare(coordinate);
    }

    private void ProcessStep18(Vector2Int coordinate)
    {
        blockCardUse.SetActive(false);
        SetShadow(-1, -1, isCard: true);

        foreach (var s in GameBoard.instance.gameData.boardSquares)
        {
            if (s.coordinate == coordinate)
            {
                s.OnClick = DoNothing;
            }
        }
        var whitePlayer = FindObjectOfType<PlayerController>();
        whitePlayer.isUsingCard = false;
        step = 18;
        OnClickBoardSquare(coordinate);
        descriptionText.text =
            "'음치 음유시인' 카드를 사용하세요.\n";
        SetTextSize(1);
        foreach (var s in GameBoard.instance.gameData.boardSquares)
        {
            s.OnClick = DoNothing;
        }
    }

    private void ProcessStep19()
    {
        blockCardUse.SetActive(true);
        SetShadow(1, 4);

        ShowCard(bard);
        descriptionText.text =
            "기물에 영혼을 부여하세요.\n";
        SetTextSize(1);

        foreach (var s in GameBoard.instance.gameData.boardSquares)
        {
            if (s.coordinate == new Vector2Int(1, 4))
            {
                s.OnClick = ProcessStep20;
            }
            else
            {
                s.OnClick = DoNothing;
            }
        }
    }

    private void ProcessStep20(Vector2Int coordinate)
    {
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                Vector2Int pos = new Vector2Int(i, j);

                ChessPiece currentPiece = GameBoard.instance.gameData.GetPiece(pos);

                if (currentPiece == null) continue;

                if (currentPiece.pieceColor == PlayerColor.White)
                {
                    GameBoard.instance.GetBoardSquare(pos).isPositiveTargetable = true;
                }
            }
        }

        SetShadow(4, 0);

        foreach (var s in GameBoard.instance.gameData.boardSquares)
        {
            if (s.coordinate == coordinate)
            {
                s.OnClick = DoNothing;
            }
        }

        descriptionText.text =
            "강림 키워드는 영혼 부여 직후\n" +
            "발동되는 효과를 의미합니다.\n" +
            "효과의 대상이 될\n" +
            "기물을 선택하세요.";
        SetTextSize(4);

        SoulCard bardInstance = Instantiate(bard) as SoulCard;

        bardInstance.Infuse(GameBoard.instance.gameData.GetPiece(coordinate));
        
        foreach (var s in GameBoard.instance.gameData.boardSquares)
        {
            if (s.coordinate == new Vector2Int(4, 0))
            {
                s.OnClick = ProcessStep21;
            }
            else
            {
                s.OnClick = DoNothing;
            }
        }
    }

    private void ProcessStep21(Vector2Int coordinate)
    {
        ClearTargetableObjects();

        SetShadow(-1, -1, isTurnButton: true);

        foreach (var s in GameBoard.instance.gameData.boardSquares)
        {
            if (s.coordinate == coordinate)
            {
                s.OnClick = DoNothing;
            }
        }
        // Set Active false Bard
        ToneDeafBard[] bards = FindObjectsOfType<ToneDeafBard>();

        for (int i = 0; i < bards.Length; i++)
        {
            if (bards[i].transform.parent == null)
            {
                bards[i].gameObject.SetActive(false);
            }
        }

        //GameObject.Find("CancelButton").SetActive(false);
       
        GameBoard.instance.gameData.GetPiece(coordinate).maxHP = 2;
        GameBoard.instance.gameData.GetPiece(coordinate).AD = 2;
        descriptionText.text =
            "이제 턴을 종료하세요.";
        SetTextSize(1);
        tutorialController.EnableTurnChangeButton();
    }

    public void ProcessStep22()
    {
        EnableShadow();
        SetShadow(3, 6);

        ShowText();
        descriptionText.text =
            "적 폰에 영혼이 부여되었습니다.\n" +
            "방어를 뚫기는 어려워 보입니다.";
        SetTextSize(2);

        textTransform.anchorMin = new Vector2(textTransform.anchorMin.x, 0.5f);
        textTransform.anchorMax = new Vector2(textTransform.anchorMin.x, 0.5f);
        textTransform.anchoredPosition = Vector2.zero;

        nextButton.gameObject.SetActive(true);
        nextButton.GetComponent<Button>().onClick.RemoveAllListeners();
        nextButton.GetComponent<Button>().onClick.AddListener(tutorialController.StartEnemyThirdRoutine2);
    }

    public void ProcessStep23()
    {
        UpdateMyHandPosition();

        EnableShadow();
        SetShadow(5, 2);

        ShowText();
        descriptionText.text =
            "적 기물이 아군 진영\n" +
            "깊숙히 들어왔습니다.\n" +
            "조심하세요.";
        SetTextSize(3);

        textTransform.anchorMin = new Vector2(textTransform.anchorMin.x, 0.78f);
        textTransform.anchorMax = new Vector2(textTransform.anchorMin.x, 0.78f);
        textTransform.anchoredPosition = Vector2.zero;

        nextButton.gameObject.SetActive(true);
        nextButton.GetComponent<Button>().onClick.RemoveAllListeners();
        nextButton.GetComponent<Button>().onClick.AddListener(tutorialController.OnTurnEndClicked);
    }

    public void ProcessStep24()
    {
        UpdateMyHandPosition();

        SetShadow(-1, -1, isCard: true);

        descriptionText.text =
            "덱에서 마법 카드를 뽑았습니다.\n" +
            "마법 카드는 영혼 부여가\n" +
            "불가능한 대신\n" +
            "강력한 효과를 가지고 있습니다.";
        SetTextSize(4);

        nextButton.gameObject.SetActive(true);
        nextButton.GetComponent<Button>().onClick.RemoveAllListeners();
        nextButton.GetComponent<Button>().onClick.AddListener(ProcessStep25);
    }

    private void ProcessStep25()
    {
        blockCardUse.SetActive(false);
        step = 25;
        nextButton.gameObject.SetActive(false);

        var whitePlayer = FindObjectOfType<PlayerController>();
        whitePlayer.isUsingCard = false;

        descriptionText.text =
            "처형 카드를 사용해\n" +
            "적을 처치하세요.";
        SetTextSize(2);
    }

    private void ProcessStep26()
    {
        blockCardUse.SetActive(true);
        SetShadow(3, 6);

        descriptionText.text =
            "적을 처치하세요.";
        SetTextSize(1);
        textTransform.anchorMin = new Vector2(0.8f, 0.5f);
        textTransform.anchorMax = new Vector2(0.8f, 0.5f);
        textTransform.anchoredPosition = Vector2.zero;

        foreach (var s in GameBoard.instance.gameData.boardSquares)
        {
            if (s.coordinate == new Vector2Int(3, 6))
            {
                s.OnClick = ProcessStep27;
            }
            else
            {
                s.OnClick = DoNothing;
            }
        }
    }

    private void ProcessStep27(Vector2Int coordinate)
    {
        SetShadow(1, 4);

        foreach (var s in GameBoard.instance.gameData.boardSquares)
        {
            if (s.coordinate == coordinate)
            {
                s.OnClick = DoNothing;
            }
        }
        GameBoard.instance.gameData.GetPiece(coordinate).Kill();

        descriptionText.text =
            "킹으로 가는 길이 열렸습니다!\n" +
            "적의 킹을 처치하고\n" +
            "게임에서 승리하세요!!";
        SetTextSize(3);

        ClearTargetableObjects();

        var whitePlayer = FindObjectOfType<PlayerController>();
        whitePlayer.isUsingCard = false;
        isUsingCard = false;
        isMoved = false;

        GameBoard.instance.HideCard();
        GameObject.Find("CancelButton").SetActive(false);

        foreach (var s in GameBoard.instance.gameData.boardSquares)
        {
            if (s.coordinate == new Vector2Int(1, 4))
            {
                s.OnClick = ProcessStep27_1;
            }
            else if (s.coordinate == new Vector2Int(4, 7))
            {
                s.OnClick = OnClickBoardSquare;
            }
            else
            {
                s.OnClick = DoNothing;
            }
        }
    }

    private void ProcessStep27_1(Vector2Int coordinate)
    {
        SetShadow(4, 7);

        OnClickBoardSquare(coordinate);
    }

    public void ShowCard(Card card)
    {
        Transform cardBoard = GameObject.Find("CardBoard").transform;
        float cardSize = 1.5f;
        ShowingCard = Instantiate(card, cardBoard.position, Quaternion.identity);
        ShowingCard.GetComponent<Collider2D>().enabled = false;
        ShowingCard.GetComponent<SortingGroup>().sortingOrder = -1;
        ShowingCard.transform.localScale = new Vector3(1f, 1f, 0f) * cardSize;

        ShowingCard.FlipFront();
    }

    public void RemoveShowCard()
    {
        ShowingCard.Destroy();
    }

    public void ShowText()
    {
        textTransform.gameObject.SetActive(true);
    }

    public void RemoveText()
    {
        textTransform.gameObject.SetActive(false);
    }

    private void OnEndTutorial(ChessPiece piece)
    {
        RemoveText();
        RemoveArrow();
    }

    public void RemoveNextButton()
    {
        nextButton.gameObject.SetActive(false);
    }

    private void SetTextSize(int lineCount)
    {
        if (lineCount == 1) lineCount = 2;
        textTransform.sizeDelta = new Vector2(textTransform.sizeDelta.x, lineCount * 30f);
        RectTransform buttion = nextButton.GetComponent<RectTransform>();
        buttion.anchoredPosition = new Vector2(buttion.anchoredPosition.x, -13f * lineCount + 7f);
    }

    public void RemoveArrow()
    {
        arrow.gameObject.SetActive(false);
    }

    public void EnableArrow()
    {
        arrow.gameObject.SetActive(true);
    }

    public void OnClickBoardSquare(Vector2Int coordinate)
    {
        if (!GameBoard.instance.isActivePlayer && !GameBoard.instance.isDebugMode)
            return;

        ChessPiece targetPiece = GameBoard.instance.gameData.GetPiece(coordinate);

        if (isUsingCard)
        {
            //Debug.Log("Using Card");
            if (targetableObjects.Any(obj => obj.coordinate == coordinate))
            {
                ClearTargetableObjects();

                TargetableObject target;
                if (targetingEffect.GetTargetType().targetType == TargetingEffect.TargetType.Piece)
                    target = targetPiece;
                else // if (targetingEffect.GetTargetType().targetType == TargetingEffect.TargetType.Tile)
                    target = GameBoard.instance.gameData.boardSquares[coordinate.x, coordinate.y];

                if (targetingEffect.SetTarget(target))
                {
                    UseCardEffect();
                }
                else
                {
                    targetableObjects = targetingEffect.GetTargetType().GetTargetList(playerColor);

                    // 타겟 효과가 부정적인지 파라미터 전달
                    SetTargetableObjects(true, targetingEffect.IsNegativeEffect);
                }
            }
        }
        else // 이동 관련 코드
        {
            if (chosenPiece == null)// 선택된 (아군)기물이 없을 때
            {
                if (targetPiece != null)
                {
                    if (IsMyPiece(targetPiece))// 고른 기물이 아군일때
                        if (!isMoved || (targetPiece.moveCountInThisTurn > 0 && targetPiece.moveCountInThisTurn <= targetPiece.moveCount))
                        {
                            SetChosenPiece(targetPiece);
                        }
                }
            }
            else // 선택된 (아군)기물이 있을 때
            {
                if (targetPiece != null)
                {
                    if (chosenPiece == targetPiece)
                    {
                        targetPiece.SelectedEffectOff();
                        chosenPiece = null;
                        ClearMovableCoordniates();
                    }
                    else if (IsMyPiece(targetPiece))// 고른 기물이 아군일때
                    {
                        chosenPiece.SelectedEffectOff();
                        SetChosenPiece(targetPiece);
                    }
                    else// 고른 기물이 적일 때
                    {
                        if (IsMovableCoordniate(coordinate))
                        {
                            chosenPiece.SelectedEffectOff();
                            //photonView.RPC("MovePiece", RpcTarget.All, chosenPiece.coordinate.x, chosenPiece.coordinate.y, coordinate.x, coordinate.y, true);
                            MovePiece(chosenPiece.coordinate.x, chosenPiece.coordinate.y, coordinate.x, coordinate.y, true);

                            chosenPiece = null;
                            ClearMovableCoordniates();
                        }
                    }
                }
                else // 고른 칸이 빈칸일때
                {

                    Debug.Log("Move to " + coordinate);
                    chosenPiece.SelectedEffectOff();
                    if (IsMovableCoordniate(coordinate))
                    {

                        //photonView.RPC("MovePiece", RpcTarget.All, chosenPiece.coordinate.x, chosenPiece.coordinate.y, coordinate.x, coordinate.y, false);
                        MovePiece(chosenPiece.coordinate.x, chosenPiece.coordinate.y, coordinate.x, coordinate.y, false);
                    }
                    chosenPiece = null;
                    ClearMovableCoordniates();
                }

            }
        }
    }
    public void UseCardEffect()
    {
        GameBoard.instance.cancelButton.Hide();
        if (isInfusing)
        {
            (usingCard as SoulCard).infusion.EffectAction(playerController);
            if (usingCard.EffectOnCardUsed != null)
            {
                targetingEffect = null;
                UseCard(usingCard);
                return;
            }
            isInfusing = false;
        }

        if (usingCard is SoulCard)
        {
            if (usingCard.EffectOnCardUsed is TargetingEffect)
            {

            }
                //photonView.RPC("UseCardRemote", RpcTarget.Others, usingCard.handIndex, (usingCard as SoulCard).infusion.serializedTargetData[0], (usingCard.EffectOnCardUsed as TargetingEffect).serializedTargetData);
            else
            {

            }
                //photonView.RPC("UseCardRemote", RpcTarget.Others, usingCard.handIndex, (usingCard as SoulCard).infusion.serializedTargetData[0], null);
        }
        else
        {
            if (usingCard.EffectOnCardUsed is TargetingEffect)
            {

            }
                //photonView.RPC("UseCardRemote", RpcTarget.Others, usingCard.handIndex, new Vector3(-1, -1, -1), (usingCard.EffectOnCardUsed as TargetingEffect).serializedTargetData);
            else
            {

            }
                //photonView.RPC("UseCardRemote", RpcTarget.Others, usingCard.handIndex, new Vector3(-1, -1, -1), null);
        }

        GameBoard.instance.CurrentPlayerData().soulEssence -= usingCard.cost;

        GameBoard.instance.gameData.myPlayerData.TryRemoveCardInHand(usingCard);

        usingCard.EffectOnCardUsed?.EffectAction(playerController);

        if (!(usingCard is SoulCard))
            usingCard.Destroy();
        usingCard = null;
        isUsingCard = false;
        targetingEffect = null;


        GameBoard.instance.HideCard();

    }
    public bool UseCard(Card card)
    {
        if (GameBoard.instance.CurrentPlayerData().soulEssence < card.cost)
            return false;

        usingCard = card;
        isUsingCard = true;

        if (usingCard is SoulCard)
        {
            if (!isInfusing) // 소울 카드를 처음 냈을 때
            {
                if (!(usingCard as SoulCard).infusion.isAvailable(playerColor)) // 카드의 기물 제한을 만족하지 못하는 경우
                {
                    usingCard = null;
                    isUsingCard = false;
                    return false;
                }
                else if (usingCard.EffectOnCardUsed is TargetingEffect)
                {
                    if (!(usingCard.EffectOnCardUsed as TargetingEffect).isAvailable(playerColor)) // 카드의 효과 대상이 없는 경우
                    {
                        usingCard = null;
                        isUsingCard = false;
                        return false;
                    }
                }

                GameBoard.instance.cancelButton.Show();

                isInfusing = true;
                (usingCard as SoulCard).gameObject.SetActive(false);
                targetingEffect = (usingCard as SoulCard).infusion;
                ActiveTargeting(); // 카드 강림 대상 선택
            }
            else // 소울 카드 내고 -> 강림 대상 선택 후
            { 
                isInfusing = false;

                if (usingCard.EffectOnCardUsed is TargetingEffect)
                {
                    if (!(usingCard.EffectOnCardUsed as TargetingEffect).isAvailable(playerColor))
                    {
                        usingCard = null;
                        isUsingCard = false;
                        return false;
                    }
                    //영혼 카드는 강림 선택 시점이 여기인듯
                    (usingCard as SoulCard).gameObject.SetActive(false);
                    targetingEffect = usingCard.EffectOnCardUsed as TargetingEffect;
                    ActiveTargeting();
                }
                else
                {
                    UseCardEffect();
                }
            }
        }
        else
        {
            if (card.EffectOnCardUsed is TargetingEffect)
            {
                if (!(usingCard.EffectOnCardUsed as TargetingEffect).isAvailable(playerColor))
                {
                    usingCard = null;
                    isUsingCard = false;
                    return false;
                }

                GameBoard.instance.cancelButton.Show();

                usingCard.gameObject.SetActive(false); //마법 카드는 여기인듯?
                targetingEffect = usingCard.EffectOnCardUsed as TargetingEffect;
                ActiveTargeting();
            }
            else
            {
                UseCardEffect();
            }
        }

        return true;
    }


    public void UpdateMyHandPosition()
    {
        List<Card> hand = GameBoard.instance.gameData.myPlayerData.hand;
        float anchor_x;

        for (int i = hand.Count - 1; i >= 0; i--)
        {
            if (!hand[i].gameObject.activeSelf)
            {
                hand.RemoveAt(i);
            }
        }

        if (hand.Count == 0)
            anchor_x = 0;
        else if (hand.Count % 2 == 0)
            anchor_x = -(hand.Count / 2f - 0.5f) * CARD_DISTANCE_IN_HAND;
        else
            anchor_x = -(hand.Count / 2f) * CARD_DISTANCE_IN_HAND;


        for (int i = 0; i < hand.Count; i++)
        {
            Debug.Log(hand[i].name);

            hand[i].handIndex = i;
            hand[i].GetComponent<SortingGroup>().sortingOrder = i;
            hand[i].transform.SetParent(myHandTransform);
            hand[i].transform.localPosition = new Vector3(anchor_x + CARD_DISTANCE_IN_HAND * i, 0, -0.1f * i); //UI에 맞게 좌표수정
        }
    }

    public void UpdateOpponentHandPosition()
    {
        List<Card> hand = GameBoard.instance.gameData.opponentPlayerData.hand;
        float anchor_x;

        for (int i = hand.Count - 1; i >= 0; i--)
        {
            if (!hand[i].gameObject.activeSelf)
            {
                hand.RemoveAt(i);
            }
        }

        if (hand.Count == 0)
            anchor_x = 0;
        else if (hand.Count % 2 == 0)
            anchor_x = (hand.Count / 2f - 0.5f) * CARD_DISTANCE_IN_HAND;
        else
            anchor_x = (hand.Count / 2f) * CARD_DISTANCE_IN_HAND;


        for (int i = 0; i < hand.Count; i++)
        {
            hand[i].handIndex = i;
            hand[i].GetComponent<SortingGroup>().sortingOrder = i - GameBoard.instance.gameData.opponentPlayerData.maxHandCardCount;
            hand[i].transform.SetParent(opponentHandTransform);
            hand[i].transform.localPosition = new Vector3(anchor_x - CARD_DISTANCE_IN_HAND * i, 0, -0.1f * i); //UI에 맞게 좌표수정
            if (player.isRevealed)
            {
                hand[i].FlipFront();
            }
        }
    }

    private void ActiveTargeting()
    {
        ClearMovableCoordniates();

        targetableObjects = targetingEffect.GetTargetType().GetTargetList(playerColor);

        //타겟 효과가 부정적인지 파라미터 전달
        SetTargetableObjects(true, targetingEffect.IsNegativeEffect);
    }

    void SetChosenPiece(ChessPiece targetPiece)
    {
        targetPiece.SelectedEffectOn();
        ClearMovableCoordniates();

        movableCoordinates.AddRange(targetPiece.GetMovableCoordinates());
        foreach (var c in movableCoordinates)
        {
            GameBoard.instance.GetBoardSquare(c).isMovable = true;
        }

        chosenPiece = targetPiece;
    }
    void ClearMovableCoordniates()
    {
        movableCoordinates.Clear();
        foreach (var sq in GameBoard.instance.gameData.boardSquares)
        {
            sq.isNegativeTargetable = false;
            sq.isPositiveTargetable = false;
        }
    }
    bool IsMovableCoordniate(Vector2Int coordinate)
    {
        return movableCoordinates.Contains(coordinate);
    }
    bool IsMyPiece(ChessPiece chessPiece)
    {
        return chessPiece.pieceColor == playerColor;
    }

    void ClearTargetableObjects()
    {
        targetableObjects.Clear();
        foreach (var sq in GameBoard.instance.gameData.boardSquares)
        {
            sq.isMovable = false;
        }
    }
    void SetTargetableObjects(bool isTargetable, bool isNegativeEffect)
    {
        foreach (var obj in targetableObjects)
            if (isTargetable)
            {
                //타겟 효과가 부정적인지 체크
                if (isNegativeEffect)
                    GameBoard.instance.GetBoardSquare(obj.coordinate).isNegativeTargetable = true;
                else
                    GameBoard.instance.GetBoardSquare(obj.coordinate).isPositiveTargetable = true;
            }
    }

    public void MovePiece(int src_x, int src_y, int dst_x, int dst_y, bool isAttack)
    {
        Vector2Int dst_coordinate = new Vector2Int(dst_x, dst_y);
        Vector2Int src_coordinate = new Vector2Int(src_x, src_y);

        ChessPiece srcPiece = GameBoard.instance.gameData.GetPiece(src_coordinate);

        srcPiece.moveCountInThisTurn++;
        isMoved = true;


        if (isAttack)
        {
            ChessPiece dstPiece = GameBoard.instance.gameData.GetPiece(dst_coordinate);
            if (srcPiece.Attack(dstPiece))
            {
                Debug.Log("kill" + dstPiece.name);
                srcPiece.Move(dst_coordinate);
                GameBoard.instance.chessBoard.KillAnimation(srcPiece, dstPiece);
            }
            else
            {
                Debug.Log("Can't kill" + dstPiece.name);
                srcPiece.GetComponent<Animator>().SetTrigger("moveTrigger");
                srcPiece.GetComponent<Animator>().SetBool("isReturning", true);
                GameBoard.instance.chessBoard.ForthBackPieceAnimation(srcPiece, dstPiece);
            }
        }
        else
        {
            srcPiece.Move(dst_coordinate);
            GameManager.instance.soundManager.PlaySFX("Move", pitch: 2.0f);
            GameBoard.instance.chessBoard.MovePieceAnimation(srcPiece);
        }
    }

    private void DoNothing(Vector2Int pos)
    {

    }
}
