using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbySceneUI : MonoBehaviour
{
    private PhotonView photonView;
    public NetworkManager networkManager;

    public int SelectedDeckIndex = -1;
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
    }

    private void TryStartGame()
    {
        if (isBlackReady && isWhiteReady && GameManager.instance.isHost)
            networkManager.StartGame();
    }

    public void ExitButton()
    {
        GameManager.instance.LoadMatchingScene();
    }

    public void StartButton()
    {
        if (SelectedDeckIndex == -1)
        {
            Debug.Log("선택된 덱이 없습니다.");
        }
        else
        {
            photonView.RPC("SetReady", RpcTarget.AllBuffered, GameManager.instance.isHost, true);
        }
    }

    [PunRPC]
    public void SetReady(bool isHost, bool ready)
    {
        if (isHost)
        {
            Debug.Log("player white is " + (ready ? "" : "not ") + "ready");
            isWhiteReady = ready;
        }
        else
        {
            Debug.Log("player black is " + (ready ? "" : "not ") + "ready");
            isBlackReady = ready;
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
                    if (GameManager.instance.deckList[i] != null)
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

        SelectedDeckInfo.text = "Selected Deck Name : " + GameManager.instance.deckList[SelectedDeckIndex].deckname;

        GameManager.instance.selectedDeck = GameManager.instance.deckList[SelectedDeckIndex];
    }

}
