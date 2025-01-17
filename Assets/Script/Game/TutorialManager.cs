using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using static GameBoard;

public class TutorialManager : MonoBehaviour
{
    public CutoutMaskUI cutoutMaskUI;
    public Transform myDeckTransform;
    public Transform myHandTransform;
    public Transform opponentDeckTransform;
    public Transform opponentHandTransform;

    private PlayerData player;
    readonly float CARD_DISTANCE_IN_HAND = 0.5f;

    private Card usingCard = null;
    private TargetingEffect targetingEffect;
    public bool isUsingCard = false;
    private bool isInfusing = false;
    ChessPiece chosenPiece = null;
    List<Vector2Int> movableCoordinates = new List<Vector2Int>();
    List<TargetableObject> targetableObjects = new List<TargetableObject>();

    private bool isMoved;

    // Tutorial
    private bool isInitalized;

    public PlayerController playerController;
    private PlayerColor playerColor { get => playerController.playerColor; }

    private void Awake()
    {
        isInitalized = false;
        cutoutMaskUI.SetToCanvasSize();
    }

    private void Start()
    {
    }

    private void Update()
    {
        if (!isInitalized)
        {
            InitializeMyCards();
            InitializeOpponentCards();
            foreach (var s in GameBoard.instance.gameData.boardSquares)
            {
                s.OnClick = OnClickBoardSquare;
            }
            isInitalized = true;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (IsClickingTouchable())
            {

            }
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
        foreach (Card card in GameManager.instance.GetCardListFrom(GetList(13, 46, 16)))
        {
            card.owner = GameBoard.instance.gameData.myPlayerData;
            var instantiatedCard = Instantiate(card, deckAnchor);
            instantiatedCard.FlipBack();
            player.deck.Add(instantiatedCard);
            instantiatedCard.GetComponent<SortingGroup>().sortingOrder = -1;
        }

        player.deckPosition = new Vector2(7.6f, -2.3f); //UI에 맞게 좌표수정
    }

    private void InitializeOpponentCards()
    {
        var deckAnchor = opponentDeckTransform;

        foreach (Card card in GameManager.instance.GetCardListFrom(GetList(1, 13, 1, 14)))
        {
            card.owner = GameBoard.instance.gameData.opponentPlayerData;
            var instantiatedCard = Instantiate(card, deckAnchor);
            instantiatedCard.FlipBack();
            GameBoard.instance.gameData.opponentPlayerData.deck.Add(instantiatedCard);
            instantiatedCard.GetComponent<SortingGroup>().sortingOrder = -1;
        }
    }

    private List<int> GetList(params int[] index)
    {
        List<int> cards = new List<int>()
            {
                13,
                46,
                16
            };
        return cards;
    }

    private void UpdateMyHandPosition()
    {
        List<Card> hand = GameBoard.instance.gameData.myPlayerData.hand;
        float anchor_x;

        if (hand.Count == 0)
            anchor_x = 0;
        else if (hand.Count % 2 == 0)
            anchor_x = -(hand.Count / 2f - 0.5f) * CARD_DISTANCE_IN_HAND;
        else
            anchor_x = -(hand.Count / 2f) * CARD_DISTANCE_IN_HAND;


        for (int i = 0; i < hand.Count; i++)
        {
            hand[i].handIndex = i;
            hand[i].GetComponent<SortingGroup>().sortingOrder = i;
            hand[i].transform.SetParent(myHandTransform);
            hand[i].transform.localPosition = new Vector3(anchor_x + CARD_DISTANCE_IN_HAND * i, 0, -0.1f * i); //UI에 맞게 좌표수정
        }
    }

    public void OnClickBoardSquare(Vector2Int coordinate)
    {
        if (!GameBoard.instance.isActivePlayer && !GameBoard.instance.isDebugMode)
            return;

        Debug.Log("Clicked");

        ChessPiece targetPiece = GameBoard.instance.gameData.GetPiece(coordinate);

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
                    if (IsMyPiece(targetPiece))// 고른 기물이 아군일때
                    {
                        SetChosenPiece(targetPiece);
                    }
                    else// 고른 기물이 적일 때
                    {
                        if (IsMovableCoordniate(coordinate))
                        {
                            //photonView.RPC("MovePiece", RpcTarget.All, chosenPiece.coordinate.x, chosenPiece.coordinate.y, coordinate.x, coordinate.y, true);

                            chosenPiece = null;
                            ClearMovableCoordniates();
                        }
                    }
                }
                else // 고른 칸이 빈칸일때
                {
                    if (IsMovableCoordniate(coordinate))
                    {
                        //photonView.RPC("MovePiece", RpcTarget.All, chosenPiece.coordinate.x, chosenPiece.coordinate.y, coordinate.x, coordinate.y, false);
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


    private void ActiveTargeting()
    {
        ClearMovableCoordniates();

        targetableObjects = targetingEffect.GetTargetType().GetTargetList(playerColor);

        //타겟 효과가 부정적인지 파라미터 전달
        SetTargetableObjects(true, targetingEffect.IsNegativeEffect);
    }

    void SetChosenPiece(ChessPiece targetPiece)
    {
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
}
