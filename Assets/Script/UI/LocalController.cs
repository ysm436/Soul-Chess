using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class LocalController : MonoBehaviour, IPointerClickHandler
{
    PhotonView photonView;

    SpriteRenderer spriteRenderer;
    public Sprite whiteButton;
    public Sprite blackButton;
    public PlayerController whiteController;
    public PlayerController blackController;
    public TurnChangeButtonHighlight turnChangeButtonHighlight;
    public SoulOrb mySoulOrb;
    public SoulOrb opponentSoulOrb;
    public ChessTimer myTimer;
    public ChessTimer opponentTimer;
    public GameObject turnDisplay;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = whiteButton;

        photonView = GetComponent<PhotonView>();

        if (GameManager.instance.isHost)
        {
            mySoulOrb.playerColor = GameBoard.PlayerColor.White;
            opponentSoulOrb.playerColor = GameBoard.PlayerColor.Black;
            whiteController.soulOrb = mySoulOrb;
            blackController.soulOrb = opponentSoulOrb;
            
            myTimer.playerColor = GameBoard.PlayerColor.White;
            opponentTimer.playerColor = GameBoard.PlayerColor.Black;
            whiteController.chessTimer = myTimer;
            blackController.chessTimer = opponentTimer;
        }
        else
        {
            mySoulOrb.playerColor = GameBoard.PlayerColor.Black;
            opponentSoulOrb.playerColor = GameBoard.PlayerColor.White;
            blackController.soulOrb = mySoulOrb;
            whiteController.soulOrb = opponentSoulOrb;

            myTimer.playerColor = GameBoard.PlayerColor.Black;
            opponentTimer.playerColor = GameBoard.PlayerColor.White;
            blackController.chessTimer = myTimer;
            whiteController.chessTimer = opponentTimer;
        }

    }
    private void Start()
    {
        whiteController.enabled = true;
        blackController.enabled = false;

        if (GameBoard.instance.playerColor == GameBoard.PlayerColor.White)
            turnDisplay.GetComponentInChildren<TextMeshProUGUI>().text = "당신의 턴";
        else
            turnDisplay.GetComponentInChildren<TextMeshProUGUI>().text = "상대의 턴";
        StartCoroutine("TurnDisplayOnOff");

        whiteController.chessTimer.StartTimer();
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (GameBoard.instance.myController.TurnEndPossible)
            TurnEnd();
        else
            Debug.Log("Please Move Any Chess Piece at least Once");
    }

    public void TurnEnd()
    {
        if (GameBoard.instance.isActivePlayer)
            photonView.RPC("OnTurnEndClicked", RpcTarget.All);
    }

    [PunRPC]
    private void OnTurnEndClicked()
    {
        if (whiteController.enabled)
        {
            spriteRenderer.sprite = blackButton;

            whiteController.TurnEnd();
            blackController.OpponentTurnEnd();

            whiteController.enabled = false;
            blackController.enabled = true;
            
            if (GameBoard.instance.playerColor == GameBoard.PlayerColor.White)
                turnDisplay.GetComponentInChildren<TextMeshProUGUI>().text = "상대의 턴";
            else
                turnDisplay.GetComponentInChildren<TextMeshProUGUI>().text = "당신의 턴";
            StartCoroutine("TurnDisplayOnOff");

            blackController.TurnStart();
            whiteController.OpponentTurnStart();

            blackController.LocalDraw();
            whiteController.OpponentDraw();
        }
        else
        {
            spriteRenderer.sprite = whiteButton;

            blackController.TurnEnd();
            whiteController.OpponentTurnEnd();
            
            blackController.enabled = false;
            whiteController.enabled = true;

            if (GameBoard.instance.playerColor == GameBoard.PlayerColor.White)
                turnDisplay.GetComponentInChildren<TextMeshProUGUI>().text = "당신의 턴";
            else
                turnDisplay.GetComponentInChildren<TextMeshProUGUI>().text = "상대의 턴";
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
        turnDisplay.SetActive(true);
        yield return new WaitForSeconds(1f);
        turnDisplay.SetActive(false);
    }
}
