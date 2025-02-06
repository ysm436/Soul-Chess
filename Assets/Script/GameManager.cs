using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    static public GameManager instance;

    public bool isHost;
    public GameBoard gameBoard; //Used only in GameScene
    public List<Deck> deckList = new List<Deck>();
    public Deck selectedDeck = null;

    [HideInInspector]
    public GameObject[] AllCards;

    [Serializable]
    public class Data
    {
        public List<Deck> DeckData;
    }
    private const string PATH = "/Save/";
    private const string FILE_NAME = "DeckData.json";
    public SoundManager soundManager;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            soundManager = transform.GetChild(0).GetComponent<SoundManager>();
        }
        else
        {
            Destroy(gameObject);
        }

        LoadDeckData();
    }

    private void Start()
    {
        FindAllCards();
    }

    // Update is called once per frame
    void Update()
    {
        // Test Code
        if (Input.GetKeyDown(KeyCode.T))
        {
            SceneManager.LoadScene("TutorialScene");
            isHost = true;
        }
    }
    public void LoadMatchingSceneFromLobbyScene()
    {
        PhotonNetwork.LeaveRoom();
        LoadMatchingScene();
    }
    public void LoadMainScene()
    {
        SceneManager.LoadScene("MainScene");
        soundManager.PlayBgm("MainScene");
    }
    public void LoadDeckBuildingScene()
    {
        SceneManager.LoadScene("DeckBuildingScene");
        soundManager.PlayBgm("DeckBuildingScene");
    }
    public void LoadMatchingScene()
    {
        SceneManager.LoadScene("MatchingScene");
    }
    public void LoadLobbyScene()
    {
        SceneManager.LoadScene("LobbyScene");
        soundManager.PlayBgm("LobbyScene");
    }
    public void LoadGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void LoadPvELobbyScene()
    {
        SceneManager.LoadScene("PvELobbyScene");
    }
    public void LoadPvEGameScene()
    {
        SceneManager.LoadScene("PvEGameScene");
    }
    public void LoadCreditScene()
    {
        SceneManager.LoadScene("CreditScene");
    }
    public void SaveDeckData()
    {
        string path = Application.dataPath + PATH;
        Data tempdata = new Data();

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        tempdata.DeckData = instance.deckList;
        string JsonData = JsonUtility.ToJson(tempdata, true);
        File.WriteAllText(path + FILE_NAME, JsonData);
    }

    public void LoadDeckData()
    {
        string path = Application.dataPath + PATH;

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        if (!File.Exists(path + FILE_NAME))
        {
            Debug.Log("No file");
        }
        else
        {
            string JsonData = File.ReadAllText(path + FILE_NAME);

            Data tempdata = JsonUtility.FromJson<Data>(JsonData);

            if (tempdata != null)
            {
                instance.deckList = tempdata.DeckData;
            }
        }
    }
    // Greek, Norse, Western 폴더 내 존재하는 모든 카드를 찾습니다.
    private void FindAllCards()
    {
        int max_card_index = Card.cardIdDict.Values.Max();
        AllCards = new GameObject[max_card_index + 1];

        List<GameObject> AllCardObjectsList = new();
        AllCardObjectsList.AddRange(Resources.LoadAll<GameObject>("Greek"));
        AllCardObjectsList.AddRange(Resources.LoadAll<GameObject>("Norse"));
        AllCardObjectsList.AddRange(Resources.LoadAll<GameObject>("Western"));

        foreach (var g in AllCardObjectsList)
        {
            Debug.Log(g.GetComponent<Card>().cardName);
            Debug.Log(Card.cardIdDict[g.GetComponent<Card>().cardName]);
            AllCards[Card.cardIdDict[g.GetComponent<Card>().cardName]] = g;
        }
    }
    public List<Card> GetCardListFrom(List<int> deck)
    {
        List<Card> cards = new();

        foreach (int index in deck)
        {
            cards.Add(AllCards[index].GetComponent<Card>());
        }

        return cards;
    }
}
