using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
public class PvELocalController : MonoBehaviour, IPointerClickHandler
{
    PhotonView photonView;

    SpriteRenderer spriteRenderer;
    public Sprite whiteButton;
    public Sprite blackButton;
    public PlayerController whiteController;
    public PlayerController blackController;
    public TurnChangeButtonHighlight turnChangeButtonHighlight;
    public SoulOrb mySoulOrb;
    public SoulOrb opponentSoulrOrb;
    public GameObject turn_display;
    [SerializeField] private GameObject blocker;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = whiteButton;

        photonView = GetComponent<PhotonView>();

        if (GameManager.instance.isHost)
        {
            mySoulOrb.playerColor = GameBoard.PlayerColor.White;
            opponentSoulrOrb.playerColor = GameBoard.PlayerColor.Black;
            whiteController.soulOrb = mySoulOrb;
            blackController.soulOrb = opponentSoulrOrb;
            ChangeComputerTurn(false);
        }
        else
        {
            mySoulOrb.playerColor = GameBoard.PlayerColor.Black;
            opponentSoulrOrb.playerColor = GameBoard.PlayerColor.White;
            blackController.soulOrb = mySoulOrb;
            whiteController.soulOrb = opponentSoulrOrb;
            ChangeComputerTurn(true);
        }

    }
    private void Start()
    {
        whiteController.enabled = true;
        blackController.enabled = false;

        if (GameBoard.instance.playerColor == GameBoard.PlayerColor.White)
            turn_display.GetComponentInChildren<TextMeshProUGUI>().text = "당신의 턴";
        else
            turn_display.GetComponentInChildren<TextMeshProUGUI>().text = "상대의 턴";
        StartCoroutine("TurnDisplayOnOff");
    }

    void IPointerClickHandler.OnPointerClick(UnityEngine.EventSystems.PointerEventData eventData)
    {
        if (!GameBoard.instance.myController.enabled)
            return;
        if (GameBoard.instance.myController.TurnEndPossible)
            TurnEnd();
        else
            Debug.Log("Please Move Any Chess Piece at least Once");
    }

    public void TurnEnd()
    {
        if ((GameBoard.instance.isWhiteTurn && whiteController.enabled) || (!GameBoard.instance.isWhiteTurn && blackController.enabled))
        {
            OnTurnEndClicked();
        }
    }

    private void OnTurnEndClicked()
    {
        if (whiteController.enabled)
        {
            GameBoard.instance.isWhiteTurn = false;
            ChangeComputerTurn(!GameBoard.instance.isComputerTurn);
            spriteRenderer.sprite = blackButton;

            whiteController.TurnEnd();
            blackController.OpponentTurnEnd();

            whiteController.enabled = false;
            blackController.enabled = true;

            if (GameBoard.instance.playerColor == GameBoard.PlayerColor.White)
                turn_display.GetComponentInChildren<TextMeshProUGUI>().text = "상대의 턴";
            else
                turn_display.GetComponentInChildren<TextMeshProUGUI>().text = "당신의 턴";
            StartCoroutine("TurnDisplayOnOff");

            blackController.TurnStart();
            whiteController.OpponentTurnStart();

            blackController.LocalDraw();
            whiteController.OpponentDraw();
        }
        else
        {
            GameBoard.instance.isWhiteTurn = true;
            ChangeComputerTurn(!GameBoard.instance.isComputerTurn);
            spriteRenderer.sprite = whiteButton;

            blackController.TurnEnd();
            whiteController.OpponentTurnEnd();

            blackController.enabled = false;
            whiteController.enabled = true;

            if (GameBoard.instance.playerColor == GameBoard.PlayerColor.White)
                turn_display.GetComponentInChildren<TextMeshProUGUI>().text = "당신의 턴";
            else
                turn_display.GetComponentInChildren<TextMeshProUGUI>().text = "상대의 턴";
            StartCoroutine("TurnDisplayOnOff");

            whiteController.TurnStart();
            blackController.OpponentTurnStart();

            whiteController.LocalDraw();
            blackController.OpponentDraw();
        }
        turnChangeButtonHighlight.spriteRenderer.enabled = false;
    }

    private IEnumerator TurnDisplayOnOff()
    {
        turn_display.SetActive(true);
        yield return new WaitForSeconds(1f);
        turn_display.SetActive(false);
    }

    private void ChangeComputerTurn(bool active)
    {
        GameBoard.instance.isComputerTurn = active;
        blocker.gameObject.SetActive(active);
    }
}
