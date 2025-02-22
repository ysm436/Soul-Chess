using DG.Tweening;
using Photon.Pun;
using TMPro;
using UnityEngine;
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
    public Button startButton;

    public GameObject whiteInfo;
    public GameObject blackInfo;
    public GameObject whiteLoading;
    public GameObject blackLoading;

    public int SelectedDeckIndex = -1;

    private bool isReady { get => GameManager.instance.isHost ? isWhiteReady : isBlackReady; }
    private bool isWhiteReady
    {
        get => _isWhiteReady;
        set
        {
            _isWhiteReady = value;
            TryActivateStartButton();
        }
    }
    private bool _isWhiteReady = false;
    private bool isBlackReady
    {
        get => _isBlackReady;
        set
        {
            _isBlackReady = value;
            TryActivateStartButton();
        }
    }
    public bool _isBlackReady = false;



    [SerializeField] private GameObject DeckSelectPanel;
    [SerializeField] private GameObject DeckSelectButton;
    [SerializeField] private Transform DeckDisplay;
    [SerializeField] private Transform TrashCan;
    [SerializeField] private TextMeshProUGUI SelectedDeckInfo;
    [SerializeField] private Transform CardListDisplay;
    [SerializeField] private GameObject CardInfoUIView; //이건 들어갈 scrollview
    [SerializeField] private GameObject cautionUI;
    public TextMeshProUGUI deckPanelSelectedDeckInfo;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        blackReadyButton.enabled = false;
        whiteReadyButton.enabled = false;
        blackReadyButton.GetComponent<Image>().color = new Color(0.7f, 0.7f, 0.7f, 0.7f);
        whiteReadyButton.GetComponent<Image>().color = new Color(0.7f, 0.7f, 0.7f, 0.7f);
    }
    bool isInitialzed = false;
    public void Init()
    {
        if (isInitialzed)
            return;

        if (GameManager.instance.isHost)
        {
            Debug.Log("is host");
            whiteReadyButton.enabled = true;
            whiteReadyButton.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            myCardText.gameObject.SetActive(true);
            if (PhotonNetwork.CurrentRoom.PlayerCount <= 1)
            {
                blackInfo.SetActive(false);
                blackLoading.SetActive(true);
            }
        }
        else
        {
            Debug.Log("is not host");
            blackReadyButton.enabled = true;
            blackReadyButton.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            myCardText.anchoredPosition = new Vector3(-myCardText.anchoredPosition.x, myCardText.anchoredPosition.y);
            myCardText.gameObject.SetActive(true);
            if (PhotonNetwork.CurrentRoom.PlayerCount <= 1)
            {
                whiteInfo.SetActive(false);
                whiteLoading.SetActive(true);
            }
        }

        //덱 선택해주세요 셰이더 활성화

        startButton.onClick.AddListener(() => networkManager.StartGame());

        isInitialzed = true;
    }

    private void TryActivateStartButton()
    {
        if (isBlackReady && isWhiteReady && GameManager.instance.isHost)
        {
            startButton.gameObject.SetActive(true);
            TextMeshProUGUI startButtonText = startButton.GetComponentInChildren<TextMeshProUGUI>();
            DOVirtual.Float(1f, -0.1f, 0.5f, (value) => {
                startButton.GetComponent<Image>().material.SetFloat("_FadeAmount", value);
                startButtonText.alpha = 1.0f - Mathf.InverseLerp(-0.1f, 1.0f, value);
            });
        }
    }


    public void ReadyButton()
    {
        if (SelectedDeckIndex == -1)
        {
            cautionUI.SetActive(true);
        }
        else
        {
            photonView.RPC("SetReady", RpcTarget.AllBuffered, GameManager.instance.isHost, !isReady);
        }
    }

    public void CloseCautionUI()
    {
        cautionUI.SetActive(false);
    }

    [PunRPC]
    public void SetReady(bool isHost, bool ready)
    {
        if (isHost)
        {
            Debug.Log("player white is " + (ready ? "" : "not ") + "ready");
            isWhiteReady = ready;
            if (ready)
            {
                whiteReadyButtonText.color = Color.red;
            }
            else
            {
                whiteReadyButtonText.color = Color.black;
                startButton.gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.Log("player black is " + (ready ? "" : "not ") + "ready");
            isBlackReady = ready;
            if (ready)
            {
                blackReadyButtonText.color = Color.red;
            }
            else
            {
                blackReadyButtonText.color = Color.black;
                startButton.gameObject.SetActive(false);
            }
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
                        buttoninfo.deckname.text = GameManager.instance.deckList[i].deckName;
                        buttoninfo.deckIndex = i;
                    }
                }
            }

            if (SelectedDeckIndex != -1)
            {
                var deckSelectButton = GetSelectedDeckButton();
                //선택되어있는게 있다면 쉐이더 활성화
                deckSelectButton.ControlShader(true);
            }
            //카드목록 띄운다면 여기서도 추가해야함
            ShowSelectedDeckCardList();
        }
    }

    public void ShowSelectedDeckCardList()
    {
        if (SelectedDeckIndex == -1)
        {
            CardInfoUIView.SetActive(false);
            return;
        }

        CardInfoUIView.SetActive(true);

        for (int i = 0; i < CardListDisplay.childCount; i++)
        {
            CardListDisplay.GetChild(i).gameObject.SetActive(false);
        }

        Deck selectedDeck = GameManager.instance.deckList[SelectedDeckIndex];

        int index = 0;

        foreach(var card in selectedDeck.cards)
        {
            var tmp = CardListDisplay.GetChild(index++);
            tmp.gameObject.SetActive(true);
            tmp.GetComponent<CardInfoUI>().SetCardInfoUI(GameManager.instance.AllCards[card].GetComponent<Card>());
        }
    }

    public DeckSelectButton GetSelectedDeckButton()
    {
        if (SelectedDeckIndex == -1)
            return null;

        DeckSelectButton selectedDeckButton = null;
        for (int i = 0; i < DeckDisplay.childCount; i++)
        {
            selectedDeckButton = DeckDisplay.GetChild(i).GetComponent<DeckSelectButton>();
            if (selectedDeckButton.deckIndex == SelectedDeckIndex)
                break;
        }
        
        Debug.Log(selectedDeckButton.deckIndex);
        return selectedDeckButton;
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
            //덱 선택해주세요 셰이더 활성화
            SelectedDeckInfo.text = "덱을 선택해 주세요.";
        }
        else
        {
            //덱 선택해주세요 셰이더 비활성화
            SelectedDeckInfo.text = "선택된 덱\n" + "<" + GameManager.instance.deckList[SelectedDeckIndex].deckName + ">";
            GameManager.instance.selectedDeck = GameManager.instance.deckList[SelectedDeckIndex];
        }
    }
    public void OnOtherPlayerJoined()
    {
        if (GameManager.instance.isHost)
        {
            blackInfo.SetActive(true);
            blackLoading.SetActive(false);
        }
        else
        {
            whiteInfo.SetActive(true);
            whiteLoading.SetActive(false);
        }
    }
}
