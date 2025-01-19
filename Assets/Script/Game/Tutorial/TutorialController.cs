using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UIElements;

public class TutorialController : MonoBehaviour, IPointerClickHandler
{
    PhotonView photonView;

    SpriteRenderer spriteRenderer;
    public Sprite whiteButton;
    public Sprite blackButton;
    public PlayerController whiteController;
    public PlayerController blackController;
    public TutorialButtonHighlight turnChangeButtonHighlight;
    public SoulOrb mySoulOrb;
    public SoulOrb opponentSoulrOrb;
    public GameObject turn_display;

    public TutorialManager tutorialManager;

    public TextMeshProUGUI descriptionText;

    public SoulCard plunder;

    private int turn;

    private void Start()
    {
        turn = 0;

        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = whiteButton;
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

        whiteController.enabled = true;
        blackController.enabled = false;

        if (GameBoard.instance.playerColor == GameBoard.PlayerColor.White)
            turn_display.GetComponentInChildren<TextMeshProUGUI>().text = "당신의 턴";
        else
            turn_display.GetComponentInChildren<TextMeshProUGUI>().text = "상대의 턴";
        StartCoroutine("TurnDisplayOnOff");
    }

    public void EnableTurnChangeButton()
    {
        tutorialManager.isAllowingTurnEnd = true;
        turnChangeButtonHighlight.gameObject.SetActive(true);
    }

    void IPointerClickHandler.OnPointerClick(UnityEngine.EventSystems.PointerEventData eventData)
    {
        Debug.Log("TurnEndClicked");
        if (tutorialManager.isMoved)
            TurnEnd();
        else
            Debug.Log("Please Move Any Chess Piece at least Once");
    }

    public void TurnEnd()
    {
        if (GameBoard.instance.isActivePlayer)
        {
            if (turn == 0)
            {
                descriptionText.text = "";
            }
            OnTurnEndClicked();
        }
            //photonView.RPC("OnTurnEndClicked", RpcTarget.All);
    }
    public void OnTurnEndClicked()
    {
        if (!tutorialManager.isAllowingTurnEnd)
        {
            return;
        }
        turn++;
        if (turn == 2)
        {
            tutorialManager.ProcessStep3();
        }

        if (turn == 4)
        {
            tutorialManager.ProcessStep17();
        }

        if (turn == 6)
        {
            tutorialManager.ProcessStep24();
        }

        if (whiteController.enabled)
        {

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

            if (turn == 1)
            {
                StartCoroutine("EnemyFirstRoutine");
            }
            if (turn == 3)
            {
                StartCoroutine("EnemySecondRoutine1");
            }
            if (turn == 5)
            {
                StartCoroutine("EnemyThirdRoutine1");
            }
        }
        else
        {
            tutorialManager.isAllowingTurnEnd = false;

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
        turnChangeButtonHighlight.gameObject.SetActive(false);
    }

    private IEnumerator TurnDisplayOnOff()
    {
        turn_display.SetActive(true);
        yield return new WaitForSeconds(1f);
        turn_display.SetActive(false);
    }

    private IEnumerator EnemyFirstRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        tutorialManager.MovePiece(6, 6, 6, 4, false);
        yield return new WaitForSeconds(0.5f);
        OnTurnEndClicked();
    }

    private IEnumerator EnemySecondRoutine1()
    {
        yield return new WaitForSeconds(0.5f);

        tutorialManager.MovePiece(6, 4, 5, 3, true);

        yield return new WaitForSeconds(0.5f);

        tutorialManager.ProcessStep14();
    }

    public void StartEnemySecondRoutine2()
    {
        StartCoroutine("EnemySecondRoutine2");
    }

    private IEnumerator EnemySecondRoutine2()
    {
        yield return new WaitForSeconds(1f);

        SoulCard vikingInstance = Instantiate(tutorialManager.viking) as SoulCard;
        vikingInstance.Infuse(GameBoard.instance.gameData.GetPiece(new Vector2Int(5, 3)));

        yield return new WaitForSeconds(0.5f);

        tutorialManager.ProcessStep16();
    }

    private IEnumerator EnemyThirdRoutine1()
    {
        tutorialManager.RemoveText();
        yield return new WaitForSeconds(1f);

        SoulCard plunderInstance = Instantiate(plunder) as SoulCard;
        plunderInstance.Infuse(GameBoard.instance.gameData.GetPiece(new Vector2Int(3, 6)));

        yield return new WaitForSeconds(0.5f);

        tutorialManager.ProcessStep22();
    }
    public void StartEnemyThirdRoutine2()
    {
        StartCoroutine("EnemyThirdRoutine2");
    }
    private IEnumerator EnemyThirdRoutine2()
    {
        tutorialManager.RemoveText();
        tutorialManager.RemoveNextButton();
        yield return new WaitForSeconds(0.5f);

        tutorialManager.MovePiece(5, 3, 5, 2, false);

        yield return new WaitForSeconds(0.5f);

        tutorialManager.ProcessStep23();
    }
}
