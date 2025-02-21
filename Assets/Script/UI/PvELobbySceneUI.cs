using DG.Tweening;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PvELobbySceneUI : MonoBehaviour
{
    private PhotonView photonView;
    public Button whiteReadyButton;
    public TextMeshProUGUI whiteReadyButtonText;
    public Button blackReadyButton;
    public TextMeshProUGUI blackReadyButtonText;
    public Button startButton;

    public Button exitButton;

    public int SelectedDeckIndex = -1;

    private bool isPlayerWhite
    {
        get => _isPlayerWhite;
        set 
        {
            _isPlayerWhite = value;
            GameManager.instance.isHost = value;
            ChangePlayerColor();
        }
    }

    private bool _isPlayerWhite = true;

    private bool isReady
    {
        get => _isReady;
        set
        {
            _isReady = value;
            TryActivateStartButton();
        }
    }
    private bool _isReady = false;



    [SerializeField] private GameObject DeckSelectPanel;
    [SerializeField] private GameObject DeckSelectButton;
    [SerializeField] private Transform DeckDisplay;
    [SerializeField] private Transform TrashCan;
    [SerializeField] private TextMeshProUGUI SelectedDeckInfo;
    [SerializeField] private Transform CardListDisplay;
    [SerializeField] private GameObject CardInfoUIView; //scrollview

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        GameManager.instance.isHost = true;
        blackReadyButton.enabled = false;
        blackReadyButton.GetComponent<Image>().color = new Color(0.7f,0.7f,0.7f,0.7f);
    }
    bool isInitialzed = false;
    private void Start()
    {
        startButton.onClick.AddListener(() => GameManager.instance.LoadPvEGameScene());
        exitButton.onClick.AddListener(() => GameManager.instance.LoadMainScene());
    }

    private void TryActivateStartButton()
    {
        if (isReady)
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
            Debug.Log("선택된 덱이 없습니다.");
        }
        else
        {
            SetReady(!isReady);
        }
    }

    public void SetReady(bool ready)
    {
        isReady = ready;
        if (isPlayerWhite)
        {
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
                deckSelectButton.ControlShader(true);
            }
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

        foreach (var card in selectedDeck.cards)
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

        return DeckDisplay.GetChild(SelectedDeckIndex).GetComponent<DeckSelectButton>();
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
            SelectedDeckInfo.text = "선택된 덱\n" + "<" + GameManager.instance.deckList[SelectedDeckIndex].deckName + ">";
            GameManager.instance.selectedDeck = GameManager.instance.deckList[SelectedDeckIndex];
        }
    }

    public void ColorChangeButton()
    {
        isPlayerWhite = !isPlayerWhite;
    }

    private void ChangePlayerColor()
    { 
        SetReady(false);
        if (isPlayerWhite)
        {
            whiteReadyButtonText.text = "준비";
            whiteReadyButton.enabled = true;
            whiteReadyButton.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            blackReadyButton.GetComponent<Image>().color = new Color(0.7f, 0.7f, 0.7f, 0.7f);
            blackReadyButton.enabled = false;
            blackReadyButtonText.text = "컴퓨터";
            blackReadyButtonText.color = Color.black;
        }
        else
        {
            blackReadyButtonText.text = "준비";
            whiteReadyButton.enabled = false;
            whiteReadyButton.GetComponent<Image>().color = new Color(0.7f, 0.7f, 0.7f, 0.7f);
            blackReadyButton.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            blackReadyButton.enabled = true;
            whiteReadyButtonText.text = "컴퓨터";
            whiteReadyButtonText.color = Color.black;
        }
    }
}
