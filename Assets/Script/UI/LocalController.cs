using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;

public class LocalController : MonoBehaviour, IPointerClickHandler
{
    PhotonView photonView;

    SpriteRenderer spriteRenderer;
    public Sprite whiteButton;
    public Sprite blackButton;
    public PlayerController whiteController;
    public PlayerController blackController;
    public SoulOrb mySoulOrb;
    public SoulOrb opponentSoulrOrb;

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
            spriteRenderer.sprite = blackButton;

            whiteController.enabled = false;
            blackController.enabled = true;

            whiteController.TurnEnd();
            blackController.OpponentTurnEnd();

            blackController.TurnStart();
            whiteController.OpponentTurnStart();

            blackController.LocalDraw();
            whiteController.OpponentDraw();
        }
        else
        {
            spriteRenderer.sprite = whiteButton;

            blackController.enabled = false;
            whiteController.enabled = true;

            blackController.TurnEnd();
            whiteController.OpponentTurnEnd();

            whiteController.TurnStart();
            blackController.OpponentTurnStart();

            whiteController.LocalDraw();
            blackController.OpponentDraw();
        }
    }
}
