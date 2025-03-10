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

    [SerializeField] private GameObject turnChangeButton;
    private SpriteRenderer turnChangeButtonSR;
    private TurnChangeButtonHighlight turnChangeButtonHighlight;
    private Material turnChangeButtonMaterial;
    public Sprite whiteButton;
    public Sprite blackButton;
    public PlayerController whiteController;
    public PlayerController blackController;
    public SoulOrb mySoulOrb;
    public SoulOrb opponentSoulrOrb;
    public SoulCost mySoulCost;
    public SoulCost opponentSoulCost;
    public GameObject turnDisplay;
    public Graveyard myGraveyard;
    public Graveyard opponentGraveyard;
    public SettingUI settingUI;

    public TutorialManager tutorialManager;

    public SoulCard plunder;

    private int turn;

    private const float routineDelay = 1f;

    private void Start()
    {
        turn = 0;

        turnChangeButtonSR = turnChangeButton.GetComponent<SpriteRenderer>();
        turnChangeButtonHighlight = turnChangeButton.GetComponent<TurnChangeButtonHighlight>();

        if (GameManager.instance.isHost)
        {
            mySoulOrb.playerColor = GameBoard.PlayerColor.White;
            opponentSoulrOrb.playerColor = GameBoard.PlayerColor.Black;
            mySoulCost.playerColor = GameBoard.PlayerColor.White;
            opponentSoulCost.playerColor = GameBoard.PlayerColor.Black;
            myGraveyard.playerColor = GameBoard.PlayerColor.White;
            opponentGraveyard.playerColor = GameBoard.PlayerColor.Black;
            whiteController.soulOrb = mySoulOrb;
            blackController.soulOrb = opponentSoulrOrb;
        }
        else
        {
            mySoulOrb.playerColor = GameBoard.PlayerColor.Black;
            opponentSoulrOrb.playerColor = GameBoard.PlayerColor.White;
            mySoulCost.playerColor = GameBoard.PlayerColor.Black;
            opponentSoulCost.playerColor = GameBoard.PlayerColor.White;
            myGraveyard.playerColor = GameBoard.PlayerColor.Black;
            opponentGraveyard.playerColor = GameBoard.PlayerColor.White;
            blackController.soulOrb = mySoulOrb;
            whiteController.soulOrb = opponentSoulrOrb;
            turnChangeButtonHighlight.buttonText.text = "상대 턴";
        }

        whiteController.enabled = true;
        blackController.enabled = false;

        if (GameBoard.instance.playerColor == GameBoard.PlayerColor.White)
            turnDisplay.GetComponentInChildren<TextMeshProUGUI>().text = "당신의 턴";
        else
            turnDisplay.GetComponentInChildren<TextMeshProUGUI>().text = "상대의 턴";
        StartCoroutine("TurnDisplayOnOff");

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            settingUI.ToggleSettingUI();
        }
    }

    public void EnableTurnChangeButton()
    {
        tutorialManager.isAllowingTurnEnd = true;
        turnChangeButtonHighlight.tutorialFlag = true;
    }

    void IPointerClickHandler.OnPointerClick(UnityEngine.EventSystems.PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;
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
                tutorialManager.RemoveText();
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
        turnChangeButtonHighlight.DisableHighlightForTutorial();
    }

    private IEnumerator TurnDisplayOnOff()
    {
        turnDisplay.SetActive(true);
        yield return new WaitForSeconds(1f);
        turnDisplay.SetActive(false);
    }

    private IEnumerator EnemyFirstRoutine()
    {
        tutorialManager.RemoveShadow();
        yield return new WaitForSeconds(routineDelay);
        tutorialManager.MovePiece(6, 6, 6, 4, false);
        yield return new WaitForSeconds(routineDelay);
        OnTurnEndClicked();
    }

    private IEnumerator EnemySecondRoutine1()
    {
        tutorialManager.RemoveShadow();

        tutorialManager.RemoveText();

        yield return new WaitForSeconds(routineDelay);

        tutorialManager.MovePiece(6, 4, 5, 3, true);

        yield return new WaitForSeconds(0.5f);

        //tutorialManager.ProcessStep14();

        StartEnemySecondRoutine2();
    }

    public void StartEnemySecondRoutine2()
    {
        StartCoroutine("EnemySecondRoutine2");
    }

    private IEnumerator EnemySecondRoutine2()
    {
        tutorialManager.RemoveShadow();

        yield return new WaitForSeconds(routineDelay);

        SoulCard vikingInstance = Instantiate(tutorialManager.viking) as SoulCard;
        vikingInstance.Infuse(GameBoard.instance.gameData.GetPiece(new Vector2Int(5, 3)));
        var opponentHand = GameObject.Find("OpponentHand");
        opponentHand.transform.GetChild(opponentHand.transform.childCount - 1).gameObject.SetActive(false);

        StartCoroutine(EnemyShowCardRoutine(tutorialManager.viking));

        tutorialManager.UpdateOpponentHandPosition();

        yield return new WaitForSeconds(routineDelay);

        //tutorialManager.ProcessStep16();

        OnTurnEndClicked();
    }

    private IEnumerator EnemyThirdRoutine1()
    {
        tutorialManager.RemoveShadow();
        tutorialManager.RemoveText();

        yield return new WaitForSeconds(routineDelay);

        SoulCard plunderInstance = Instantiate(plunder) as SoulCard;
        plunderInstance.Infuse(GameBoard.instance.gameData.GetPiece(new Vector2Int(3, 6)));
        var opponentHand = GameObject.Find("OpponentHand");
        opponentHand.transform.GetChild(opponentHand.transform.childCount - 1).gameObject.SetActive(false);

        tutorialManager.UpdateOpponentHandPosition();

        StartCoroutine(EnemyShowCardRoutine(plunder));

        yield return new WaitForSeconds(routineDelay);

        //tutorialManager.ProcessStep22();

        StartEnemyThirdRoutine2();
    }
    public void StartEnemyThirdRoutine2()
    {
        StartCoroutine("EnemyThirdRoutine2");
    }
    private IEnumerator EnemyThirdRoutine2()
    {
        tutorialManager.RemoveShadow();
        tutorialManager.RemoveText();
        tutorialManager.RemoveNextButton();

        yield return new WaitForSeconds(routineDelay);

        tutorialManager.MovePiece(5, 3, 5, 2, false);

        yield return new WaitForSeconds(routineDelay);

        //tutorialManager.ProcessStep23();

        OnTurnEndClicked();
    }

    private IEnumerator EnemyShowCardRoutine(Card card)
    {
        tutorialManager.ShowCard(card);

        yield return new WaitForSeconds(1.5f);

        tutorialManager.RemoveShowCard();
    }
}
