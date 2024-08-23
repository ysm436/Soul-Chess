using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbySceneUI : MonoBehaviour
{
    private PhotonView photonView;
    public NetworkManager networkManager;
    public Button whiteReadyButton;
    public TextMeshProUGUI whiteReadyButtonText;
    public Button blackReadyButton;
    public TextMeshProUGUI blackReadyButtonText;
    public RectTransform myCardText;

    public int SelectedDeckIndex = -1;

    private bool isReady { get => GameManager.instance.isHost ? isWhiteReady : isBlackReady; }
    private bool isWhiteReady
    {
        get => _isWhiteReady;
        set
        {
            _isWhiteReady = value;
            TryStartGame();
        }
    }
    private bool _isWhiteReady = false;
    private bool isBlackReady
    {
        get => _isBlackReady;
        set
        {
            _isBlackReady = value;
            TryStartGame();
        }
    }
    private bool _isBlackReady = false;



    [SerializeField] private GameObject DeckSelectPanel;
    [SerializeField] private GameObject DeckSelectButton;
    [SerializeField] private Transform DeckDisplay;
    [SerializeField] private Transform TrashCan;
    [SerializeField] private TextMeshProUGUI SelectedDeckInfo;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        blackReadyButton.enabled = false;
        whiteReadyButton.enabled = false;
    }
    public void Init()
    {
        if (GameManager.instance.isHost)
        {
            whiteReadyButton.enabled = true;
            myCardText.gameObject.SetActive(true);
        }
        else
        {
            blackReadyButton.enabled = true;
            myCardText.anchoredPosition = new Vector3(-myCardText.anchoredPosition.x, myCardText.anchoredPosition.y);
            myCardText.gameObject.SetActive(true);
        }
    }

    private void TryStartGame()
    {
        if (isBlackReady && isWhiteReady && GameManager.instance.isHost)
            networkManager.StartGame();
    }

    public void ExitButton()
    {
        PhotonNetwork.AutomaticallySyncScene = false;
        GameManager.instance.LoadMainScene();
    }

    public void ReadyButton()
    {
        if (SelectedDeckIndex == -1)
        {
            Debug.Log("선택된 덱이 없습니다.");
        }
        else
        {
            photonView.RPC("SetReady", RpcTarget.AllBuffered, GameManager.instance.isHost, !isReady);
        }
    }

    [PunRPC]
    public void SetReady(bool isHost, bool ready)
    {
        if (isHost)
        {
            Debug.Log("player white is " + (ready ? "" : "not ") + "ready");
            isWhiteReady = ready;
            if (ready)
                whiteReadyButtonText.color = Color.red;
            else
                whiteReadyButtonText.color = Color.black;
        }
        else
        {
            Debug.Log("player black is " + (ready ? "" : "not ") + "ready");
            isBlackReady = ready;
            if (ready)
                blackReadyButtonText.color = Color.red;
            else

                blackReadyButtonText.color = Color.black;
        }
    }

    public void OpenDeckButton()
    {
        if (DeckSelectPanel != null)
        {
            DeckSelectPanel.SetActive(true);

            if (GameManager.instance.deckList.Count > 0)
            {
                for (int i = 0; i < GameManager.instance.deckList.Count; i++)
                {
                    if (GameManager.instance.deckList[i].index != -1)
                    {
                        GameObject deckselectbutton = Instantiate(DeckSelectButton, DeckDisplay);
                        DeckSelectButton buttoninfo = deckselectbutton.GetComponent<DeckSelectButton>();
                        buttoninfo.deckname.text = GameManager.instance.deckList[i].deckname;
                        buttoninfo.deck_index = i;
                    }
                }
            }
        }
    }

    public void CloseDeckButton()
    {
        if (DeckSelectPanel != null)
        {
            DeckSelectPanel.SetActive(false);
        }

        for (int i = DeckDisplay.childCount; i > 0; i--)
        {
            Transform deck = DeckDisplay.GetChild(i - 1);
            Destroy(deck.gameObject);
        }

        if (SelectedDeckIndex == -1)
        {
            SelectedDeckInfo.text = "덱을 선택해 주세요.";
        }
        else
        {
            SelectedDeckInfo.text = "선택된 덱\n" + "<" + GameManager.instance.deckList[SelectedDeckIndex].deckname + ">";
        }

        GameManager.instance.selectedDeck = GameManager.instance.deckList[SelectedDeckIndex];
    }

}
