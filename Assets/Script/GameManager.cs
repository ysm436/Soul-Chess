using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    static public GameManager instance;

    public bool isHost;
    public GameBoard gameBoard; //Used only in GameScene
    public List<Deck> deckList = new List<Deck>();
    public Deck selectedDeck = null;

    [Serializable]
    public class Data
    {
        public List<Deck> DeckData;
    }
    private const string PATH = "/Save/";
    private const string FILE_NAME = "DeckData.json";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        LoadDeckData();
    }


    // Update is called once per frame
    void Update()
    {

    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }
    public void LoadDeckBuildingScene()
    {
        SceneManager.LoadScene("DeckBuildingScene");
    }
    public void LoadMatchingScene()
    {
        SceneManager.LoadScene("MatchingScene");
    }
    public void LoadLobbyScene()
    {
        SceneManager.LoadScene("LobbyScene");
    }
    public void LoadGameScene()
    {
        SceneManager.LoadScene("GameScene");
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
}
