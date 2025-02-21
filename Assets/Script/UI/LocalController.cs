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
    private TurnChangeButtonHighlight turnChangeButtonHighlight;
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
    public Graveyard myGraveyard;
    public Graveyard opponentGraveyard;
    public SettingUI settingUI;

    private void Awake()
    {
        turnChangeButtonSR = turnChangeButton.GetComponent<SpriteRenderer>();
        turnChangeButtonHighlight = turnChangeButton.GetComponent<TurnChangeButtonHighlight>();

        photonView = GetComponent<PhotonView>();

        if (GameManager.instance.isHost)
        {
            mySoulOrb.playerColor = GameBoard.PlayerColor.White;
            opponentSoulOrb.playerColor = GameBoard.PlayerColor.Black;
            mySoulCost.playerColor = GameBoard.PlayerColor.White;
            opponentSoulCost.playerColor = GameBoard.PlayerColor.Black;
            whiteController.soulOrb = mySoulOrb;
            blackController.soulOrb = opponentSoulOrb;
            myGraveyard.playerColor = GameBoard.PlayerColor.White;
            opponentGraveyard.playerColor = GameBoard.PlayerColor.Black;

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
            myGraveyard.playerColor = GameBoard.PlayerColor.Black;
            opponentGraveyard.playerColor = GameBoard.PlayerColor.White;

            myTimer.playerColor = GameBoard.PlayerColor.Black;
            opponentTimer.playerColor = GameBoard.PlayerColor.White;
            blackController.chessTimer = myTimer;
            whiteController.chessTimer = opponentTimer;
            turnChangeButtonHighlight.buttonText.text = "상대 턴";
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            settingUI.ToggleSettingUI();
        }
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
            turnChangeButtonSR.color = new Color(0.4f, 0.4f, 0.4f, 1);
            turnChangeButtonHighlight.buttonText.color = Color.white;

            whiteController.TurnEnd();
            blackController.OpponentTurnEnd();

            whiteController.enabled = false;
            blackController.enabled = true;
            
            if (GameBoard.instance.playerColor == GameBoard.PlayerColor.White)
            {
                turnChangeButtonHighlight.buttonText.color = Color.white;
                turnChangeButtonHighlight.buttonText.text = "상대 턴";
                turnDisplay.GetComponentInChildren<TextMeshProUGUI>().text = "상대의 턴";
            }
            else
            {
                turnChangeButtonHighlight.buttonText.color = Color.white;
                turnChangeButtonHighlight.buttonText.text = "나의 턴";
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
            turnChangeButtonSR.color = Color.white;
            turnChangeButtonHighlight.buttonText.color = Color.black;

            blackController.TurnEnd();
            whiteController.OpponentTurnEnd();
            
            blackController.enabled = false;
            whiteController.enabled = true;

            if (GameBoard.instance.playerColor == GameBoard.PlayerColor.White)
            {
                turnChangeButtonHighlight.buttonText.color = Color.black;
                turnChangeButtonHighlight.buttonText.text = "나의 턴";
                turnDisplay.GetComponentInChildren<TextMeshProUGUI>().text = "당신의 턴";
            }
            else
            {
                turnChangeButtonHighlight.buttonText.color = Color.black;
                turnChangeButtonHighlight.buttonText.text = "상대 턴";
                turnDisplay.GetComponentInChildren<TextMeshProUGUI>().text = "상대의 턴";
            }
            StartCoroutine("TurnDisplayOnOff");

            whiteController.TurnStart();
            blackController.OpponentTurnStart();

            whiteController.LocalDraw();
            blackController.OpponentDraw();
        }

        turnChangeButtonHighlight.DisableHighlight();
    }

    private IEnumerator TurnDisplayOnOff()
    {
        turnDisplay.SetActive(true);
        yield return new WaitForSeconds(1f);
        turnDisplay.SetActive(false);
    }
}
