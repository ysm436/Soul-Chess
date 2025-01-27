using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using System;
using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using UnityEngine.SceneManagement;

public class PvEPlayerController : PlayerController
{
    private PlayerData thisPlayerData;
    public bool isComputer
    {
        get
        {
            return _isComputer;
        }
        set
        {
            _isComputer = value;
            if (value)
            {
                //computer 코드추가
                OnMyTurnStart += () => StartCoroutine(ComputerAct());
            }
        }
    }
    [SerializeField] private bool _isComputer = false;
    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }
    private void Start()
    {
        OnOpponentTurnEnd += () => { isMoved = false; };

        if (isComputer)
            thisPlayerData = GameBoard.instance.ComputerData();
        else
            thisPlayerData = GameBoard.instance.CurrentPlayerData();
    }
    private void OnEnable()
    {
        foreach (var s in gameBoard.gameData.boardSquares)
        {
            if (SceneManager.GetActiveScene().name == "TutorialScene") return;
            s.OnClick = OnClickBoardSquare;
        }
    }
    private void OnDisnable()
    {
        foreach (var s in gameBoard.gameData.boardSquares)
        {
            s.OnClick = null;
        }
    }

    public override void OnClickBoardSquare(Vector2Int coordinate)
    {
        if (!((GameBoard.instance.isWhiteTurn && playerColor == GameBoard.PlayerColor.White) || (!GameBoard.instance.isWhiteTurn && playerColor == GameBoard.PlayerColor.Black)))
            return;

        ChessPiece targetPiece = gameBoard.gameData.GetPiece(coordinate);

        if (isUsingCard)
        {
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
                            MovePiece( chosenPiece.coordinate.x, chosenPiece.coordinate.y, coordinate.x, coordinate.y, true);

                            chosenPiece = null;
                            ClearMovableCoordniates();
                        }
                    }
                }
                else // 고른 칸이 빈칸일때
                {
                    chosenPiece.SelectedEffectOff();
                    if (IsMovableCoordniate(coordinate))
                    {
                        MovePiece( chosenPiece.coordinate.x, chosenPiece.coordinate.y, coordinate.x, coordinate.y, false);
                    }
                    chosenPiece = null;
                    ClearMovableCoordniates();
                }
            }
        }
    }

    private void MovePiece(int src_x, int src_y, int dst_x, int dst_y, bool isAttack)
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
                srcPiece.Move(dst_coordinate);
                gameBoard.chessBoard.KillAnimation(srcPiece, dstPiece);
            }
            else
            {
                srcPiece.GetComponent<Animator>().SetTrigger("moveTrigger");
                srcPiece.GetComponent<Animator>().SetBool("isReturning", true);
                gameBoard.chessBoard.ForthBackPieceAnimation(srcPiece, dstPiece);
            }
        }
        else
        {
            srcPiece.Move(dst_coordinate);
            gameBoard.chessBoard.MovePieceAnimation(srcPiece);
        }
    }
    void SetChosenPiece(ChessPiece targetPiece)
    {
        targetPiece.SelectedEffectOn();
        ClearMovableCoordniates();

        movableCoordinates.AddRange(targetPiece.GetMovableCoordinates());
        foreach (var c in movableCoordinates)
        {
            gameBoard.GetBoardSquare(c).isMovable = true;
        }

        chosenPiece = targetPiece;
    }
    void ClearMovableCoordniates()
    {
        movableCoordinates.Clear();
        foreach (var sq in gameBoard.gameData.boardSquares)
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
        foreach (var sq in gameBoard.gameData.boardSquares)
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
    public override bool UseCard(Card card)
    {
        if (thisPlayerData.soulEssence < card.cost)
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

                if (!isComputer)
                    GameBoard.instance.cancelButton.Show();
                else
                {
                    gameBoard.ShowCard(usingCard);
                }

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

                if(!isComputer)
                    GameBoard.instance.cancelButton.Show();
                else
                    gameBoard.ShowCard(usingCard);

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
    private void ActiveTargeting()
    {
        ClearMovableCoordniates();

        targetableObjects = targetingEffect.GetTargetType().GetTargetList(playerColor);

        //타겟 효과가 부정적인지 파라미터 전달
        SetTargetableObjects(true, targetingEffect.IsNegativeEffect);
    }
    public override void UseCardEffect()
    {
        GameBoard.instance.cancelButton.Hide();
        if (isInfusing)
        {
            (usingCard as SoulCard).infusion.EffectAction(this);
            if (usingCard.EffectOnCardUsed != null)
            {
                targetingEffect = null;
                UseCard(usingCard);
                return;
            }
            isInfusing = false;
        }

        thisPlayerData.soulEssence -= usingCard.cost;
        
        thisPlayerData.TryRemoveCardInHand(usingCard);

        usingCard.EffectOnCardUsed?.EffectAction(this);

        if (!(usingCard is SoulCard))
            usingCard.Destroy();
        usingCard = null;
        isUsingCard = false;
        targetingEffect = null;


        GameBoard.instance.HideCard();

    }

    private void HideRemoteUsedCard()
    {
        gameBoard.HideCard();
        foreach (BoardSquare bs in gameBoard.gameData.boardSquares)
        {
            bs.outline.changeOutline(BoardSquareOutline.TargetableStates.none);
        }
        GameBoard.instance.gameData.opponentPlayerData.UpdateHandPosition();
    }

    public override void DiscardCard(Card card)
    {
        Debug.Log("Discard");
        RPCDiscardCard(card.handIndex);
    }

    public override void RPCDiscardCard(int handIndex)
    {
        Card cardInstance = thisPlayerData.hand[handIndex];

        thisPlayerData.soulEssence -= Card.discardCost;
        thisPlayerData.TryRemoveCardInHand(cardInstance);
        cardInstance.Destroy();
        GameBoard.instance.HideCard();
        thisPlayerData.UpdateHandPosition();

        LocalDraw();
    }
    public override void Draw()
    {
        LocalDraw();
    }

    public override void LocalDraw()
    {
        if (playerColor == GameBoard.PlayerColor.White)
        {
            StartCoroutine(GameBoard.instance.gameData.playerWhite.DrawCardWithAnimation());
        }
        else
        {
            StartCoroutine(GameBoard.instance.gameData.playerBlack.DrawCardWithAnimation());
        }
        OnMyDraw?.Invoke();
    }

    private IEnumerator MultipleDrawC(int count, PlayerController opponentController)
    {
        for (int i = 0; i < count; i++)
        {
            LocalDraw();
            opponentController.OpponentDraw();
            yield return new WaitForSeconds(1f);
        }
    }

    
    public IEnumerator ComputerAct()
    {
        yield return new WaitForSeconds(4f);
        //카드 쓰기
        yield return UseCardComputer();
        //체스 말 이동
        yield return MovePieceComputer();
        GetComponentInParent<PvELocalController>().TurnEnd();
    }

    private IEnumerator UseCardComputer()
    {
        int nowCost = GameBoard.instance.ComputerData().soulEssence;
        List<Card> hand = GameBoard.instance.ComputerData().hand;
        int handCount = hand.Count;

        int[,] dp = new int[handCount,2];
        dp[0, 0] = 0;
        if (nowCost >= hand[0].cost && CanUseCard(hand[0]))
            dp[0, 1] = hand[0].cost;
        else
            dp[0, 1] = -1;
        for (int i = 1; i < handCount; i++)
        {
            dp[i, 0] = Mathf.Max(dp[i - 1, 0], dp[i - 1, 1]);

            if (hand[i].cost > nowCost || !CanUseCard(hand[i]))
            {
                dp[i, 1] = -1;
                continue;
            }
            else
                dp[i, 1] = hand[i].cost;

            if (dp[i - 1, 0] + hand[i].cost <= nowCost)
                dp[i, 1] = dp[i - 1, 0] + hand[i].cost;
            if (dp[i-1,1] != -1 && dp[i - 1, 1] + hand[i].cost <= nowCost)
                dp[i, 1] = dp[i - 1, 1] + hand[i].cost;
        }

        for (int i = 0; i < handCount; i++)
        {
            Debug.Log(i + " " + 0 + " : " + dp[i, 0]);
            Debug.Log(i + " " + 1 + " : " + dp[i, 1]);
        }

        List<Card> willUseCards = new List<Card>();

        for (int i = handCount - 1; i >= 0; i--)
        {
            if (hand[i].cost > nowCost)
                continue;
            if (dp[i, 1] >= dp[i, 0])
            {
                willUseCards.Add(hand[i]);
                nowCost -= hand[i].cost;
            }
        }

        foreach (var card in willUseCards)
        {
            Debug.Log(card);
        }

        foreach (var card in willUseCards)
        {
            if (UseCard(card))
            {
                yield return new WaitForSeconds(3f);
                int randNum = UnityEngine.Random.Range(0, targetableObjects.Count);
                //Debug.Log(targetableObjects.Count);
                //Debug.Log(randNum);
                OnClickBoardSquare(targetableObjects[randNum].coordinate);
                yield return new WaitForSeconds(2f);
                if (targetableObjects.Count > 0)
                {
                    OnClickBoardSquare(targetableObjects[randNum].coordinate);
                    yield return new WaitForSeconds(2f);
                }
            }
        }

        yield return null;
    }

    private IEnumerator MovePieceComputer()
    {
        yield return CalculateAllPossibleCoordinate();
    }

    private IEnumerator CalculateAllPossibleCoordinate()
    {
        List<(ChessPiece piece, Vector2Int coord)> AllMovableCoordinates = new List<(ChessPiece piece, Vector2Int coord)>();

        List<ChessPiece> AllChessPieces = GameBoard.instance.chessBoard.GetAllPieces(playerColor);

        foreach (var piece in AllChessPieces)
        {
            foreach(Vector2Int coord in piece.GetMovableCoordinates())
                AllMovableCoordinates.Add((piece,coord));
        }

        //가중치 계산하기
        if (AllMovableCoordinates.Count > 0)
        {
            int randNum = UnityEngine.Random.Range(0, AllChessPieces.Count);

            OnClickBoardSquare(AllMovableCoordinates[randNum].piece.coordinate);

            yield return new WaitForSeconds(2f);

            OnClickBoardSquare(AllMovableCoordinates[randNum].coord);

            yield return new WaitForSeconds(1f);
        }
    }

    public bool CanUseCard(Card card)
    {
        if (card is SoulCard)
        {
            if (!(card as SoulCard).infusion.isAvailable(playerColor)) // 카드의 기물 제한을 만족하지 못하는 경우
            {
                return false;
            }
            else if (card.EffectOnCardUsed is TargetingEffect)
            {
                if (!(card.EffectOnCardUsed as TargetingEffect).isAvailable(playerColor)) // 카드의 효과 대상이 없는 경우
                {
                    return false;
                }
            }
        }
        else
        {
            if (card.EffectOnCardUsed is TargetingEffect)
            {
                if (!(card.EffectOnCardUsed as TargetingEffect).isAvailable(playerColor))
                {

                    return false;
                }
            }
        }
        return true;
    }
}
