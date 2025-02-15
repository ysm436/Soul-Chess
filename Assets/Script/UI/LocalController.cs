using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class LocalController : MonoBehaviour, IPointerClickHandler
{
    PhotonView photonView;

    [SerializeField] private GameObject turnChangeButton;
    private SpriteRenderer turnChangeButtonSR;
    private Material turnChangeButtonMaterial;
    public Sprite whiteButton;
    public Sprite blackButton;
    public PlayerController whiteController;
    public PlayerController blackController;
    public SoulOrb mySoulOrb;
    public SoulOrb opponentSoulOrb;
    public SoulCost mySoulCost;
    public SoulCost opponentSoulCost;
    public ChessTimer myTimer;
    public ChessTimer opponentTimer;
    public GameObject turnDisplay;
    [SerializeField] private GameObject turnCaution;

    private void Awake()
    {
        turnChangeButtonSR = turnChangeButton.GetComponent<SpriteRenderer>();
        turnChangeButtonSR.sprite = whiteButton;
        turnChangeButtonMaterial = turnChangeButton.GetComponent<Renderer>().material;

        photonView = GetComponent<PhotonView>();

        if (GameManager.instance.isHost)
        {
            mySoulOrb.playerColor = GameBoard.PlayerColor.White;
            opponentSoulOrb.playerColor = GameBoard.PlayerColor.Black;
            mySoulCost.playerColor = GameBoard.PlayerColor.White;
            opponentSoulCost.playerColor = GameBoard.PlayerColor.Black;
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
            mySoulCost.playerColor = GameBoard.PlayerColor.Black;
            opponentSoulCost.playerColor = GameBoard.PlayerColor.White;
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
        if (eventData.button != PointerEventData.InputButton.Left)
            return;
        if (GameBoard.instance.myController.TurnEndPossible)
            TurnEnd();
        else
        {
            Debug.Log("Please Move Any Chess Piece at least Once");
            turnCaution.SetActive(true);
            Invoke("TurnEndCaution", 1f);
        }
    }

    private void TurnEndCaution()
    {
        turnCaution.SetActive(false);
    }

    public void TurnEnd()
    {
        if (GameBoard.instance.isActivePlayer)
        {
            GameBoard.instance.myController.CancelUseCard();
            GameBoard.instance.cancelButton.Hide();
            photonView.RPC("OnTurnEndClicked", RpcTarget.All);
        }
    }

    [PunRPC]
    private void OnTurnEndClicked()
    {
        if (whiteController.enabled)
        {
            turnChangeButtonSR.sprite = blackButton;

            whiteController.TurnEnd();
            blackController.OpponentTurnEnd();

            whiteController.enabled = false;
            blackController.enabled = true;
            
            if (GameBoard.instance.playerColor == GameBoard.PlayerColor.White)
            {
                TextMeshPro TurnButtonText = turnChangeButton.GetComponentInChildren<TextMeshPro>();
                TurnButtonText.color = Color.white;
                TurnButtonText.text = "상대 턴";
                turnDisplay.GetComponentInChildren<TextMeshProUGUI>().text = "상대의 턴";
            }
            else
            {
                TextMeshPro TurnButtonText = turnChangeButton.GetComponentInChildren<TextMeshPro>();
                TurnButtonText.color = Color.white;
                TurnButtonText.text = "턴 종료";
                turnDisplay.GetComponentInChildren<TextMeshProUGUI>().text = "당신의 턴";
            }
            StartCoroutine("TurnDisplayOnOff");

            blackController.TurnStart();
            whiteController.OpponentTurnStart();

            blackController.LocalDraw();
            whiteController.OpponentDraw();
        }
        else
        {
            turnChangeButtonSR.sprite = whiteButton;

            blackController.TurnEnd();
            whiteController.OpponentTurnEnd();
            
            blackController.enabled = false;
            whiteController.enabled = true;

            if (GameBoard.instance.playerColor == GameBoard.PlayerColor.White)
            {
                TextMeshPro TurnButtonText = turnChangeButton.GetComponentInChildren<TextMeshPro>();
                TurnButtonText.color = Color.black;
                TurnButtonText.text = "턴 종료";
                turnDisplay.GetComponentInChildren<TextMeshProUGUI>().text = "당신의 턴";
            }
            else
            {
                TextMeshPro TurnButtonText = turnChangeButton.GetComponentInChildren<TextMeshPro>();
                TurnButtonText.color = Color.black;
                TurnButtonText.text = "상대 턴";
                turnDisplay.GetComponentInChildren<TextMeshProUGUI>().text = "상대의 턴";
            }
            StartCoroutine("TurnDisplayOnOff");

            whiteController.TurnStart();
            blackController.OpponentTurnStart();

            whiteController.LocalDraw();
            blackController.OpponentDraw();
        }

        turnChangeButtonMaterial.SetFloat("_InnerOutlineAlpha", 0f);
    }

    private IEnumerator TurnDisplayOnOff()
    {
        turnDisplay.SetActive(true);
        yield return new WaitForSeconds(1f);
        turnDisplay.SetActive(false);
    }
}
