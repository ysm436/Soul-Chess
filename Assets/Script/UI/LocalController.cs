using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class LocalController : MonoBehaviour, IPointerClickHandler
{
    PhotonView photonView;

    public GameObject turnChangeButton;
    private SpriteRenderer turnChangeButtonSR;
    private Material turnChangeButtonMaterial;
    public Sprite whiteButton;
    public Sprite blackButton;
    public PlayerController whiteController;
    public PlayerController blackController;
    public SoulOrb mySoulOrb;
    public SoulOrb opponentSoulrOrb;
    public GameObject turn_display;

    private void Awake()
    {
        turnChangeButtonSR = turnChangeButton.GetComponent<SpriteRenderer>();
        turnChangeButtonSR.sprite = whiteButton;
        turnChangeButtonMaterial = turnChangeButton.GetComponent<Renderer>().material;

        photonView = GetComponent<PhotonView>();

        if (GameManager.instance.isHost)
        {
            mySoulOrb.playerColor = GameBoard.PlayerColor.White;
            opponentSoulrOrb.playerColor = GameBoard.PlayerColor.Black;
            whiteController.soulOrb = mySoulOrb;
            blackController.soulOrb = opponentSoulrOrb;
        }
        else
        {
            mySoulOrb.playerColor = GameBoard.PlayerColor.Black;
            opponentSoulrOrb.playerColor = GameBoard.PlayerColor.White;
            blackController.soulOrb = mySoulOrb;
            whiteController.soulOrb = opponentSoulrOrb;
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
                turn_display.GetComponentInChildren<TextMeshProUGUI>().text = "상대의 턴";
            }
            else
            {
                TextMeshPro TurnButtonText = turnChangeButton.GetComponentInChildren<TextMeshPro>();
                TurnButtonText.color = Color.white;
                TurnButtonText.text = "턴 종료";
                turn_display.GetComponentInChildren<TextMeshProUGUI>().text = "당신의 턴";
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
                turn_display.GetComponentInChildren<TextMeshProUGUI>().text = "당신의 턴";
            }
            else
            {
                TextMeshPro TurnButtonText = turnChangeButton.GetComponentInChildren<TextMeshPro>();
                TurnButtonText.color = Color.black;
                TurnButtonText.text = "상대 턴";
                turn_display.GetComponentInChildren<TextMeshProUGUI>().text = "상대의 턴";
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
        turn_display.SetActive(true);
        yield return new WaitForSeconds(1f);
        turn_display.SetActive(false);
    }
}
